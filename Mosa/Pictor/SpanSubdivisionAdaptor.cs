/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;

namespace Pictor
{

    //=================================================SpanSubdivisionAdaptor
    public class SpanSubdivisionAdaptor : ISpanInterpolator
    {
        int m_subdiv_shift;
        int m_subdiv_size;
        int m_subdiv_mask;
        ISpanInterpolator m_interpolator;
        int      m_src_x;
        double   m_src_y;
        uint m_pos;
        uint m_len;

        const int subpixel_shift = 8;
        const int subpixel_scale = 1 << subpixel_shift;


        //----------------------------------------------------------------
        public SpanSubdivisionAdaptor(ISpanInterpolator interpolator)
            : this(interpolator, 4)
        {
        }

        public SpanSubdivisionAdaptor(ISpanInterpolator interpolator, int subdiv_shift)
        {
            m_subdiv_shift = subdiv_shift;
            m_subdiv_size = 1 << m_subdiv_shift;
            m_subdiv_mask = m_subdiv_size - 1;
            m_interpolator = interpolator;
        }

        public SpanSubdivisionAdaptor(ISpanInterpolator interpolator,
                             double x, double y, uint len,
                             int subdiv_shift)
            : this(interpolator, subdiv_shift)
        {
            Begin(x, y, len);
        }

        public void ReSynchronize(double xe, double ye, uint len)
        {
            throw new System.NotImplementedException();
        }

        //----------------------------------------------------------------
        public ISpanInterpolator Interpolator
        {
            get { return m_interpolator; }
            set { m_interpolator = value; }
        }

        //----------------------------------------------------------------
        public Transform.ITransform Transformer
        {
            get
            {
                return m_interpolator.Transformer;
            }
            set
            {
                m_interpolator.Transformer = value;
            }
        }

        //----------------------------------------------------------------
        public int SubdivisionShift
        {
            get { return m_subdiv_shift; }
            set
            {
                m_subdiv_shift = value;
                m_subdiv_size = 1 << m_subdiv_shift;
                m_subdiv_mask = m_subdiv_size - 1;
            }
        }
        //----------------------------------------------------------------
        public void Begin(double x, double y, uint len)
        {
            m_pos   = 1;
            m_src_x = Basics.Round(x * subpixel_scale) + subpixel_scale;
            m_src_y = y;
            m_len   = len;
            if(len > m_subdiv_size) len = (uint)m_subdiv_size;
            m_interpolator.Begin(x, y, len);
        }

        //----------------------------------------------------------------
        public void Next()
        {
            m_interpolator.Next();
            if(m_pos >= m_subdiv_size)
            {
                uint len = m_len;
                if(len > m_subdiv_size) len = (uint)m_subdiv_size;
                m_interpolator.ReSynchronize((double)m_src_x / (double)subpixel_scale + len, 
                                              m_src_y, 
                                              len);
                m_pos = 0;
            }
            m_src_x += subpixel_scale;
            ++m_pos;
            --m_len;
        }

        //----------------------------------------------------------------
        public void Coordinates(out int x, out int y)
        {
            m_interpolator.Coordinates(out x, out y);
        }

        //----------------------------------------------------------------
        public void LocalScale(out int x, out int y)
        {
            m_interpolator.LocalScale(out x, out y);
        }
    };
}