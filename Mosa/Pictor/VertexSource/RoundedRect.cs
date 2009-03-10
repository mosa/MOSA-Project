/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
namespace Pictor.VertexSource
{
    //------------------------------------------------------------rounded_rect
    //
    // See Implemantation agg_rounded_rect.cpp
    //
    public class RoundedRect : IVertexSource
    {
        RectD m_Bounds;
        double m_rx1;
        double m_ry1;
        double m_rx2;
        double m_ry2;
        double m_rx3;
        double m_ry3;
        double m_rx4;
        double m_ry4;
        uint m_status;
        Arc      m_arc = new Arc();

        public RoundedRect(double left, double bottom, double right, double top, double r)
        {
            m_Bounds.Left = (left); m_Bounds.Bottom = (bottom); m_Bounds.Right = (right); m_Bounds.Top = (top);
            m_rx1=(r); m_ry1=(r); m_rx2=(r); m_ry2=(r);
            m_rx3=(r); m_ry3=(r); m_rx4=(r); m_ry4=(r);

            if (left > right) { m_Bounds.Left = right; m_Bounds.Right = left; }
            if (bottom > top) { m_Bounds.Bottom = top; m_Bounds.Top = bottom; }
        }

        public RoundedRect(RectD bounds, double r)
            : this(bounds.x1, bounds.y1, bounds.x2, bounds.y2, r)
        {
        }

        public void Rectangle(double left, double bottom, double right, double top)
        {
            m_Bounds.Left = left;
            m_Bounds.Bottom = bottom;
            m_Bounds.Right = right;
            m_Bounds.Top = top;
            if (left > right) { m_Bounds.Left = right; m_Bounds.Right = left; }
            if (bottom > top) { m_Bounds.Bottom = top; m_Bounds.Top = bottom; }
        }

        public void Radius(double r)
        {
            m_rx1 = m_ry1 = m_rx2 = m_ry2 = m_rx3 = m_ry3 = m_rx4 = m_ry4 = r; 
        }

        public void Radius(double rx, double ry)
        {
            m_rx1 = m_rx2 = m_rx3 = m_rx4 = rx; 
            m_ry1 = m_ry2 = m_ry3 = m_ry4 = ry; 
        }

        public void Radius(double rx_bottom, double ry_bottom, double rx_top, double ry_top)
        {
            m_rx1 = m_rx2 = rx_bottom; 
            m_rx3 = m_rx4 = rx_top; 
            m_ry1 = m_ry2 = ry_bottom; 
            m_ry3 = m_ry4 = ry_top; 
        }

        public void Radius(double rx1, double ry1, double rx2, double ry2, 
                              double rx3, double ry3, double rx4, double ry4)
        {
            m_rx1 = rx1; m_ry1 = ry1; m_rx2 = rx2; m_ry2 = ry2; 
            m_rx3 = rx3; m_ry3 = ry3; m_rx4 = rx4; m_ry4 = ry4;
        }

        public void NormalizeRadius()
        {
            double dx = Math.Abs(m_Bounds.Top - m_Bounds.Bottom);
            double dy = Math.Abs(m_Bounds.Right - m_Bounds.Left);

            double k = 1.0;
            double t;
            t = dx / (m_rx1 + m_rx2); if(t < k) k = t; 
            t = dx / (m_rx3 + m_rx4); if(t < k) k = t; 
            t = dy / (m_ry1 + m_ry2); if(t < k) k = t; 
            t = dy / (m_ry3 + m_ry4); if(t < k) k = t; 

            if(k < 1.0)
            {
                m_rx1 *= k; m_ry1 *= k; m_rx2 *= k; m_ry2 *= k;
                m_rx3 *= k; m_ry3 *= k; m_rx4 *= k; m_ry4 *= k;
            }
        }

        public double ApproximationScale
        {
            get { return m_arc.ApproximationScale; }
            set { m_arc.ApproximationScale = value; }
        }

        public void Rewind(uint unused)
        {
            m_status = 0;
        }

        public uint Vertex(out double x, out double y)
        {
            x = 0;
            y = 0;
            uint cmd = (uint)Path.EPathCommands.Stop;
            switch (m_status)
            {
                case 0:
                    m_arc.Init(m_Bounds.Left + m_rx1, m_Bounds.Bottom + m_ry1, m_rx1, m_ry1,
                               Math.PI, Math.PI + Math.PI  * 0.5);
                    m_arc.Rewind(0);
                    m_status++;
                    goto case 1;

                case 1:
                    cmd = m_arc.Vertex(out x, out y);
                    if (Path.IsStop(cmd))
                    {
                        m_status++;
                    }
                    else
                    {
                        return cmd;
                    }
                    goto case 2;

                case 2:
                    m_arc.Init(m_Bounds.Right - m_rx2, m_Bounds.Bottom + m_ry2, m_rx2, m_ry2,
                               Math.PI + Math.PI * 0.5, 0.0);
                    m_arc.Rewind(0);
                    m_status++;
                    goto case 3;

                case 3:
                    cmd = m_arc.Vertex(out x, out y);
                    if (Path.IsStop(cmd))
                    {
                        m_status++;
                    }
                    else
                    {
                        return (uint)Path.EPathCommands.LineTo;
                    }
                    goto case 4;

                case 4:
                    m_arc.Init(m_Bounds.Right - m_rx3, m_Bounds.Top - m_ry3, m_rx3, m_ry3,
                               0.0, Math.PI * 0.5);
                    m_arc.Rewind(0);
                    m_status++;
                    goto case 5;

                case 5:
                    cmd = m_arc.Vertex(out x, out y);
                    if (Path.IsStop(cmd))
                    {
                        m_status++;
                    }
                    else
                    {
                        return (uint)Path.EPathCommands.LineTo;
                    }
                    goto case 6;

                case 6:
                    m_arc.Init(m_Bounds.Left + m_rx4, m_Bounds.Top - m_ry4, m_rx4, m_ry4,
                               Math.PI * 0.5, Math.PI);
                    m_arc.Rewind(0);
                    m_status++;
                    goto case 7;

                case 7:
                    cmd = m_arc.Vertex(out x, out y);
                    if (Path.IsStop(cmd))
                    {
                        m_status++;
                    }
                    else
                    {
                        return (uint)Path.EPathCommands.LineTo;
                    }
                    goto case 8;

                case 8:
                    cmd = (uint)Path.EPathCommands.EndPoly
                        | (uint)Path.EPathFlags.Close
                        | (uint)Path.EPathFlags.CounterClockwise;
                    m_status++;
                    break;
            }
            return cmd;
        }
    };
}

