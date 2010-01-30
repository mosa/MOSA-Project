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
//
// classes cbox_ctrl
//
//----------------------------------------------------------------------------
using System;
using Pictor;
using Pictor.VertexSource;

namespace Pictor.UI
{
    //----------------------------------------------------------cbox_ctrl_impl
    public class CheckBoxWidget : SimpleVertexSourceWidget
    {
        private double m_text_thickness;
        private double m_FontSize;
        private String m_label;
        private bool m_status;
        private double[] m_vx = new double[32];
        private double[] m_vy = new double[32];

        private GsvText m_text;
        private StrokeConverter m_text_poly;

        private uint m_idx;
        private uint m_vertex;

        private RGBA_Doubles m_text_color;
        private RGBA_Doubles m_inactive_color;
        private RGBA_Doubles m_active_color;

        public CheckBoxWidget(double x, double y, string label)
            : base(x, y, x + 9.0 * 1.5, y + 9.0 * 1.5)
        {
            m_text_thickness = (1.5);
            m_FontSize = (9.0);
            m_status = (false);
            m_text = new GsvText();
            m_text_poly = new StrokeConverter(m_text);
            m_label = label;

            m_text_color = new RGBA_Doubles(0.0, 0.0, 0.0);
            m_inactive_color = new RGBA_Doubles(0.0, 0.0, 0.0);
            m_active_color = new RGBA_Doubles(0.4, 0.0, 0.0);
        }

        override public uint NumberOfPaths
        {
            get { return 3; }
        }
        override public void Rewind(uint idx)
        {
            m_idx = idx;

            double d2;
            double t;

            switch (idx)
            {
                default:
                case 0:                 // Border
                    m_vertex = 0;
                    m_vx[0] = m_Bounds.Left;
                    m_vy[0] = m_Bounds.Bottom;
                    m_vx[1] = m_Bounds.Right;
                    m_vy[1] = m_Bounds.Bottom;
                    m_vx[2] = m_Bounds.Right;
                    m_vy[2] = m_Bounds.Top;
                    m_vx[3] = m_Bounds.Left;
                    m_vy[3] = m_Bounds.Top;
                    m_vx[4] = m_Bounds.Left + m_text_thickness;
                    m_vy[4] = m_Bounds.Bottom + m_text_thickness;
                    m_vx[5] = m_Bounds.Left + m_text_thickness;
                    m_vy[5] = m_Bounds.Top - m_text_thickness;
                    m_vx[6] = m_Bounds.Right - m_text_thickness;
                    m_vy[6] = m_Bounds.Top - m_text_thickness;
                    m_vx[7] = m_Bounds.Right - m_text_thickness;
                    m_vy[7] = m_Bounds.Bottom + m_text_thickness;
                    break;

                case 1:
                    m_text.Text = m_label;
                    m_text.StartPoint(m_Bounds.Left + m_FontSize * 2.0, m_Bounds.Bottom + m_FontSize / 5.0);

                    m_text.SetFontSize(m_FontSize);
                    m_text_poly.Width(m_text_thickness);
                    m_text_poly.LineJoin(MathStroke.ELineJoin.round_join);
                    m_text_poly.LineCap(MathStroke.ELineCap.round_cap);
                    m_text_poly.Rewind(0);
                    break;

                case 2:                 // Active item
                    m_vertex = 0;
                    d2 = (m_Bounds.Top - m_Bounds.Bottom) / 2.0;
                    t = m_text_thickness * 1.5;
                    m_vx[0] = m_Bounds.Left + m_text_thickness;
                    m_vy[0] = m_Bounds.Bottom + m_text_thickness;
                    m_vx[1] = m_Bounds.Left + d2;
                    m_vy[1] = m_Bounds.Bottom + d2 - t;
                    m_vx[2] = m_Bounds.Right - m_text_thickness;
                    m_vy[2] = m_Bounds.Bottom + m_text_thickness;
                    m_vx[3] = m_Bounds.Left + d2 + t;
                    m_vy[3] = m_Bounds.Bottom + d2;
                    m_vx[4] = m_Bounds.Right - m_text_thickness;
                    m_vy[4] = m_Bounds.Top - m_text_thickness;
                    m_vx[5] = m_Bounds.Left + d2;
                    m_vy[5] = m_Bounds.Bottom + d2 + t;
                    m_vx[6] = m_Bounds.Left + m_text_thickness;
                    m_vy[6] = m_Bounds.Top - m_text_thickness;
                    m_vx[7] = m_Bounds.Left + d2 - t;
                    m_vy[7] = m_Bounds.Bottom + d2;
                    break;

            }
        }

        override public uint Vertex(out double x, out double y)
        {
            x = 0;
            y = 0;
            uint cmd = (uint)Path.EPathCommands.LineTo;
            switch (m_idx)
            {
                case 0:
                    if (m_vertex == 0 || m_vertex == 4) cmd = (uint)Path.EPathCommands.MoveTo;
                    if (m_vertex >= 8) cmd = (uint)Path.EPathCommands.Stop;
                    x = m_vx[m_vertex];
                    y = m_vy[m_vertex];
                    m_vertex++;
                    break;

                case 1:
                    cmd = m_text_poly.Vertex(out x, out y);
                    break;

                case 2:
                    if (m_status)
                    {
                        if (m_vertex == 0) cmd = (uint)Path.EPathCommands.MoveTo;
                        if (m_vertex >= 8) cmd = (uint)Path.EPathCommands.Stop;
                        x = m_vx[m_vertex];
                        y = m_vy[m_vertex];
                        m_vertex++;
                    }
                    else
                    {
                        cmd = (uint)Path.EPathCommands.Stop;
                    }
                    break;

                default:
                    cmd = (uint)Path.EPathCommands.Stop;
                    break;
            }

            if (!Path.IsStop(cmd))
            {
                GetTransform().InverseTransform(ref x, ref y);
            }
            return cmd;
        }

        public void TextThickness(double t) { m_text_thickness = t; }

        public void SetFontSize(double fontSize)
        {
            SetFontSizeAndWidthRatio(fontSize, 1);
        }

        public void SetFontSizeAndWidthRatio(double fontSize, double widthRatioOfHeight)
        {
            if (fontSize == 0 || widthRatioOfHeight == 0)
            {
                throw new System.Exception("You can't have a font with 0 width or height.  Nothing will render.");
            }
            m_FontSize = fontSize;
        }

        public String Label() { return m_label; }
        public void Label(String in_label)
        {
            m_label = in_label;
        }

        public bool Status() { return m_status; }
        public void Status(bool st) { m_status = st; }

        override public bool InRect(double x, double y)
        {
            GetTransform().InverseTransform(ref x, ref y);
            return Bounds.HitTest(x, y);
        }

        override public void OnMouseDown(MouseEventArgs mouseEvent)
        {
            double x = mouseEvent.X;
            double y = mouseEvent.Y;
            GetTransform().InverseTransform(ref x, ref y);
            if (Bounds.HitTest(x, y))
            {
                m_status = !m_status;
                mouseEvent.Handled = true;
            }
        }

        override public void OnMouseUp(MouseEventArgs mouseEvent)
        {
        }

        override public void OnMouseMove(MouseEventArgs mouseEvent)
        {
        }

        override public void OnKeyDown(KeyEventArgs keyEvent)
        {
            if (keyEvent.KeyCode == Keys.Space)
            {
                m_status = !m_status;
                keyEvent.Handled = true;
            }
        }

        public void TextColor(IColorType c) { m_text_color = c.GetAsRGBA_Doubles(); }
        public void InactiveColor(IColorType c) { m_inactive_color = c.GetAsRGBA_Doubles(); }
        public void ActiveColor(IColorType c) { m_active_color = c.GetAsRGBA_Doubles(); }

        override public IColorType Color(uint i)
        {
            switch (i)
            {
                case 0:
                    return m_inactive_color;

                case 1:
                    return m_text_color;

                case 2:
                    return m_active_color;

                default:
                    return m_inactive_color;
            }
        }
    };
}
