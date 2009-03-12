//----------------------------------------------------------------------------
// Anti-Grain Geometry - Version 2.4
// Copyright (C) 2002-2005 Maxim Shemanarev (http://www.antigrain.com)
//
// C# Port port by: Lars Brubaker
//                  larsbrubaker@gmail.com
// Copyright (C) 2007
//
// Permission to copy, use, modify, sell and distribute this software 
// is granted provided this copyright notice appears in all copies. 
// This software is provided "as is" without express or implied
// warranty, and with no claim as to its suitability for any purpose.
//
//----------------------------------------------------------------------------
// Contact: mcseem@antigrain.com
//          mcseemagg@yahoo.com
//          http://www.antigrain.com
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Pictor.PixelFormat;
using Pictor.Transform;
using Pictor.VertexSource;

namespace Pictor.UI
{
    public class UIWidget
    {
        private bool m_Visible = true;
        protected RectD m_Bounds = new RectD();
        protected UIWidget m_Parrent = null;

        protected Transform.Affine m_Transform = Affine.NewIdentity();
        public List<UIWidget> m_Children = new List<UIWidget>();

        private bool m_ContainsFocus = false;

        //private int m_CurrentChildIndex;

        public UIWidget()
        {
        }

        public RectD Bounds
        {
            get
            {
                return m_Bounds;
            }
            set
            {
                m_Bounds = value;
            }
        }

        public bool Visible
        {
            get
            {
                return m_Visible;

            }
            set
            {
                m_Visible = value;
            }
        }

        public UIWidget Parrent
        {
            get
            {
                return m_Parrent;
            }
        }

        public double Height
        {
            get
            {
                return m_Bounds.Height;
            }
        }

        public double Width
        {
            get
            {
                return m_Bounds.Width;
            }
        }

        public virtual Renderer GetRenderer()
        {
            if (m_Parrent != null)
            {
                return m_Parrent.GetRenderer();
            }

            return null;
        }

        public void AddChild(UIWidget child)
        {
            child.m_Parrent = this;
            m_Children.Add(child);
        }

        public void RemoveChild(UIWidget child)
        {
            child.m_Parrent = null;
            m_Children.Remove(child);
        }

        public virtual bool InRect(double x, double y)
        {
            PointToClient(ref x, ref y);
            if (m_Bounds.HitTest(x, y))
            {
                return true;
            }
            return false;
        }

        public virtual void Invalidate()
        {
            Parrent.Invalidate(Bounds);
        }

        public virtual void Invalidate(RectD rectToInvalidate)
        {
            Parrent.Invalidate(rectToInvalidate);
        }

        protected void Unfocus()
        {
            m_ContainsFocus = false;
            foreach (UIWidget child in m_Children)
            {
                child.Unfocus();
            }
        }

        public void Focus()
        {
            if (m_Parrent != null)
            {
                m_Parrent.Focus();
            }

            // make sure none of the children have focus.
            Unfocus();

            // now say that we do
            m_ContainsFocus = true;
        }

        public bool CanFocus
        {
            get
            {
                return true;
            }
        }

        public bool Focused
        {
            get
            {
                if (ContainsFocus)
                {
                    foreach (UIWidget child in m_Children)
                    {
                        if (child.ContainsFocus)
                        {
                            return false;
                        }
                    }

                    // we contain focus and none of our children do so we are focused.
                    return true;
                }

                return false;
            }
        }

        public bool ContainsFocus
        {
            get
            {
                return m_ContainsFocus;
            }
        }

        protected UIWidget GetChildContainingFocus()
        {
            foreach (UIWidget child in m_Children)
            {
                if (child.ContainsFocus)
                {
                    return child;
                }
            }

            return null;
        }

        public virtual void OnDraw()
        {
            for (int i = 0; i < m_Children.Count; i++)
            {
                UIWidget child = m_Children[i];
                if (child.Visible)
                {
                    GetRenderer().PushTransform();
                    Affine transform = GetRenderer().Transform;
                    transform *= GetTransform();
                    GetRenderer().Transform = transform;
                    child.OnDraw();
#if false
                    if (Focused)
                    {
                        RoundedRect rect = new RoundedRect(-5, -5, 5, 5, 0);
                        GetRenderer().Render(rect, new RGBA_Bytes(1.0, 0, 0, .5));
                    }
#endif

                    GetRenderer().PopTransform();
                }
            }
        }

        public virtual void OnClosed()
        {

        }

        public void PointToClient(ref double screenPointX, ref double screenPointY)
        {
            UIWidget prevGUIWidget = Parrent;
            while (prevGUIWidget != null)
            {
                prevGUIWidget.GetTransform().InverseTransform(ref screenPointX, ref screenPointY);
                prevGUIWidget = prevGUIWidget.Parrent;
            }
        }

        public void PointToScreen(ref PointD clientPoint)
        {
            UIWidget prevGUIWidget = this;
            while (prevGUIWidget != null)
            {
                prevGUIWidget.GetTransform().Transform(ref clientPoint);
                prevGUIWidget = prevGUIWidget.Parrent;
            }
        }

        public virtual void OnMouseDown(MouseEventArgs mouseEvent)
        {
            int i = 0;
            foreach (UIWidget child in m_Children)
            {
                if (child.Visible)
                {
                    child.OnMouseDown(mouseEvent);
                    if (child.InRect(mouseEvent.X, mouseEvent.Y))
                    {
                        if (child.CanFocus)
                        {
                            child.Focus();
                        }
                        return;
                    }
                }
                i++;
            }
        }

        public virtual void OnMouseMove(MouseEventArgs mouseEvent)
        {
            foreach (UIWidget child in m_Children)
            {
                if (child.Visible)
                {
                    child.OnMouseMove(mouseEvent);
                }
            }
        }

        public virtual void OnMouseUp(MouseEventArgs mouseEvent)
        {
            foreach (UIWidget child in m_Children)
            {
                if (child.Visible)
                {
                    child.OnMouseUp(mouseEvent);
                }
            }
        }

        public virtual void OnKeyPress(KeyPressEventArgs keyPressEvent)
        {
            UIWidget childWithFocus = GetChildContainingFocus();
            if (childWithFocus != null && childWithFocus.Visible)
            {
                childWithFocus.OnKeyPress(keyPressEvent);
            }
        }

        protected void FocusNext()
        {
            UIWidget childWithFocus = GetChildContainingFocus();
            for (int i = 0; i < m_Children.Count; i++)
            {
                UIWidget child = m_Children[i];
                if (child.Visible)
                {
                }
            }
        }

        protected void FocusPrevious()
        {

        }

        public virtual void OnKeyDown(KeyEventArgs keyEvent)
        {
            UIWidget childWithFocus = GetChildContainingFocus();
            if (childWithFocus != null && childWithFocus.Visible)
            {
                childWithFocus.OnKeyDown(keyEvent);
            }

            if (!keyEvent.Handled && keyEvent.KeyCode == Keys.Tab)
            {
                if (keyEvent.Shift)
                {
                    FocusPrevious();
                }
                else
                {
                    FocusNext();
                }
            }
        }

        public virtual void OnKeyUp(KeyEventArgs keyEvent)
        {
            UIWidget childWithFocus = GetChildContainingFocus();
            if (childWithFocus != null && childWithFocus.Visible)
            {
                childWithFocus.OnKeyUp(keyEvent);
            }
        }

        public bool SetChildCurrent(double x, double y)
        {
            for (int i = 0; i < m_Children.Count; i++)
            {
                UIWidget child = m_Children[i];
                if (child.Visible)
                {
                    if (child.InRect(x, y))
                    {
                        if (!child.Focused)
                        {
                            child.Focus();
                            return true;
                        }

                        return false;
                    }
                }
            }

            return false;
        }

        public Affine GetTransform()
        {
            return m_Transform;
        }

        public void SetTransform(Affine value)
        {
            m_Transform = value;
        }

        public double scale() { return GetTransform().GetScale(); }
    };

    abstract public class SimpleVertexSourceWidget : UIWidget, IVertexSource
    {
        public SimpleVertexSourceWidget(double x1, double y1, double x2, double y2)
        {
            Bounds = new RectD(x1, y1, x2, y2);
        }

        public abstract uint NumberOfPaths
        {
            get;
        }
        public abstract void Rewind(uint path_id);
        public abstract uint Vertex(out double x, out double y);

        public virtual IColorType Color(uint i) { return (IColorType)new RGBA_Doubles(); }

        public override void OnDraw()
        {
            Renderer rendererForSurfaceThisIsOn = Parrent.GetRenderer();

            for (uint i = 0; i < NumberOfPaths; i++)
            {
                rendererForSurfaceThisIsOn.Render(this, i, Color(i).GetAsRGBA_Bytes());
            }
            base.OnDraw();
        }

#if false
        public override void Render(Renderer renderer)
        {
            for (uint i = 0; i < num_paths(); i++)
            {
                renderer.Render(this, i, color(i).Get_rgba8());
            }
        }

        public void Render(IRasterizer rasterizer, IScanline scanline, IPixelFormat rendererBase)
        {
            uint i;
            for(i = 0; i < num_paths(); i++)
            {
                rasterizer.reset();
                rasterizer.add_path(this, i);
                Renderer.RenderSolid(rendererBase, rasterizer, scanline, color(i).Get_rgba8());
            }
        }
#endif
    }
}
