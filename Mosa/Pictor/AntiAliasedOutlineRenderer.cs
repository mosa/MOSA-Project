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
    /*

    //===================================================distance_interpolator0
    class distance_interpolator0
    {
    public:
        //---------------------------------------------------------------------
        distance_interpolator0() {}
        distance_interpolator0(int x1, int y1, int x2, int y2, int x, int y) :
            m_dx(line_mr(x2) - line_mr(x1)),
            m_dy(line_mr(y2) - line_mr(y1)),
            m_dist((line_mr(x + line_subpixel_scale/2) - line_mr(x2)) * m_dy - 
                   (line_mr(y + line_subpixel_scale/2) - line_mr(y2)) * m_dx)
        {
            m_dx <<= line_mr_subpixel_shift;
            m_dy <<= line_mr_subpixel_shift;
        }

        //---------------------------------------------------------------------
        void inc_x() { m_dist += m_dy; }
        int  dist() { return m_dist; }

    private:
        //---------------------------------------------------------------------
        int m_dx;
        int m_dy;
        int m_dist;
    };

    //==================================================distance_interpolator00
    class distance_interpolator00
    {
    public:
        //---------------------------------------------------------------------
        distance_interpolator00() {}
        distance_interpolator00(int xc, int yc, 
                                int x1, int y1, int x2, int y2, 
                                int x,  int y) :
            m_dx1(line_mr(x1) - line_mr(xc)),
            m_dy1(line_mr(y1) - line_mr(yc)),
            m_dx2(line_mr(x2) - line_mr(xc)),
            m_dy2(line_mr(y2) - line_mr(yc)),
            m_dist1((line_mr(x + line_subpixel_scale/2) - line_mr(x1)) * m_dy1 - 
                    (line_mr(y + line_subpixel_scale/2) - line_mr(y1)) * m_dx1),
            m_dist2((line_mr(x + line_subpixel_scale/2) - line_mr(x2)) * m_dy2 - 
                    (line_mr(y + line_subpixel_scale/2) - line_mr(y2)) * m_dx2)
        {
            m_dx1 <<= line_mr_subpixel_shift;
            m_dy1 <<= line_mr_subpixel_shift;
            m_dx2 <<= line_mr_subpixel_shift;
            m_dy2 <<= line_mr_subpixel_shift;
        }

        //---------------------------------------------------------------------
        void inc_x() { m_dist1 += m_dy1; m_dist2 += m_dy2; }
        int  dist1() { return m_dist1; }
        int  dist2() { return m_dist2; }

    private:
        //---------------------------------------------------------------------
        int m_dx1;
        int m_dy1;
        int m_dx2;
        int m_dy2;
        int m_dist1;
        int m_dist2;
    };

    //===================================================distance_interpolator1
    class distance_interpolator1
    {
    public:
        //---------------------------------------------------------------------
        distance_interpolator1() {}
        distance_interpolator1(int x1, int y1, int x2, int y2, int x, int y) :
            m_dx(x2 - x1),
            m_dy(y2 - y1),
            m_dist(Round(double(x + line_subpixel_scale/2 - x2) * double(m_dy) - 
                          double(y + line_subpixel_scale/2 - y2) * double(m_dx)))
        {
            m_dx <<= line_subpixel_shift;
            m_dy <<= line_subpixel_shift;
        }

        //---------------------------------------------------------------------
        void inc_x() { m_dist += m_dy; }
        void dec_x() { m_dist -= m_dy; }
        void inc_y() { m_dist -= m_dx; }
        void dec_y() { m_dist += m_dx; }

        //---------------------------------------------------------------------
        void inc_x(int dy)
        {
            m_dist += m_dy; 
            if(dy > 0) m_dist -= m_dx; 
            if(dy < 0) m_dist += m_dx; 
        }

        //---------------------------------------------------------------------
        void dec_x(int dy)
        {
            m_dist -= m_dy; 
            if(dy > 0) m_dist -= m_dx; 
            if(dy < 0) m_dist += m_dx; 
        }

        //---------------------------------------------------------------------
        void inc_y(int dx)
        {
            m_dist -= m_dx; 
            if(dx > 0) m_dist += m_dy; 
            if(dx < 0) m_dist -= m_dy; 
        }

        void dec_y(int dx)
        //---------------------------------------------------------------------
        {
            m_dist += m_dx; 
            if(dx > 0) m_dist += m_dy; 
            if(dx < 0) m_dist -= m_dy; 
        }

        //---------------------------------------------------------------------
        int dist() { return m_dist; }
        int dx()   { return m_dx;   }
        int dy()   { return m_dy;   }

    private:
        //---------------------------------------------------------------------
        int m_dx;
        int m_dy;
        int m_dist;
    };





    //===================================================distance_interpolator2
    class distance_interpolator2
    {
    public:
        //---------------------------------------------------------------------
        distance_interpolator2() {}
        distance_interpolator2(int x1, int y1, int x2, int y2,
                               int sx, int sy, int x,  int y) :
            m_dx(x2 - x1),
            m_dy(y2 - y1),
            m_dx_start(line_mr(sx) - line_mr(x1)),
            m_dy_start(line_mr(sy) - line_mr(y1)),

            m_dist(Round(double(x + line_subpixel_scale/2 - x2) * double(m_dy) - 
                          double(y + line_subpixel_scale/2 - y2) * double(m_dx))),

            m_dist_start((line_mr(x + line_subpixel_scale/2) - line_mr(sx)) * m_dy_start - 
                         (line_mr(y + line_subpixel_scale/2) - line_mr(sy)) * m_dx_start)
        {
            m_dx       <<= line_subpixel_shift;
            m_dy       <<= line_subpixel_shift;
            m_dx_start <<= line_mr_subpixel_shift;
            m_dy_start <<= line_mr_subpixel_shift;
        }

        distance_interpolator2(int x1, int y1, int x2, int y2,
                               int ex, int ey, int x,  int y, int) :
            m_dx(x2 - x1),
            m_dy(y2 - y1),
            m_dx_start(line_mr(ex) - line_mr(x2)),
            m_dy_start(line_mr(ey) - line_mr(y2)),

            m_dist(Round(double(x + line_subpixel_scale/2 - x2) * double(m_dy) - 
                          double(y + line_subpixel_scale/2 - y2) * double(m_dx))),

            m_dist_start((line_mr(x + line_subpixel_scale/2) - line_mr(ex)) * m_dy_start - 
                         (line_mr(y + line_subpixel_scale/2) - line_mr(ey)) * m_dx_start)
        {
            m_dx       <<= line_subpixel_shift;
            m_dy       <<= line_subpixel_shift;
            m_dx_start <<= line_mr_subpixel_shift;
            m_dy_start <<= line_mr_subpixel_shift;
        }


        //---------------------------------------------------------------------
        void inc_x() { m_dist += m_dy; m_dist_start += m_dy_start; }
        void dec_x() { m_dist -= m_dy; m_dist_start -= m_dy_start; }
        void inc_y() { m_dist -= m_dx; m_dist_start -= m_dx_start; }
        void dec_y() { m_dist += m_dx; m_dist_start += m_dx_start; }

        //---------------------------------------------------------------------
        void inc_x(int dy)
        {
            m_dist       += m_dy; 
            m_dist_start += m_dy_start; 
            if(dy > 0)
            {
                m_dist       -= m_dx; 
                m_dist_start -= m_dx_start; 
            }
            if(dy < 0)
            {
                m_dist       += m_dx; 
                m_dist_start += m_dx_start; 
            }
        }

        //---------------------------------------------------------------------
        void dec_x(int dy)
        {
            m_dist       -= m_dy; 
            m_dist_start -= m_dy_start; 
            if(dy > 0)
            {
                m_dist       -= m_dx; 
                m_dist_start -= m_dx_start; 
            }
            if(dy < 0)
            {
                m_dist       += m_dx; 
                m_dist_start += m_dx_start; 
            }
        }

        //---------------------------------------------------------------------
        void inc_y(int dx)
        {
            m_dist       -= m_dx; 
            m_dist_start -= m_dx_start; 
            if(dx > 0)
            {
                m_dist       += m_dy; 
                m_dist_start += m_dy_start; 
            }
            if(dx < 0)
            {
                m_dist       -= m_dy; 
                m_dist_start -= m_dy_start; 
            }
        }

        //---------------------------------------------------------------------
        void dec_y(int dx)
        {
            m_dist       += m_dx; 
            m_dist_start += m_dx_start; 
            if(dx > 0)
            {
                m_dist       += m_dy; 
                m_dist_start += m_dy_start; 
            }
            if(dx < 0)
            {
                m_dist       -= m_dy; 
                m_dist_start -= m_dy_start; 
            }
        }

        //---------------------------------------------------------------------
        int dist()       { return m_dist;       }
        int dist_start() { return m_dist_start; }
        int dist_end()   { return m_dist_start; }

        //---------------------------------------------------------------------
        int dx()       { return m_dx;       }
        int dy()       { return m_dy;       }
        int dx_start() { return m_dx_start; }
        int dy_start() { return m_dy_start; }
        int dx_end()   { return m_dx_start; }
        int dy_end()   { return m_dy_start; }

    private:
        //---------------------------------------------------------------------
        int m_dx;
        int m_dy;
        int m_dx_start;
        int m_dy_start;

        int m_dist;
        int m_dist_start;
    };





    //===================================================distance_interpolator3
    class distance_interpolator3
    {
    public:
        //---------------------------------------------------------------------
        distance_interpolator3() {}
        distance_interpolator3(int x1, int y1, int x2, int y2,
                               int sx, int sy, int ex, int ey, 
                               int x,  int y) :
            m_dx(x2 - x1),
            m_dy(y2 - y1),
            m_dx_start(line_mr(sx) - line_mr(x1)),
            m_dy_start(line_mr(sy) - line_mr(y1)),
            m_dx_end(line_mr(ex) - line_mr(x2)),
            m_dy_end(line_mr(ey) - line_mr(y2)),

            m_dist(Round(double(x + line_subpixel_scale/2 - x2) * double(m_dy) - 
                          double(y + line_subpixel_scale/2 - y2) * double(m_dx))),

            m_dist_start((line_mr(x + line_subpixel_scale/2) - line_mr(sx)) * m_dy_start - 
                         (line_mr(y + line_subpixel_scale/2) - line_mr(sy)) * m_dx_start),

            m_dist_end((line_mr(x + line_subpixel_scale/2) - line_mr(ex)) * m_dy_end - 
                       (line_mr(y + line_subpixel_scale/2) - line_mr(ey)) * m_dx_end)
        {
            m_dx       <<= line_subpixel_shift;
            m_dy       <<= line_subpixel_shift;
            m_dx_start <<= line_mr_subpixel_shift;
            m_dy_start <<= line_mr_subpixel_shift;
            m_dx_end   <<= line_mr_subpixel_shift;
            m_dy_end   <<= line_mr_subpixel_shift;
        }

        //---------------------------------------------------------------------
        void inc_x() { m_dist += m_dy; m_dist_start += m_dy_start; m_dist_end += m_dy_end; }
        void dec_x() { m_dist -= m_dy; m_dist_start -= m_dy_start; m_dist_end -= m_dy_end; }
        void inc_y() { m_dist -= m_dx; m_dist_start -= m_dx_start; m_dist_end -= m_dx_end; }
        void dec_y() { m_dist += m_dx; m_dist_start += m_dx_start; m_dist_end += m_dx_end; }

        //---------------------------------------------------------------------
        void inc_x(int dy)
        {
            m_dist       += m_dy; 
            m_dist_start += m_dy_start; 
            m_dist_end   += m_dy_end;
            if(dy > 0)
            {
                m_dist       -= m_dx; 
                m_dist_start -= m_dx_start; 
                m_dist_end   -= m_dx_end;
            }
            if(dy < 0)
            {
                m_dist       += m_dx; 
                m_dist_start += m_dx_start; 
                m_dist_end   += m_dx_end;
            }
        }

        //---------------------------------------------------------------------
        void dec_x(int dy)
        {
            m_dist       -= m_dy; 
            m_dist_start -= m_dy_start; 
            m_dist_end   -= m_dy_end;
            if(dy > 0)
            {
                m_dist       -= m_dx; 
                m_dist_start -= m_dx_start; 
                m_dist_end   -= m_dx_end;
            }
            if(dy < 0)
            {
                m_dist       += m_dx; 
                m_dist_start += m_dx_start; 
                m_dist_end   += m_dx_end;
            }
        }

        //---------------------------------------------------------------------
        void inc_y(int dx)
        {
            m_dist       -= m_dx; 
            m_dist_start -= m_dx_start; 
            m_dist_end   -= m_dx_end;
            if(dx > 0)
            {
                m_dist       += m_dy; 
                m_dist_start += m_dy_start; 
                m_dist_end   += m_dy_end;
            }
            if(dx < 0)
            {
                m_dist       -= m_dy; 
                m_dist_start -= m_dy_start; 
                m_dist_end   -= m_dy_end;
            }
        }

        //---------------------------------------------------------------------
        void dec_y(int dx)
        {
            m_dist       += m_dx; 
            m_dist_start += m_dx_start; 
            m_dist_end   += m_dx_end;
            if(dx > 0)
            {
                m_dist       += m_dy; 
                m_dist_start += m_dy_start; 
                m_dist_end   += m_dy_end;
            }
            if(dx < 0)
            {
                m_dist       -= m_dy; 
                m_dist_start -= m_dy_start; 
                m_dist_end   -= m_dy_end;
            }
        }

        //---------------------------------------------------------------------
        int dist()       { return m_dist;       }
        int dist_start() { return m_dist_start; }
        int dist_end()   { return m_dist_end;   }

        //---------------------------------------------------------------------
        int dx()       { return m_dx;       }
        int dy()       { return m_dy;       }
        int dx_start() { return m_dx_start; }
        int dy_start() { return m_dy_start; }
        int dx_end()   { return m_dx_end;   }
        int dy_end()   { return m_dy_end;   }

    private:
        //---------------------------------------------------------------------
        int m_dx;
        int m_dy;
        int m_dx_start;
        int m_dy_start;
        int m_dx_end;
        int m_dy_end;

        int m_dist;
        int m_dist_start;
        int m_dist_end;
    };




    
    //================================================line_interpolator_aa_base
    template<class Renderer> class line_interpolator_aa_base
    {
    public:
        typedef Renderer renderer_type;

        //---------------------------------------------------------------------
        enum max_half_width_e
        { 
            max_half_width = 64
        };

        //---------------------------------------------------------------------
        line_interpolator_aa_base(renderer_type& ren, LineParameters& lp) :
            m_lp(&lp),
            m_li(lp.vertical ? line_dbl_hr(lp.x2 - lp.x1) :
                               line_dbl_hr(lp.y2 - lp.y1),
                 lp.vertical ? abs(lp.y2 - lp.y1) : 
                               abs(lp.x2 - lp.x1) + 1),
            m_ren(ren),
            m_len((lp.vertical == (lp.inc > 0)) ? -lp.len : lp.len),
            m_x(lp.x1 >> line_subpixel_shift),
            m_y(lp.y1 >> line_subpixel_shift),
            m_old_x(m_x),
            m_old_y(m_y),
            m_count((lp.vertical ? abs((lp.y2 >> line_subpixel_shift) - m_y) :
                                   abs((lp.x2 >> line_subpixel_shift) - m_x))),
            m_width(ren.subpixel_width()),
            //m_max_extent(m_width >> (line_subpixel_shift - 2)),
            m_max_extent((m_width + line_subpixel_mask) >> line_subpixel_shift),
            m_step(0)
        {
            Pictor::Dda2LineInterpolator li(0, lp.vertical ? 
                                              (lp.dy << Pictor::line_subpixel_shift) :
                                              (lp.dx << Pictor::line_subpixel_shift),
                                           lp.len);

            uint i;
            int stop = m_width + line_subpixel_scale * 2;
            for(i = 0; i < max_half_width; ++i)
            {
                m_dist[i] = li.y();
                if(m_dist[i] >= stop) break;
                ++li;
            }
            m_dist[i++] = 0x7FFF0000;
        }

        //---------------------------------------------------------------------
        template<class DI> int step_hor_base(DI& di)
        {
            ++m_li;
            m_x += m_lp->inc;
            m_y = (m_lp->y1 + m_li.y()) >> line_subpixel_shift;

            if(m_lp->inc > 0) di.inc_x(m_y - m_old_y);
            else              di.dec_x(m_y - m_old_y);

            m_old_y = m_y;

            return di.dist() / m_len;
        }

        //---------------------------------------------------------------------
        template<class DI> int step_ver_base(DI& di)
        {
            ++m_li;
            m_y += m_lp->inc;
            m_x = (m_lp->x1 + m_li.y()) >> line_subpixel_shift;

            if(m_lp->inc > 0) di.inc_y(m_x - m_old_x);
            else              di.dec_y(m_x - m_old_x);

            m_old_x = m_x;

            return di.dist() / m_len;
        }

        //---------------------------------------------------------------------
        bool vertical() { return m_lp->vertical; }
        int  Width() { return m_width; }
        int  count() { return m_count; }

    private:
        line_interpolator_aa_base(line_interpolator_aa_base<Renderer>&);
        line_interpolator_aa_base<Renderer>& 
            operator = (line_interpolator_aa_base<Renderer>&);

    protected:
        LineParameters* m_lp;
        Dda2LineInterpolator m_li;
        renderer_type&         m_ren;
        int m_len;
        int m_x;
        int m_y;
        int m_old_x;
        int m_old_y;
        int m_count;
        int m_width;
        int m_max_extent;
        int m_step;
        int m_dist[max_half_width + 1];
        byte m_covers[max_half_width * 2 + 4];
    };







    //====================================================line_interpolator_aa0
    template<class Renderer> class line_interpolator_aa0 :
    public line_interpolator_aa_base<Renderer>
    {
    public:
        typedef Renderer renderer_type;
        typedef line_interpolator_aa_base<Renderer> base_type;

        //---------------------------------------------------------------------
        line_interpolator_aa0(renderer_type& ren, LineParameters& lp) :
            line_interpolator_aa_base<Renderer>(ren, lp),
            m_di(lp.x1, lp.y1, lp.x2, lp.y2, 
                 lp.x1 & ~line_subpixel_mask, lp.y1 & ~line_subpixel_mask)
        {
            base_type::m_li.adjust_forward();
        }

        //---------------------------------------------------------------------
        bool step_hor()
        {
            int dist;
            int dy;
            int s1 = base_type::step_hor_base(m_di);
            byte* p0 = base_type::m_covers + base_type::max_half_width + 2;
            byte* p1 = p0;

            *p1++ = (byte)base_type::m_ren.cover(s1);

            dy = 1;
            while((dist = base_type::m_dist[dy] - s1) <= base_type::m_width)
            {
                *p1++ = (byte)base_type::m_ren.cover(dist);
                ++dy;
            }

            dy = 1;
            while((dist = base_type::m_dist[dy] + s1) <= base_type::m_width)
            {
                *--p0 = (byte)base_type::m_ren.cover(dist);
                ++dy;
            }
            base_type::m_ren.BlendSolidVerticalSpan(base_type::m_x, 
                                               base_type::m_y - dy + 1, 
                                               uint(p1 - p0), 
                                               p0);
            return ++base_type::m_step < base_type::m_count;
        }

        //---------------------------------------------------------------------
        bool step_ver()
        {
            int dist;
            int dx;
            int s1 = base_type::step_ver_base(m_di);
            byte* p0 = base_type::m_covers + base_type::max_half_width + 2;
            byte* p1 = p0;

            *p1++ = (byte)base_type::m_ren.cover(s1);

            dx = 1;
            while((dist = base_type::m_dist[dx] - s1) <= base_type::m_width)
            {
                *p1++ = (byte)base_type::m_ren.cover(dist);
                ++dx;
            }

            dx = 1;
            while((dist = base_type::m_dist[dx] + s1) <= base_type::m_width)
            {
                *--p0 = (byte)base_type::m_ren.cover(dist);
                ++dx;
            }
            base_type::m_ren.BlendSolidHorizontalSpan(base_type::m_x - dx + 1, 
                                               base_type::m_y,
                                               uint(p1 - p0), 
                                               p0);
            return ++base_type::m_step < base_type::m_count;
        }

    private:
        line_interpolator_aa0(line_interpolator_aa0<Renderer>&);
        line_interpolator_aa0<Renderer>& 
            operator = (line_interpolator_aa0<Renderer>&);

        //---------------------------------------------------------------------
        distance_interpolator1 m_di; 
    };






    //====================================================line_interpolator_aa1
    template<class Renderer> class line_interpolator_aa1 :
    public line_interpolator_aa_base<Renderer>
    {
    public:
        typedef Renderer renderer_type;
        typedef line_interpolator_aa_base<Renderer> base_type;

        //---------------------------------------------------------------------
        line_interpolator_aa1(renderer_type& ren, LineParameters& lp, 
                              int sx, int sy) :
            line_interpolator_aa_base<Renderer>(ren, lp),
            m_di(lp.x1, lp.y1, lp.x2, lp.y2, sx, sy,
                 lp.x1 & ~line_subpixel_mask, lp.y1 & ~line_subpixel_mask)
        {
            int dist1_start;
            int dist2_start;

            int npix = 1;

            if(lp.vertical)
            {
                do
                {
                    --base_type::m_li;
                    base_type::m_y -= lp.inc;
                    base_type::m_x = (base_type::m_lp->x1 + base_type::m_li.y()) >> line_subpixel_shift;

                    if(lp.inc > 0) m_di.dec_y(base_type::m_x - base_type::m_old_x);
                    else           m_di.inc_y(base_type::m_x - base_type::m_old_x);

                    base_type::m_old_x = base_type::m_x;

                    dist1_start = dist2_start = m_di.dist_start(); 

                    int dx = 0;
                    if(dist1_start < 0) ++npix;
                    do
                    {
                        dist1_start += m_di.dy_start();
                        dist2_start -= m_di.dy_start();
                        if(dist1_start < 0) ++npix;
                        if(dist2_start < 0) ++npix;
                        ++dx;
                    }
                    while(base_type::m_dist[dx] <= base_type::m_width);
                    --base_type::m_step;
                    if(npix == 0) break;
                    npix = 0;
                }
                while(base_type::m_step >= -base_type::m_max_extent);
            }
            else
            {
                do
                {
                    --base_type::m_li;
                    base_type::m_x -= lp.inc;
                    base_type::m_y = (base_type::m_lp->y1 + base_type::m_li.y()) >> line_subpixel_shift;

                    if(lp.inc > 0) m_di.dec_x(base_type::m_y - base_type::m_old_y);
                    else           m_di.inc_x(base_type::m_y - base_type::m_old_y);

                    base_type::m_old_y = base_type::m_y;

                    dist1_start = dist2_start = m_di.dist_start(); 

                    int dy = 0;
                    if(dist1_start < 0) ++npix;
                    do
                    {
                        dist1_start -= m_di.dx_start();
                        dist2_start += m_di.dx_start();
                        if(dist1_start < 0) ++npix;
                        if(dist2_start < 0) ++npix;
                        ++dy;
                    }
                    while(base_type::m_dist[dy] <= base_type::m_width);
                    --base_type::m_step;
                    if(npix == 0) break;
                    npix = 0;
                }
                while(base_type::m_step >= -base_type::m_max_extent);
            }
            base_type::m_li.adjust_forward();
        }

        //---------------------------------------------------------------------
        bool step_hor()
        {
            int dist_start;
            int dist;
            int dy;
            int s1 = base_type::step_hor_base(m_di);

            dist_start = m_di.dist_start();
            byte* p0 = base_type::m_covers + base_type::max_half_width + 2;
            byte* p1 = p0;

            *p1 = 0;
            if(dist_start <= 0)
            {
                *p1 = (byte)base_type::m_ren.cover(s1);
            }
            ++p1;

            dy = 1;
            while((dist = base_type::m_dist[dy] - s1) <= base_type::m_width)
            {
                dist_start -= m_di.dx_start();
                *p1 = 0;
                if(dist_start <= 0)
                {   
                    *p1 = (byte)base_type::m_ren.cover(dist);
                }
                ++p1;
                ++dy;
            }

            dy = 1;
            dist_start = m_di.dist_start();
            while((dist = base_type::m_dist[dy] + s1) <= base_type::m_width)
            {
                dist_start += m_di.dx_start();
                *--p0 = 0;
                if(dist_start <= 0)
                {   
                    *p0 = (byte)base_type::m_ren.cover(dist);
                }
                ++dy;
            }

            base_type::m_ren.BlendSolidVerticalSpan(base_type::m_x, 
                                               base_type::m_y - dy + 1,
                                               uint(p1 - p0), 
                                               p0);
            return ++base_type::m_step < base_type::m_count;
        }

        //---------------------------------------------------------------------
        bool step_ver()
        {
            int dist_start;
            int dist;
            int dx;
            int s1 = base_type::step_ver_base(m_di);
            byte* p0 = base_type::m_covers + base_type::max_half_width + 2;
            byte* p1 = p0;

            dist_start = m_di.dist_start();

            *p1 = 0;
            if(dist_start <= 0)
            {
                *p1 = (byte)base_type::m_ren.cover(s1);
            }
            ++p1;

            dx = 1;
            while((dist = base_type::m_dist[dx] - s1) <= base_type::m_width)
            {
                dist_start += m_di.dy_start();
                *p1 = 0;
                if(dist_start <= 0)
                {   
                    *p1 = (byte)base_type::m_ren.cover(dist);
                }
                ++p1;
                ++dx;
            }

            dx = 1;
            dist_start = m_di.dist_start();
            while((dist = base_type::m_dist[dx] + s1) <= base_type::m_width)
            {
                dist_start -= m_di.dy_start();
                *--p0 = 0;
                if(dist_start <= 0)
                {   
                    *p0 = (byte)base_type::m_ren.cover(dist);
                }
                ++dx;
            }
            base_type::m_ren.BlendSolidHorizontalSpan(base_type::m_x - dx + 1, 
                                               base_type::m_y,
                                               uint(p1 - p0), 
                                               p0);
            return ++base_type::m_step < base_type::m_count;
        }

    private:
        line_interpolator_aa1(line_interpolator_aa1<Renderer>&);
        line_interpolator_aa1<Renderer>& 
            operator = (line_interpolator_aa1<Renderer>&);

        //---------------------------------------------------------------------
        distance_interpolator2 m_di; 
    };












    //====================================================line_interpolator_aa2
    template<class Renderer> class line_interpolator_aa2 :
    public line_interpolator_aa_base<Renderer>
    {
    public:
        typedef Renderer renderer_type;
        typedef line_interpolator_aa_base<Renderer> base_type;

        //---------------------------------------------------------------------
        line_interpolator_aa2(renderer_type& ren, LineParameters& lp, 
                              int ex, int ey) :
            line_interpolator_aa_base<Renderer>(ren, lp),
            m_di(lp.x1, lp.y1, lp.x2, lp.y2, ex, ey, 
                 lp.x1 & ~line_subpixel_mask, lp.y1 & ~line_subpixel_mask,
                 0)
        {
            base_type::m_li.adjust_forward();
            base_type::m_step -= base_type::m_max_extent;
        }

        //---------------------------------------------------------------------
        bool step_hor()
        {
            int dist_end;
            int dist;
            int dy;
            int s1 = base_type::step_hor_base(m_di);
            byte* p0 = base_type::m_covers + base_type::max_half_width + 2;
            byte* p1 = p0;

            dist_end = m_di.dist_end();

            int npix = 0;
            *p1 = 0;
            if(dist_end > 0)
            {
                *p1 = (byte)base_type::m_ren.cover(s1);
                ++npix;
            }
            ++p1;

            dy = 1;
            while((dist = base_type::m_dist[dy] - s1) <= base_type::m_width)
            {
                dist_end -= m_di.dx_end();
                *p1 = 0;
                if(dist_end > 0)
                {   
                    *p1 = (byte)base_type::m_ren.cover(dist);
                    ++npix;
                }
                ++p1;
                ++dy;
            }

            dy = 1;
            dist_end = m_di.dist_end();
            while((dist = base_type::m_dist[dy] + s1) <= base_type::m_width)
            {
                dist_end += m_di.dx_end();
                *--p0 = 0;
                if(dist_end > 0)
                {   
                    *p0 = (byte)base_type::m_ren.cover(dist);
                    ++npix;
                }
                ++dy;
            }
            base_type::m_ren.BlendSolidVerticalSpan(base_type::m_x,
                                               base_type::m_y - dy + 1, 
                                               uint(p1 - p0), 
                                               p0);
            return npix && ++base_type::m_step < base_type::m_count;
        }

        //---------------------------------------------------------------------
        bool step_ver()
        {
            int dist_end;
            int dist;
            int dx;
            int s1 = base_type::step_ver_base(m_di);
            byte* p0 = base_type::m_covers + base_type::max_half_width + 2;
            byte* p1 = p0;

            dist_end = m_di.dist_end();

            int npix = 0;
            *p1 = 0;
            if(dist_end > 0)
            {
                *p1 = (byte)base_type::m_ren.cover(s1);
                ++npix;
            }
            ++p1;

            dx = 1;
            while((dist = base_type::m_dist[dx] - s1) <= base_type::m_width)
            {
                dist_end += m_di.dy_end();
                *p1 = 0;
                if(dist_end > 0)
                {   
                    *p1 = (byte)base_type::m_ren.cover(dist);
                    ++npix;
                }
                ++p1;
                ++dx;
            }

            dx = 1;
            dist_end = m_di.dist_end();
            while((dist = base_type::m_dist[dx] + s1) <= base_type::m_width)
            {
                dist_end -= m_di.dy_end();
                *--p0 = 0;
                if(dist_end > 0)
                {   
                    *p0 = (byte)base_type::m_ren.cover(dist);
                    ++npix;
                }
                ++dx;
            }
            base_type::m_ren.BlendSolidHorizontalSpan(base_type::m_x - dx + 1,
                                               base_type::m_y, 
                                               uint(p1 - p0), 
                                               p0);
            return npix && ++base_type::m_step < base_type::m_count;
        }

    private:
        line_interpolator_aa2(line_interpolator_aa2<Renderer>&);
        line_interpolator_aa2<Renderer>& 
            operator = (line_interpolator_aa2<Renderer>&);

        //---------------------------------------------------------------------
        distance_interpolator2 m_di; 
    };










    //====================================================line_interpolator_aa3
    template<class Renderer> class line_interpolator_aa3 :
    public line_interpolator_aa_base<Renderer>
    {
    public:
        typedef Renderer renderer_type;
        typedef line_interpolator_aa_base<Renderer> base_type;

        //---------------------------------------------------------------------
        line_interpolator_aa3(renderer_type& ren, LineParameters& lp, 
                              int sx, int sy, int ex, int ey) :
            line_interpolator_aa_base<Renderer>(ren, lp),
            m_di(lp.x1, lp.y1, lp.x2, lp.y2, sx, sy, ex, ey, 
                 lp.x1 & ~line_subpixel_mask, lp.y1 & ~line_subpixel_mask)
        {
            int dist1_start;
            int dist2_start;
            int npix = 1;
            if(lp.vertical)
            {
                do
                {
                    --base_type::m_li;
                    base_type::m_y -= lp.inc;
                    base_type::m_x = (base_type::m_lp->x1 + base_type::m_li.y()) >> line_subpixel_shift;

                    if(lp.inc > 0) m_di.dec_y(base_type::m_x - base_type::m_old_x);
                    else           m_di.inc_y(base_type::m_x - base_type::m_old_x);

                    base_type::m_old_x = base_type::m_x;

                    dist1_start = dist2_start = m_di.dist_start(); 

                    int dx = 0;
                    if(dist1_start < 0) ++npix;
                    do
                    {
                        dist1_start += m_di.dy_start();
                        dist2_start -= m_di.dy_start();
                        if(dist1_start < 0) ++npix;
                        if(dist2_start < 0) ++npix;
                        ++dx;
                    }
                    while(base_type::m_dist[dx] <= base_type::m_width);
                    if(npix == 0) break;
                    npix = 0;
                }
                while(--base_type::m_step >= -base_type::m_max_extent);
            }
            else
            {
                do
                {
                    --base_type::m_li;
                    base_type::m_x -= lp.inc;
                    base_type::m_y = (base_type::m_lp->y1 + base_type::m_li.y()) >> line_subpixel_shift;

                    if(lp.inc > 0) m_di.dec_x(base_type::m_y - base_type::m_old_y);
                    else           m_di.inc_x(base_type::m_y - base_type::m_old_y);

                    base_type::m_old_y = base_type::m_y;

                    dist1_start = dist2_start = m_di.dist_start(); 

                    int dy = 0;
                    if(dist1_start < 0) ++npix;
                    do
                    {
                        dist1_start -= m_di.dx_start();
                        dist2_start += m_di.dx_start();
                        if(dist1_start < 0) ++npix;
                        if(dist2_start < 0) ++npix;
                        ++dy;
                    }
                    while(base_type::m_dist[dy] <= base_type::m_width);
                    if(npix == 0) break;
                    npix = 0;
                }
                while(--base_type::m_step >= -base_type::m_max_extent);
            }
            base_type::m_li.adjust_forward();
            base_type::m_step -= base_type::m_max_extent;
        }


        //---------------------------------------------------------------------
        bool step_hor()
        {
            int dist_start;
            int dist_end;
            int dist;
            int dy;
            int s1 = base_type::step_hor_base(m_di);
            byte* p0 = base_type::m_covers + base_type::max_half_width + 2;
            byte* p1 = p0;

            dist_start = m_di.dist_start();
            dist_end   = m_di.dist_end();

            int npix = 0;
            *p1 = 0;
            if(dist_end > 0)
            {
                if(dist_start <= 0)
                {
                    *p1 = (byte)base_type::m_ren.cover(s1);
                }
                ++npix;
            }
            ++p1;

            dy = 1;
            while((dist = base_type::m_dist[dy] - s1) <= base_type::m_width)
            {
                dist_start -= m_di.dx_start();
                dist_end   -= m_di.dx_end();
                *p1 = 0;
                if(dist_end > 0 && dist_start <= 0)
                {   
                    *p1 = (byte)base_type::m_ren.cover(dist);
                    ++npix;
                }
                ++p1;
                ++dy;
            }

            dy = 1;
            dist_start = m_di.dist_start();
            dist_end   = m_di.dist_end();
            while((dist = base_type::m_dist[dy] + s1) <= base_type::m_width)
            {
                dist_start += m_di.dx_start();
                dist_end   += m_di.dx_end();
                *--p0 = 0;
                if(dist_end > 0 && dist_start <= 0)
                {   
                    *p0 = (byte)base_type::m_ren.cover(dist);
                    ++npix;
                }
                ++dy;
            }
            base_type::m_ren.BlendSolidVerticalSpan(base_type::m_x,
                                               base_type::m_y - dy + 1, 
                                               uint(p1 - p0), 
                                               p0);
            return npix && ++base_type::m_step < base_type::m_count;
        }

        //---------------------------------------------------------------------
        bool step_ver()
        {
            int dist_start;
            int dist_end;
            int dist;
            int dx;
            int s1 = base_type::step_ver_base(m_di);
            byte* p0 = base_type::m_covers + base_type::max_half_width + 2;
            byte* p1 = p0;

            dist_start = m_di.dist_start();
            dist_end   = m_di.dist_end();

            int npix = 0;
            *p1 = 0;
            if(dist_end > 0)
            {
                if(dist_start <= 0)
                {
                    *p1 = (byte)base_type::m_ren.cover(s1);
                }
                ++npix;
            }
            ++p1;

            dx = 1;
            while((dist = base_type::m_dist[dx] - s1) <= base_type::m_width)
            {
                dist_start += m_di.dy_start();
                dist_end   += m_di.dy_end();
                *p1 = 0;
                if(dist_end > 0 && dist_start <= 0)
                {   
                    *p1 = (byte)base_type::m_ren.cover(dist);
                    ++npix;
                }
                ++p1;
                ++dx;
            }

            dx = 1;
            dist_start = m_di.dist_start();
            dist_end   = m_di.dist_end();
            while((dist = base_type::m_dist[dx] + s1) <= base_type::m_width)
            {
                dist_start -= m_di.dy_start();
                dist_end   -= m_di.dy_end();
                *--p0 = 0;
                if(dist_end > 0 && dist_start <= 0)
                {   
                    *p0 = (byte)base_type::m_ren.cover(dist);
                    ++npix;
                }
                ++dx;
            }
            base_type::m_ren.BlendSolidHorizontalSpan(base_type::m_x - dx + 1,
                                               base_type::m_y, 
                                               uint(p1 - p0), 
                                               p0);
            return npix && ++base_type::m_step < base_type::m_count;
        }

    private:
        line_interpolator_aa3(line_interpolator_aa3<Renderer>&);
        line_interpolator_aa3<Renderer>& 
            operator = (line_interpolator_aa3<Renderer>&);

        //---------------------------------------------------------------------
        distance_interpolator3 m_di; 
    };


    //==========================================================line_profile_aa
    //
    // See Implementation agg_line_profile_aa.cpp 
    // 
    public class line_profile_aa
    {
        const int subpixel_shift = 8;
        const int Scale = 1 << subpixel_shift;
        const int subpixel_mask  = Scale - 1;

        const int aa_shift = 8;
        const int aa_scale = 1 << aa_shift;
        const int aa_mask  = aa_scale - 1;

        pod_array<byte> m_profile;
        byte[]            m_gamma = new byte[aa_scale];
        int                   m_subpixel_width;
        double                m_min_width;
        double                m_smoother_width;

        //---------------------------------------------------------------------
        
        //---------------------------------------------------------------------
        public line_profile_aa()
        {
            m_subpixel_width=(0);
            m_min_width=(1.0);
            m_smoother_width=(1.0);

            int i;
            for(i = 0; i < aa_scale; i++) m_gamma[i] = (byte)i;
        }

        //---------------------------------------------------------------------
        public line_profile_aa(double w, IGammaFunction gamma_function)
        {
            m_subpixel_width=(0);
            m_min_width=(1.0);
            m_smoother_width=(1.0);
            Gamma(gamma_function);
            Width(w);
        }

        //---------------------------------------------------------------------
        public void min_width(double w) { m_min_width = w; }
        public void smoother_width(double w) { m_smoother_width = w; }

        //---------------------------------------------------------------------
        public void Gamma(IGammaFunction gamma_function)
        { 
            int i;
            for(i = 0; i < aa_scale; i++)
            {
                m_gamma[i] = (byte)(Basics.UnsignedRound(gamma_function.GetGamma((double)(i) / aa_mask) * aa_mask));
            }
        }

        public void Width(double w)
        {
            if(w < 0.0) w = 0.0;

            if(w < m_smoother_width) w += w;
            else                     w += m_smoother_width;

            w *= 0.5;

            w -= m_smoother_width;
            double s = m_smoother_width;
            if(w < 0.0) 
            {
                s += w;
                w = 0.0;
            }
            Set(w, s);
        }

        public uint profile_size() { return m_profile.Size(); }
        public int subpixel_width() { return m_subpixel_width; }

        //---------------------------------------------------------------------
        public double min_width() { return m_min_width; }
        public double smoother_width() { return m_smoother_width; }

        //---------------------------------------------------------------------
        public byte Value(int dist)
        {
            return m_profile.Array[dist + Scale*2];
        }

        private byte[] profile(double w)
        {
            m_subpixel_width = (int)Basics.UnsignedRound(w * Scale);
            uint Size = (uint)(m_subpixel_width + Scale * 6);
            if(Size > m_profile.Size())
            {
                m_profile.resize(Size);
            }
            return m_profile.Array;
        }

        private void Set(double center_width, double smoother_width)
        {
            double base_val = 1.0;
            if(center_width == 0.0)   center_width = 1.0 / Scale;
            if(smoother_width == 0.0) smoother_width = 1.0 / Scale;

            double Width = center_width + smoother_width;
            if(Width < m_min_width)
            {
                double k = Width / m_min_width;
                base_val *= k;
                center_width /= k;
                smoother_width /= k;
            }

            byte[] ch = profile(center_width + smoother_width);
            int chIndex = 0;

            uint subpixel_center_width = (uint)(center_width * Scale);
            uint subpixel_smoother_width = (uint)(smoother_width * Scale);

            int ch_center   = Scale*2;
            int ch_smoother = (int)subpixel_center_width;

            uint i;

            uint val = m_gamma[(uint)(base_val * aa_mask)];
            chIndex = ch_center;
            for(i = 0; i < subpixel_center_width; i++)
            {
                ch[chIndex++] = (byte)val;
            }

            for(i = 0; i < subpixel_smoother_width; i++)
            {
                ch[ch_smoother++] = 
                    m_gamma[(uint)((base_val - 
                                      base_val * 
                                      ((double)(i) / subpixel_smoother_width)) * aa_mask)];
            }

            uint n_smoother = profile_size() - 
                                  subpixel_smoother_width - 
                                  subpixel_center_width - 
                                  Scale*2;

            val = m_gamma[0];
            for(i = 0; i < n_smoother; i++)
            {
                ch[ch_smoother++] = (byte)val;
            }

            chIndex = ch_center;
            for(i = 0; i < Scale*2; i++)
            {
                ch[--chIndex] = ch[ch_center++];
            }
        }
    };

    //======================================================renderer_outline_aa
    public class renderer_outline_aa : IRenderer
    {
        private renderer_base m_ren;
        private IColorType m_color;
        line_profile_aa m_profile; 
        RectI                 m_clip_box;
        bool                   m_clipping;

        //---------------------------------------------------------------------
        public renderer_outline_aa(renderer_base ren, line_profile_aa prof)
        {
            m_ren=ren;
            m_profile=prof;
            m_clip_box = new RectI(0,0,0,0);
            m_clipping=false;
        }

        public void Attach(renderer_base ren) { m_ren = &ren; }

        //---------------------------------------------------------------------
        public void Color(IColorType c) { m_color = c; }
        public IColorType Color() { return m_color; }

        //---------------------------------------------------------------------
        public void profile(line_profile_aa prof) { m_profile = prof; }
        public line_profile_aa profile() { return m_profile; }

        //---------------------------------------------------------------------
        public int subpixel_width() { return m_profile->subpixel_width(); }

        //---------------------------------------------------------------------
        public void ResetClipping() { m_clipping = false; }
        public void ClipBox(double x1, double y1, double x2, double y2)
        {
            m_clip_box.x1 = SaturatedLineCoordinate::Convert(x1);
            m_clip_box.y1 = SaturatedLineCoordinate::Convert(y1);
            m_clip_box.x2 = SaturatedLineCoordinate::Convert(x2);
            m_clip_box.y2 = SaturatedLineCoordinate::Convert(y2);
            m_clipping = true;
        }

        //---------------------------------------------------------------------
        public int cover(int d)
        {
            return m_profile->Value(d);
        }

        //-------------------------------------------------------------------------
        public unsafe void BlendSolidHorizontalSpan(int x, int y, uint len, byte* covers)
        {
            m_ren->BlendSolidHorizontalSpan(x, y, len, m_color, covers);
        }

        //-------------------------------------------------------------------------
        public unsafe void BlendSolidVerticalSpan(int x, int y, uint len, byte* covers)
        {
            m_ren->BlendSolidVerticalSpan(x, y, len, m_color, covers);
        }

        //-------------------------------------------------------------------------
        public static bool accurate_join_only() { return false; }

        //-------------------------------------------------------------------------
        //template<class Cmp>
        public void semidot_hline(Cmp cmp,
                           int xc1, int yc1, int xc2, int yc2, 
                           int x1,  int y1,  int x2)
        {
            byte covers[line_interpolator_aa_base<renderer_outline_aa>::max_half_width * 2 + 4];
            byte* p0 = covers;
            byte* p1 = covers;
            int x = x1 << line_subpixel_shift;
            int y = y1 << line_subpixel_shift;
            int w = subpixel_width();
            distance_interpolator0 di(xc1, yc1, xc2, yc2, x, y);
            x += line_subpixel_scale/2;
            y += line_subpixel_scale/2;

            int x0 = x1;
            int dx = x - xc1;
            int dy = y - yc1;
            do
            {
                int d = int(FastSqrt(dx*dx + dy*dy));
                *p1 = 0;
                if(cmp(di.dist()) && d <= w)
                {
                    *p1 = (byte)cover(d);
                }
                ++p1;
                dx += line_subpixel_scale;
                di.inc_x();
            }
            while(++x1 <= x2);
            m_ren->BlendSolidHorizontalSpan(x0, y1, 
                                     uint(p1 - p0), 
                                     Color(), 
                                     p0);
        }

        //-------------------------------------------------------------------------
        //template<class Cmp> 
        public void semidot(Cmp cmp, int xc1, int yc1, int xc2, int yc2)
        {
            if(m_clipping && GetClippingFlags(xc1, yc1, m_clip_box)) return;

            int r = ((subpixel_width() + line_subpixel_mask) >> line_subpixel_shift);
            if(r < 1) r = 1;
            ellipse_bresenham_interpolator ei(r, r);
            int dx = 0;
            int dy = -r;
            int dy0 = dy;
            int dx0 = dx;
            int x = xc1 >> line_subpixel_shift;
            int y = yc1 >> line_subpixel_shift;

            do
            {
                dx += ei.dx();
                dy += ei.dy();

                if(dy != dy0)
                {
                    semidot_hline(cmp, xc1, yc1, xc2, yc2, x-dx0, y+dy0, x+dx0);
                    semidot_hline(cmp, xc1, yc1, xc2, yc2, x-dx0, y-dy0, x+dx0);
                }
                dx0 = dx;
                dy0 = dy;
                ++ei;
            }
            while(dy < 0);
            semidot_hline(cmp, xc1, yc1, xc2, yc2, x-dx0, y+dy0, x+dx0);
        }

        //-------------------------------------------------------------------------
        public void pie_hline(int xc, int yc, int xp1, int yp1, int xp2, int yp2, 
                       int xh1, int yh1, int xh2)
        {
            if(m_clipping && GetClippingFlags(xc, yc, m_clip_box)) return;
           
            byte covers[line_interpolator_aa_base<renderer_outline_aa>::max_half_width * 2 + 4];
            byte* p0 = covers;
            byte* p1 = covers;
            int x = xh1 << line_subpixel_shift;
            int y = yh1 << line_subpixel_shift;
            int w = subpixel_width();

            distance_interpolator00 di(xc, yc, xp1, yp1, xp2, yp2, x, y);
            x += line_subpixel_scale/2;
            y += line_subpixel_scale/2;

            int xh0 = xh1;
            int dx = x - xc;
            int dy = y - yc;
            do
            {
                int d = int(FastSqrt(dx*dx + dy*dy));
                *p1 = 0;
                if(di.dist1() <= 0 && di.dist2() > 0 && d <= w)
                {
                    *p1 = (byte)cover(d);
                }
                ++p1;
                dx += line_subpixel_scale;
                di.inc_x();
            }
            while(++xh1 <= xh2);
            m_ren->BlendSolidHorizontalSpan(xh0, yh1, 
                                     uint(p1 - p0), 
                                     Color(), 
                                     p0);
        }


        //-------------------------------------------------------------------------
        public void pie(int xc, int yc, int x1, int y1, int x2, int y2)
        {
            int r = ((subpixel_width() + line_subpixel_mask) >> line_subpixel_shift);
            if(r < 1) r = 1;
            ellipse_bresenham_interpolator ei(r, r);
            int dx = 0;
            int dy = -r;
            int dy0 = dy;
            int dx0 = dx;
            int x = xc >> line_subpixel_shift;
            int y = yc >> line_subpixel_shift;

            do
            {
                dx += ei.dx();
                dy += ei.dy();

                if(dy != dy0)
                {
                    pie_hline(xc, yc, x1, y1, x2, y2, x-dx0, y+dy0, x+dx0);
                    pie_hline(xc, yc, x1, y1, x2, y2, x-dx0, y-dy0, x+dx0);
                }
                dx0 = dx;
                dy0 = dy;
                ++ei;
            }
            while(dy < 0);
            pie_hline(xc, yc, x1, y1, x2, y2, x-dx0, y+dy0, x+dx0);
        }

        //-------------------------------------------------------------------------
        public void line0_no_clip(LineParameters lp)
        {
            if(lp.len > line_max_length)
            {
                LineParameters lp1, lp2;
                lp.Divide(lp1, lp2);
                line0_no_clip(lp1);
                line0_no_clip(lp2);
                return;
            }

            line_interpolator_aa0<renderer_outline_aa> li(*this, lp);
            if(li.count())
            {
                if(li.vertical())
                {
                    while(li.step_ver());
                }
                else
                {
                    while(li.step_hor());
                }
            }
        }

        //-------------------------------------------------------------------------
        public void line0(LineParameters lp)
        {
            if(m_clipping)
            {
                int x1 = lp.x1;
                int y1 = lp.y1;
                int x2 = lp.x2;
                int y2 = lp.y2;
                uint flags = ClipLineSegment(&x1, &y1, &x2, &y2, m_clip_box);
                if((flags & 4) == 0)
                {
                    if(flags)
                    {
                        LineParameters lp2(x1, y1, x2, y2, 
                                           UnsignedRound(CalculateDistance(x1, y1, x2, y2)));
                        line0_no_clip(lp2);
                    }
                    else
                    {
                        line0_no_clip(lp);
                    }
                }
            }
            else
            {
                line0_no_clip(lp);
            }
        }

        //-------------------------------------------------------------------------
        public void line1_no_clip(LineParameters lp, int sx, int sy)
        {
            if(lp.len > line_max_length)
            {
                LineParameters lp1, lp2;
                lp.Divide(lp1, lp2);
                line1_no_clip(lp1, (lp.x1 + sx) >> 1, (lp.y1 + sy) >> 1);
                line1_no_clip(lp2, lp1.x2 + (lp1.y2 - lp1.y1), lp1.y2 - (lp1.x2 - lp1.x1));
                return;
            }

            fix_degenerate_bisectrix_start(lp, &sx, &sy);
            line_interpolator_aa1<renderer_outline_aa> li(*this, lp, sx, sy);
            if(li.vertical())
            {
                while(li.step_ver());
            }
            else
            {
                while(li.step_hor());
            }
        }


        //-------------------------------------------------------------------------
        public void line1(LineParameters lp, int sx, int sy)
        {
            if(m_clipping)
            {
                int x1 = lp.x1;
                int y1 = lp.y1;
                int x2 = lp.x2;
                int y2 = lp.y2;
                uint flags = ClipLineSegment(&x1, &y1, &x2, &y2, m_clip_box);
                if((flags & 4) == 0)
                {
                    if(flags)
                    {
                        LineParameters lp2(x1, y1, x2, y2, 
                                           UnsignedRound(CalculateDistance(x1, y1, x2, y2)));
                        if(flags & 1)
                        {
                            sx = x1 + (y2 - y1); 
                            sy = y1 - (x2 - x1);
                        }
                        else
                        {
                            while(abs(sx - lp.x1) + abs(sy - lp.y1) > lp2.len)
                            {
                                sx = (lp.x1 + sx) >> 1;
                                sy = (lp.y1 + sy) >> 1;
                            }
                        }
                        line1_no_clip(lp2, sx, sy);
                    }
                    else
                    {
                        line1_no_clip(lp, sx, sy);
                    }
                }
            }
            else
            {
                line1_no_clip(lp, sx, sy);
            }
        }

        //-------------------------------------------------------------------------
        public void line2_no_clip(LineParameters lp, int ex, int ey)
        {
            if(lp.len > line_max_length)
            {
                LineParameters lp1, lp2;
                lp.Divide(lp1, lp2);
                line2_no_clip(lp1, lp1.x2 + (lp1.y2 - lp1.y1), lp1.y2 - (lp1.x2 - lp1.x1));
                line2_no_clip(lp2, (lp.x2 + ex) >> 1, (lp.y2 + ey) >> 1);
                return;
            }

            fix_degenerate_bisectrix_end(lp, &ex, &ey);
            line_interpolator_aa2<renderer_outline_aa> li(*this, lp, ex, ey);
            if(li.vertical())
            {
                while(li.step_ver());
            }
            else
            {
                while(li.step_hor());
            }
        }

        //-------------------------------------------------------------------------
        public void line2(LineParameters lp, int ex, int ey)
        {
            if(m_clipping)
            {
                int x1 = lp.x1;
                int y1 = lp.y1;
                int x2 = lp.x2;
                int y2 = lp.y2;
                uint flags = ClipLineSegment(&x1, &y1, &x2, &y2, m_clip_box);
                if((flags & 4) == 0)
                {
                    if(flags)
                    {
                        LineParameters lp2(x1, y1, x2, y2, 
                                           UnsignedRound(CalculateDistance(x1, y1, x2, y2)));
                        if(flags & 2)
                        {
                            ex = x2 + (y2 - y1); 
                            ey = y2 - (x2 - x1);
                        }
                        else
                        {
                            while(abs(ex - lp.x2) + abs(ey - lp.y2) > lp2.len)
                            {
                                ex = (lp.x2 + ex) >> 1;
                                ey = (lp.y2 + ey) >> 1;
                            }
                        }
                        line2_no_clip(lp2, ex, ey);
                    }
                    else
                    {
                        line2_no_clip(lp, ex, ey);
                    }
                }
            }
            else
            {
                line2_no_clip(lp, ex, ey);
            }
        }

        //-------------------------------------------------------------------------
        public void line3_no_clip(LineParameters lp, 
                           int sx, int sy, int ex, int ey)
        {
            if(lp.len > line_max_length)
            {
                LineParameters lp1, lp2;
                lp.Divide(lp1, lp2);
                int mx = lp1.x2 + (lp1.y2 - lp1.y1);
                int my = lp1.y2 - (lp1.x2 - lp1.x1);
                line3_no_clip(lp1, (lp.x1 + sx) >> 1, (lp.y1 + sy) >> 1, mx, my);
                line3_no_clip(lp2, mx, my, (lp.x2 + ex) >> 1, (lp.y2 + ey) >> 1);
                return;
            }

            fix_degenerate_bisectrix_start(lp, &sx, &sy);
            fix_degenerate_bisectrix_end(lp, &ex, &ey);
            line_interpolator_aa3<renderer_outline_aa> li(*this, lp, sx, sy, ex, ey);
            if(li.vertical())
            {
                while(li.step_ver());
            }
            else
            {
                while(li.step_hor());
            }
        }

        //-------------------------------------------------------------------------
        public void line3(LineParameters lp, 
                   int sx, int sy, int ex, int ey)
        {
            if(m_clipping)
            {
                int x1 = lp.x1;
                int y1 = lp.y1;
                int x2 = lp.x2;
                int y2 = lp.y2;
                uint flags = ClipLineSegment(&x1, &y1, &x2, &y2, m_clip_box);
                if((flags & 4) == 0)
                {
                    if(flags)
                    {
                        LineParameters lp2(x1, y1, x2, y2, 
                                           UnsignedRound(CalculateDistance(x1, y1, x2, y2)));
                        if(flags & 1)
                        {
                            sx = x1 + (y2 - y1); 
                            sy = y1 - (x2 - x1);
                        }
                        else
                        {
                            while(abs(sx - lp.x1) + abs(sy - lp.y1) > lp2.len)
                            {
                                sx = (lp.x1 + sx) >> 1;
                                sy = (lp.y1 + sy) >> 1;
                            }
                        }
                        if(flags & 2)
                        {
                            ex = x2 + (y2 - y1); 
                            ey = y2 - (x2 - x1);
                        }
                        else
                        {
                            while(abs(ex - lp.x2) + abs(ey - lp.y2) > lp2.len)
                            {
                                ex = (lp.x2 + ex) >> 1;
                                ey = (lp.y2 + ey) >> 1;
                            }
                        }
                        line3_no_clip(lp2, sx, sy, ex, ey);
                    }
                    else
                    {
                        line3_no_clip(lp, sx, sy, ex, ey);
                    }
                }
            }
            else
            {
                line3_no_clip(lp, sx, sy, ex, ey);
            }
        }
    };
    */
}
