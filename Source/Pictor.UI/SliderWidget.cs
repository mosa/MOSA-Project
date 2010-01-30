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
// classes slider_ctrl_impl, slider_ctrl
//
//----------------------------------------------------------------------------
using System;
using Pictor.VertexSource;

namespace Pictor.UI
{
    //--------------------------------------------------------slider_ctrl_impl
    public class SliderWidget : SimpleVertexSourceWidget
    {
        double m_border_width;
        double m_border_extra;
        double m_text_thickness;
        double m_value;
        double m_preview_value;
        double m_min;
        double m_max;
        uint m_num_steps;
        bool m_descending;
        string m_label = "";
        double m_xs1;
        double m_ys1;
        double m_xs2;
        double m_ys2;
        double m_pdx;
        protected bool m_mouse_move;
        private RGBA_Doubles m_background_color;
        private RGBA_Doubles m_triangle_color;
        private RGBA_Doubles m_text_color;
        private RGBA_Doubles m_pointer_preview_color;
        private RGBA_Doubles m_pointer_color;

        double[] m_vx = new double[32];
        double[] m_vy = new double[32];

        VertexSource.Ellipse m_ellipse;

        uint m_idx;
        uint m_vertex;

        GsvText m_text;
        StrokeConverter m_text_poly;
        PathStorage m_storage;

        public SliderWidget(double x1, double y1, double x2, double y2)
            : base(x1, y1, x2, y2)
        {
            m_border_width = 1.0;
            m_border_extra = ((y2 - y1) / 2);
            m_text_thickness = (1.0);
            m_pdx = (0.0);
            m_mouse_move = false;
            m_value = (0.5);
            m_preview_value = (0.5);
            m_min = (0.0);
            m_max = (1.0);
            m_num_steps = (0);
            m_descending = (false);
            m_ellipse = new VertexSource.Ellipse();
            m_storage = new PathStorage();
            m_text = new GsvText();
            m_text_poly = new StrokeConverter(m_text);

            CalculateBox();

            m_background_color = (new RGBA_Doubles(1.0, 0.9, 0.8));
            m_triangle_color = (new RGBA_Doubles(0.7, 0.6, 0.6));
            m_text_color = (new RGBA_Doubles(0.0, 0.0, 0.0));
            m_pointer_preview_color = (new RGBA_Doubles(0.6, 0.4, 0.4, 0.4));
            m_pointer_color = (new RGBA_Doubles(0.8, 0.0, 0.0, 0.6));
        }

        public void BorderWidth(double t, double extra)
        {
            m_border_width = t;
            m_border_extra = extra;
            CalculateBox();
        }

        public void Range(double min, double max) { m_min = min; m_max = max; }
        public void NumberOfSteps(uint num) { m_num_steps = num; }
        public void Label(String fmt)
        {
            m_label = fmt;
        }
        public void text_thickness(double t) { m_text_thickness = t; }

        public bool Descending() { return m_descending; }
        public void Descending(bool v) { m_descending = v; }

        public double Value() { return m_value * (m_max - m_min) + m_min; }
        public void Value(double value)
        {
            m_preview_value = (value - m_min) / (m_max - m_min);
            if (m_preview_value > 1.0) m_preview_value = 1.0;
            if (m_preview_value < 0.0) m_preview_value = 0.0;
            NormalizeValue(true);
        }

        public override bool InRect(double x, double y)
        {
            GetTransform().InverseTransform(ref x, ref y);
            return Bounds.HitTest(x, y);
        }

        private bool IsOver(double x, double y)
        {
            double xp = m_xs1 + (m_xs2 - m_xs1) * m_value;
            double yp = (m_ys1 + m_ys2) / 2.0;
            return (PictorMath.CalculateDistance(x, y, xp, yp) <= m_Bounds.Top - m_Bounds.Bottom);
        }

        public override void OnMouseDown(MouseEventArgs mouseEvent)
        {
            double x = mouseEvent.X;
            double y = mouseEvent.Y;
            GetTransform().InverseTransform(ref x, ref y);

            double xp = m_xs1 + (m_xs2 - m_xs1) * m_value;
            double yp = (m_ys1 + m_ys2) / 2.0;

            if (IsOver(x, y))
            {
                m_pdx = xp - x;
                m_mouse_move = true;
                mouseEvent.Handled = true;
            }
        }

        public override void OnMouseUp(MouseEventArgs mouseEvent)
        {
            if (m_mouse_move)
            {
                m_mouse_move = false;
                NormalizeValue(true);
                mouseEvent.Handled = true;
            }
        }

        public override void OnMouseMove(MouseEventArgs mouseEvent)
        {
            double x = mouseEvent.X;
            double y = mouseEvent.Y;
            GetTransform().InverseTransform(ref x, ref y);

            if (m_mouse_move)
            {
                double xp = x + m_pdx;
                m_preview_value = (xp - m_xs1) / (m_xs2 - m_xs1);
                if (m_preview_value < 0.0) m_preview_value = 0.0;
                if (m_preview_value > 1.0) m_preview_value = 1.0;
                mouseEvent.Handled = true;
            }
        }

        public override void OnKeyDown(KeyEventArgs keyEvent)
        {
            double d = 0.005;
            if (m_num_steps != 0)
            {
                d = 1.0 / m_num_steps;
            }

            if (keyEvent.KeyCode == Keys.Right
                || keyEvent.KeyCode == Keys.Up)
            {
                m_preview_value += d;
                if (m_preview_value > 1.0) m_preview_value = 1.0;
                NormalizeValue(true);
                keyEvent.Handled = true;
            }

            if (keyEvent.KeyCode == Keys.Left
                || keyEvent.KeyCode == Keys.Down)
            {
                m_preview_value -= d;
                if (m_preview_value < 0.0) m_preview_value = 0.0;
                NormalizeValue(true);
                keyEvent.Handled = true;
            }
        }

        // Vertex source interface
        public override uint NumberOfPaths
        {
            get { return 6; }
        }

        public override void Rewind(uint idx)
        {
            m_idx = idx;

            switch (idx)
            {
                default:

                case 0:                 // Background
                    m_vertex = 0;
                    m_vx[0] = m_Bounds.Left - m_border_extra;
                    m_vy[0] = m_Bounds.Bottom - m_border_extra;
                    m_vx[1] = m_Bounds.Right + m_border_extra;
                    m_vy[1] = m_Bounds.Bottom - m_border_extra;
                    m_vx[2] = m_Bounds.Right + m_border_extra;
                    m_vy[2] = m_Bounds.Top + m_border_extra;
                    m_vx[3] = m_Bounds.Left - m_border_extra;
                    m_vy[3] = m_Bounds.Top + m_border_extra;
                    break;

                case 1:                 // Triangle
                    m_vertex = 0;
                    if (m_descending)
                    {
                        m_vx[0] = m_Bounds.Left;
                        m_vy[0] = m_Bounds.Bottom;
                        m_vx[1] = m_Bounds.Right;
                        m_vy[1] = m_Bounds.Bottom;
                        m_vx[2] = m_Bounds.Left;
                        m_vy[2] = m_Bounds.Top;
                        m_vx[3] = m_Bounds.Left;
                        m_vy[3] = m_Bounds.Bottom;
                    }
                    else
                    {
                        m_vx[0] = m_Bounds.Left;
                        m_vy[0] = m_Bounds.Bottom;
                        m_vx[1] = m_Bounds.Right;
                        m_vy[1] = m_Bounds.Bottom;
                        m_vx[2] = m_Bounds.Right;
                        m_vy[2] = m_Bounds.Top;
                        m_vx[3] = m_Bounds.Left;
                        m_vy[3] = m_Bounds.Bottom;
                    }
                    break;

                case 2:
                    m_text.Text = m_label;
                    if (m_label.Length > 0)
                    {
                        string buf;
                        buf = string.Format(m_label, Value());
                        m_text.Text = buf;
                    }
                    m_text.StartPoint(m_Bounds.Left, m_Bounds.Bottom);
                    m_text.SetFontSize((m_Bounds.Top - m_Bounds.Bottom) * 1.2);
                    m_text_poly.Width(m_text_thickness);
                    m_text_poly.LineJoin(MathStroke.ELineJoin.round_join);
                    m_text_poly.LineCap(MathStroke.ELineCap.round_cap);
                    m_text_poly.Rewind(0);
                    break;

                case 3:                 // pointer preview
                    m_ellipse.Init(m_xs1 + (m_xs2 - m_xs1) * m_preview_value,
                        (m_ys1 + m_ys2) / 2.0,
                        m_Bounds.Top - m_Bounds.Bottom,
                        m_Bounds.Top - m_Bounds.Bottom,
                        32);
                    break;


                case 4:                 // pointer
                    NormalizeValue(false);
                    m_ellipse.Init(m_xs1 + (m_xs2 - m_xs1) * m_value,
                        (m_ys1 + m_ys2) / 2.0,
                        m_Bounds.Top - m_Bounds.Bottom,
                        m_Bounds.Top - m_Bounds.Bottom,
                        32);
                    m_ellipse.Rewind(0);
                    break;

                case 5:
                    m_storage.RemoveAll();
                    if (m_num_steps != 0)
                    {
                        uint i;
                        double d = (m_xs2 - m_xs1) / m_num_steps;
                        if (d > 0.004) d = 0.004;
                        for (i = 0; i < m_num_steps + 1; i++)
                        {
                            double x = m_xs1 + (m_xs2 - m_xs1) * i / m_num_steps;
                            m_storage.move_to(x, m_Bounds.Bottom);
                            m_storage.line_to(x - d * (m_Bounds.Right - m_Bounds.Left), m_Bounds.Bottom - m_border_extra);
                            m_storage.line_to(x + d * (m_Bounds.Right - m_Bounds.Left), m_Bounds.Bottom - m_border_extra);
                        }
                    }
                    break;
            }
        }

        public override uint Vertex(out double x, out double y)
        {
            x = 0;
            y = 0;
            uint PathAndFlags = (uint)Path.EPathCommands.LineTo;
            switch (m_idx)
            {
                case 0:
                    if (m_vertex == 0) PathAndFlags = (uint)Path.EPathCommands.MoveTo;
                    if (m_vertex >= 4) PathAndFlags = (uint)Path.EPathCommands.Stop;
                    x = m_vx[m_vertex];
                    y = m_vy[m_vertex];
                    m_vertex++;
                    break;

                case 1:
                    if (m_vertex == 0) PathAndFlags = (uint)Path.EPathCommands.MoveTo;
                    if (m_vertex >= 4) PathAndFlags = (uint)Path.EPathCommands.Stop;
                    x = m_vx[m_vertex];
                    y = m_vy[m_vertex];
                    m_vertex++;
                    break;

                case 2:
                    PathAndFlags = m_text_poly.Vertex(out x, out y);
                    //return (uint)Path.path_commands_e.path_cmd_stop;
                    break;

                case 3:
                case 4:
                    PathAndFlags = m_ellipse.Vertex(out x, out y);
                    break;

                case 5:
                    PathAndFlags = m_storage.Vertex(out x, out y);
                    break;

                default:
                    PathAndFlags = (uint)Path.EPathCommands.Stop;
                    break;
            }

            if (!Path.IsStop(PathAndFlags))
            {
                GetTransform().InverseTransform(ref x, ref y);
            }

            return PathAndFlags;
        }

        private void CalculateBox()
        {
            m_xs1 = m_Bounds.Left + m_border_width;
            m_ys1 = m_Bounds.Bottom + m_border_width;
            m_xs2 = m_Bounds.Right - m_border_width;
            m_ys2 = m_Bounds.Top - m_border_width;
        }

        private bool NormalizeValue(bool preview_value_flag)
        {
            bool ret = true;
            if (m_num_steps != 0)
            {
                int step = (int)(m_preview_value * m_num_steps + 0.5);
                ret = m_value != step / (double)(m_num_steps);
                m_value = step / (double)(m_num_steps);
            }
            else
            {
                m_value = m_preview_value;
            }

            if (preview_value_flag)
            {
                m_preview_value = m_value;
            }
            return ret;
        }

        public void BackgroundColor(RGBA_Doubles c) { m_background_color = c; }
        public void PointerColor(RGBA_Doubles c) { m_pointer_color = c; }

        public override IColorType Color(uint i)
        {
            switch (i)
            {
                case 0:
                    return m_background_color;

                case 1:
                    return m_triangle_color;

                case 2:
                    return m_text_color;

                case 3:
                    return m_pointer_preview_color;

                case 4:
                    return m_pointer_color;

                case 5:
                    return m_text_color;
            }

            return m_background_color;
        }
    };
}
