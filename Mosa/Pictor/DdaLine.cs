/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

namespace Pictor
{
    //===================================================DdaLineInterpolator
    public sealed class DdaLineInterpolator
    {
        int m_y;
        int m_inc;
        int m_dy;
        //int m_YShift;
        int m_FractionShift;

        //--------------------------------------------------------------------
        public DdaLineInterpolator(int FractionShift)
        {
            m_FractionShift = FractionShift;
        }

        //--------------------------------------------------------------------
        public DdaLineInterpolator(int y1, int y2, uint count, int FractionShift)
        {
            m_FractionShift = FractionShift;
            m_y = (y1);
            m_inc = (((y2 - y1) << m_FractionShift) / (int)(count));
            m_dy=(0);
        }

        //--------------------------------------------------------------------
        //public void operator ++ ()
        public void Next()
        {
            m_dy += m_inc;
        }

        //--------------------------------------------------------------------
        //public void operator -- ()
        public void Prev()
        {
            m_dy -= m_inc;
        }

        //--------------------------------------------------------------------
        //public void operator += (uint n)
        public void Next(uint n)
        {
            m_dy += m_inc * (int)n;
        }

        //--------------------------------------------------------------------
        //public void operator -= (uint n)
        public void Prev(int n)
        {
            m_dy -= m_inc * (int)n;
        }


        //--------------------------------------------------------------------
        public int y() { return m_y + (m_dy >> (m_FractionShift)); } // - m_YShift)); }
        public int dy() { return m_dy; }
    };

    //=================================================Dda2LineInterpolator
    public sealed class Dda2LineInterpolator
    {
        enum save_size_e { save_size = 2 };

        //--------------------------------------------------------------------
        public Dda2LineInterpolator() {}

        //-------------------------------------------- Forward-adjusted Line
        public Dda2LineInterpolator(int y1, int y2, int count)
        {
            m_cnt=(count <= 0 ? 1 : count);
            m_lft=((y2 - y1) / m_cnt);
            m_rem=((y2 - y1) % m_cnt);
            m_mod=(m_rem);
            m_y=(y1);

            if(m_mod <= 0)
            {
                m_mod += count;
                m_rem += count;
                m_lft--;
            }
            m_mod -= count;
        }

        //-------------------------------------------- Backward-adjusted Line
        public Dda2LineInterpolator(int y1, int y2, int count, int unused)
        {
            m_cnt=(count <= 0 ? 1 : count);
            m_lft=((y2 - y1) / m_cnt);
            m_rem=((y2 - y1) % m_cnt);
            m_mod=(m_rem);
            m_y=(y1);

            if(m_mod <= 0)
            {
                m_mod += count;
                m_rem += count;
                m_lft--;
            }
        }

        //-------------------------------------------- Backward-adjusted Line
        public Dda2LineInterpolator(int y, int count)
        {
            m_cnt=(count <= 0 ? 1 : count);
            m_lft=((y) / m_cnt);
            m_rem=((y) % m_cnt);
            m_mod=(m_rem);
            m_y=(0);

            if(m_mod <= 0)
            {
                m_mod += count;
                m_rem += count;
                m_lft--;
            }
        }

        /*
        //--------------------------------------------------------------------
        public void save(save_data_type* Data)
        {
            Data[0] = m_mod;
            Data[1] = m_y;
        }

        //--------------------------------------------------------------------
        public void load(save_data_type* Data)
        {
            m_mod = Data[0];
            m_y   = Data[1];
        }
         */

        //--------------------------------------------------------------------
        //public void operator++()
        public void Next()
        {
            m_mod += m_rem;
            m_y += m_lft;
            if(m_mod > 0)
            {
                m_mod -= m_cnt;
                m_y++;
            }
        }

        //--------------------------------------------------------------------
        //public void operator--()
        public void Prev()
        {
            if(m_mod <= m_rem)
            {
                m_mod += m_cnt;
                m_y--;
            }
            m_mod -= m_rem;
            m_y -= m_lft;
        }

        //--------------------------------------------------------------------
        public void adjust_forward()
        {
            m_mod -= m_cnt;
        }

        //--------------------------------------------------------------------
        public void adjust_backward()
        {
            m_mod += m_cnt;
        }

        //--------------------------------------------------------------------
        public int mod() { return m_mod; }
        public int rem() { return m_rem; }
        public int lft() { return m_lft; }

        //--------------------------------------------------------------------
        public int y() { return m_y; }

        private int m_cnt;
        private int m_lft;
        private int m_rem;
        private int m_mod;
        private int m_y;
    };


    //---------------------------------------------LineBresenhamInterpolator
    public sealed class LineBresenhamInterpolator
    {
        int m_x1_lr;
        int m_y1_lr;
        int m_x2_lr;
        int m_y2_lr;
        bool m_ver;
        uint m_len;
        int m_inc;
        Dda2LineInterpolator m_interpolator;

        public enum subpixel_scale_e
        {
            subpixel_shift = 8,
            subpixel_scale = 1 << subpixel_shift,
            subpixel_mask  = subpixel_scale - 1
        };

        //--------------------------------------------------------------------
        public static int line_lr(int v) { return v >> (int)subpixel_scale_e.subpixel_shift; }

        //--------------------------------------------------------------------
        public LineBresenhamInterpolator(int x1, int y1, int x2, int y2)
        {
            m_x1_lr=(line_lr(x1));
            m_y1_lr=(line_lr(y1));
            m_x2_lr=(line_lr(x2));
            m_y2_lr=(line_lr(y2));
            m_ver=(Math.Abs(m_x2_lr - m_x1_lr) < Math.Abs(m_y2_lr - m_y1_lr));
            if (m_ver)
            {
                m_len = (uint)Math.Abs(m_y2_lr - m_y1_lr);
            }
            else
            {
                m_len = (uint)Math.Abs(m_x2_lr - m_x1_lr);
            }
            
            m_inc=(m_ver ? ((y2 > y1) ? 1 : -1) : ((x2 > x1) ? 1 : -1));
            m_interpolator= new Dda2LineInterpolator(m_ver ? x1 : y1,
                           m_ver ? x2 : y2,
                           (int)m_len);
        }
    
        //--------------------------------------------------------------------
        public bool     is_ver() { return m_ver; }
        public uint len()    { return m_len; }
        public int      inc()    { return m_inc; }

        //--------------------------------------------------------------------
        public void hstep()
        {
            m_interpolator.Next();
            m_x1_lr += m_inc;
        }

        //--------------------------------------------------------------------
        public void vstep()
        {
            m_interpolator.Next();
            m_y1_lr += m_inc;
        }

        //--------------------------------------------------------------------
        public int x1() { return m_x1_lr; }
        public int y1() { return m_y1_lr; }
        public int x2() { return line_lr(m_interpolator.y()); }
        public int y2() { return line_lr(m_interpolator.y()); }
        public int x2_hr() { return m_interpolator.y(); }
        public int y2_hr() { return m_interpolator.y(); }
    };
}
