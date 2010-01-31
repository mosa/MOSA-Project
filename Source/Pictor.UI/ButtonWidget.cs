//----------------------------------------------------------------------------
// Anti-Grain Geometry - Version 2.4
// Copyright (C) 2007 Lars Brubaker
//                  larsbrubaker@gmail.com
//
// Permission to copy, use, modify, sell and distribute this software 
// is granted provided this copyright notice appears in all copies. 
// This software is provided "as is" without express or implied
// warranty, and with no claim as to its suitability for any purpose.
//
// classes ButtonWidget
//
//----------------------------------------------------------------------------
using System;
using Pictor;
using Pictor.VertexSource;
using Pictor.Transform;

namespace Pictor.UI
{
    //----------------------------------------------------------cbox_ctrl_impl
    public class ButtonWidget : UIWidget
    {
        TextWidget m_ButtonText;
        bool m_MouseDownOnButton = false;
        bool m_MouseOverButton = false;
        bool m_DrawHoverEffect = true;
        double m_X;
        double m_Y;
        double m_BorderWidth;
        double m_TextPadding;
        double m_BorderRadius;

        public delegate void ButtonEventHandler(ButtonWidget button);
        public event ButtonEventHandler ButtonClick;

        protected bool MouseDownOnButton
        {
            get { return m_MouseDownOnButton; }
            set { m_MouseDownOnButton = value; }
        }

        protected bool MouseOverButton
        {
            get { return m_MouseOverButton; }
            set { m_MouseOverButton = value; }
        }

        public double BorderWidth { get { return m_BorderWidth; } set { m_BorderWidth = value; } }
        public double TextPadding { get { return m_TextPadding; } set { m_TextPadding = value; } }
        public double BorderRadius { get { return m_BorderRadius; } set { m_BorderRadius = value; } }

        public ButtonWidget()
        {
        }

        public ButtonWidget(double x, double y, string lable)
            : this(x, y, lable, 16, 0, 3, 5)
        {
        }

        public ButtonWidget(double x, double y, string lable,
            double textHeight, double textPadding, double borderWidth, double borderRadius)
        {
            m_X = x;
            m_Y = y;
            Affine transform = GetTransform();
            transform.Translate(x, y);
            SetTransform(transform);
            m_ButtonText = new TextWidget(lable, 0, 0, textHeight);
            AddChild(m_ButtonText);

            TextPadding = textPadding;
            BorderWidth = borderWidth;
            BorderRadius = borderRadius;

            double totalExtra = BorderWidth + TextPadding;
            m_Bounds.Left = x - totalExtra;
            m_Bounds.Bottom = y - totalExtra;
            m_Bounds.Right = x + m_ButtonText.Width + totalExtra;
            m_Bounds.Top = y + m_ButtonText.Height + totalExtra;
        }

        public override void OnDraw()
        {
            RoundedRect rectBorder = new RoundedRect(m_Bounds, m_BorderRadius);
            GetRenderer().Render(rectBorder, new RGBA_Bytes(0, 0, 0));
            RectD insideBounds = Bounds;
            insideBounds.Inflate(-BorderWidth);
            RoundedRect rectInside = new RoundedRect(insideBounds, Math.Max(m_BorderRadius - BorderWidth, 0));
            RGBA_Bytes insideColor = new RGBA_Bytes(1.0, 1.0, 1.0);
            if (MouseOverButton)
            {
                if (MouseDownOnButton)
                {
                    insideColor = new RGBA_Bytes(255, 110, 110);
                }
                else
                {
                    insideColor = new RGBA_Bytes(225, 225, 255);
                }
            }

            GetRenderer().Render(rectInside, insideColor);

            base.OnDraw();
        }

        override public void OnMouseDown(MouseEventArgs mouseEvent)
        {
            if (InRect(mouseEvent.X, mouseEvent.Y))
            {
                MouseDownOnButton = true;
                MouseOverButton = true;
                mouseEvent.Handled = true;
            }
            else
            {
                MouseDownOnButton = false;
            }
        }

        override public void OnMouseUp(MouseEventArgs mouseEvent)
        {
            if (MouseDownOnButton
              && InRect(mouseEvent.X, mouseEvent.Y))
            {
                if (ButtonClick != null)
                {
                    ButtonClick(this);
                }
                mouseEvent.Handled = true;
            }

            MouseDownOnButton = false;
        }

        override public void OnMouseMove(MouseEventArgs mouseEvent)
        {
            if (InRect(mouseEvent.X, mouseEvent.Y))
            {
                if (!MouseOverButton)
                {
                    MouseOverButton = true;
                    if (m_DrawHoverEffect)
                    {
                        Invalidate();
                    }
                }
            }
            else
            {
                if (MouseOverButton)
                {
                    MouseOverButton = false;
                    if (m_DrawHoverEffect)
                    {
                        Invalidate();
                    }
                }
            }
        }

        override public void OnKeyDown(KeyEventArgs keyEvent)
        {
            if (keyEvent.KeyCode == Keys.Space)
            {
                if (ButtonClick != null)
                {
                    ButtonClick(this);
                }
                keyEvent.Handled = true;
            }
        }

        public virtual string Library
        {
            get { return "Core"; }
        }
    };
}
