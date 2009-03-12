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
// classes rbox_ctrl_impl, rbox_ctrl
//
//----------------------------------------------------------------------------
using System;
using Pictor.VertexSource;
using System.Collections.Generic;

namespace Pictor.UI
{
    //------------------------------------------------------------------------
    public class TextWidget : SimpleVertexSourceWidget
    {
        double m_BorderSize;
        double m_Thickness;
        double m_CapsHeight;

        double[] m_vx = new double[32];
        double[] m_vy = new double[32];

        GsvText m_text;
        StrokeConverter m_text_poly;
        IColorType m_text_color;

        uint m_idx;

        public TextWidget(string Text, double left, double bottom, double CapitalHeight)
            : base(0, 0, 0, 0)
        {
            m_text_color = (new RGBA_Doubles(0.0, 0.0, 0.0));
            m_BorderSize = CapitalHeight * .2;
            m_Thickness = CapitalHeight / 8;
            m_CapsHeight = CapitalHeight;
            m_text = new GsvText();
            m_text.Text = Text;
            m_text_poly = new StrokeConverter(m_text);
            m_idx = (0);
            double MinX, MinY, MaxX, MaxY;
            GetTextBounds(out MinX, out MinY, out MaxX, out MaxY);
            double FullWidth = MaxX - MinX + m_BorderSize * 2;
            double FullHeight = m_CapsHeight + m_text.AscenderHeight + m_text.DescenderHeight + m_BorderSize * 2;
            Bounds = new RectD(left, bottom, left + FullWidth, bottom + FullHeight);
        }

        public string Text
        {
            get
            {
                return m_text.Text;
            }
            set
            {
                m_text.Text = value;
            }
        }

        public void GetSize(int characterToMeasureStartIndexInclusive, int characterToMeasureEndIndexInclusive, out PointD offset)
        {
            m_text.GetSize(characterToMeasureStartIndexInclusive, characterToMeasureEndIndexInclusive, out offset);
        }

        // this will return the position to the left of the requested character.
        public PointD GetOffsetLeftOfCharacterIndex(int characterIndex)
        {
            PointD offset = new PointD(0, 0);
            if (characterIndex > 0)
            {
                m_text.GetSize(0, characterIndex - 1, out offset);
            }
            return offset;
        }

        // If the Text is "TEXT" and the position is less than half the distance to the center
        // of "T" the return value will be 0 if it is between the center of 'T' and the center of 'E'
        // it will be 1 and so on.
        public int GetCharacterIndex(double xOffset)
        {
            return 0;
        }

        public override bool InRect(double x, double y)
        {
            GetTransform().InverseTransform(ref x, ref y);
            return Bounds.HitTest(x, y);
        }

        public void GetTextBounds(out double min_x, out double min_y, out double max_x, out double max_y)
        {
            Rewind(0);
            double VertexX;
            double VertexY;
            uint cmd = Vertex(out VertexX, out VertexY);
            min_x = VertexX;
            min_y = VertexY;
            max_x = VertexX;
            max_y = VertexY;
            while (cmd != (uint)Path.EPathCommands.Stop)
            {
                cmd = Vertex(out VertexX, out VertexY) & (uint)Path.EPathCommands.Mask;
                if (Path.IsVertex(cmd))
                {
                    min_x = Math.Min(VertexX, min_x);
                    min_y = Math.Min(VertexY, min_y);
                    max_x = Math.Max(VertexX, max_x);
                    max_y = Math.Max(VertexY, max_y);
                }
            }
        }

        // Vertex source interface
        public override uint NumberOfPaths
        {
            get { return 1; }
        }
        public override void Rewind(uint idx)
        {
            m_idx = idx;

            switch (idx)
            {
                case 0:                 // Text
                    m_text.StartPoint(m_Bounds.Left + m_BorderSize, m_Bounds.Bottom + m_BorderSize + m_text.AscenderHeight);
                    m_text.SetFontSize(m_CapsHeight);
                    m_text_poly.Width(m_Thickness);
                    m_text_poly.LineJoin(MathStroke.ELineJoin.round_join);
                    m_text_poly.LineCap(MathStroke.ELineCap.round_cap);
                    m_text_poly.Rewind(0);
                    break;
            }
        }

        public override uint Vertex(out double x, out double y)
        {
            x = 0;
            y = 0;
            uint cmd = (uint)Path.EPathCommands.LineTo;
            switch (m_idx)
            {
                case 0:
                    cmd = m_text_poly.Vertex(out x, out y);
                    if (Path.IsStop(cmd))
                    {
                        m_text.StartPoint(m_Bounds.Left + m_BorderSize,
                                           m_Bounds.Bottom + m_BorderSize + m_text.AscenderHeight);

                        m_text_poly.Rewind(0);
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


        public IColorType TextColor
        {
            set { m_text_color = value; }
        }

        public override IColorType Color(uint i)
        {
            switch (i)
            {
                case 0:
                    return m_text_color;

                default:
                    throw new System.IndexOutOfRangeException("There is not a color for this index");
            }
        }
    };
}