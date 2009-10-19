/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

#if false
using System;
using AGG.PixelFormat;

namespace AGG
{
    /*
    //========================================================line_image_scale
    public class line_image_scale
    {
        IPixelFormat m_source;
        double        m_height;
        double        m_scale;

        public line_image_scale(IPixelFormat src, double height)
        {
            m_source = (src);
            m_height = (height);
            m_scale = (src.height() / height);
        }

        public double width()  { return m_source.width(); }
        public double height() { return m_height; }

        public RGBA_Bytes pixel(int x, int y) 
        { 
            double src_y = (y + 0.5) * m_scale - 0.5;
            int h  = m_source.height() - 1;
            int y1 = ufloor(src_y);
            int y2 = y1 + 1;
            RGBA_Bytes pix1 = (y1 < 0) ? new no_color() : m_source.pixel(x, y1);
            RGBA_Bytes pix2 = (y2 > h) ? no_color() : m_source.pixel(x, y2);
            return pix1.gradient(pix2, src_y - y1);
        }
    };

     */


    //======================================================line_image_pattern
    public class line_image_pattern
    {
        IPatternFilter m_filter;
        int            m_dilation;
        int                  m_dilation_hr;
        RasterBuffer m_buf;
        IntPtr m_data = (IntPtr)0;
        int m_DataSizeInBytes = 0;
        int                 m_width;
        int                 m_height;
        int                  m_width_hr;
        int                  m_half_height_hr;
        int                  m_offset_y_hr;

        //--------------------------------------------------------------------
        public line_image_pattern(IPatternFilter filter)
        {
            m_filter=filter;
            m_dilation=(filter.dilation() + 1);
            m_dilation_hr=(m_dilation << LineAABasics.line_subpixel_shift);
            m_width=(0);
            m_height=(0);
            m_width_hr=(0);
            m_half_height_hr=(0);
            m_offset_y_hr=(0);
        }

        ~line_image_pattern()
        {
            if(m_DataSizeInBytes > 0)
            {
                System.Runtime.InteropServices.Marshal.FreeHGlobal(m_data);
            }
        }

        // Create
        //--------------------------------------------------------------------
        public line_image_pattern(IPatternFilter filter, RasterBuffer src)
        {
            m_filter=(filter);
            m_dilation=(filter.dilation() + 1);
            m_dilation_hr=(m_dilation << LineAABasics.line_subpixel_shift);
            m_width=0;
            m_height=0;
            m_width_hr=0;
            m_half_height_hr=0;
            m_offset_y_hr=(0);
            m_buf = src;

            create(src);
        }

        // Create
        //--------------------------------------------------------------------
        public void create(RasterBuffer src)
        {
            m_height = (int)agg_basics.uceil(src.Height());
            m_width = (int)agg_basics.uceil(src.Width());
            m_width_hr = (int)agg_basics.uround(src.Width() * LineAABasics.line_subpixel_scale);
            m_half_height_hr = (int)agg_basics.uround(src.Height() * LineAABasics.line_subpixel_scale / 2);
            m_offset_y_hr = m_dilation_hr + m_half_height_hr - LineAABasics.line_subpixel_scale / 2;
            m_half_height_hr += LineAABasics.line_subpixel_scale / 2;

            int NewSizeInBytes = (m_width + m_dilation * 2) * (m_height + m_dilation * 2);
            if (m_DataSizeInBytes < NewSizeInBytes)
            {
                if (m_data != null)
                {
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(m_data);
                }
                m_DataSizeInBytes = NewSizeInBytes;
                m_data = System.Runtime.InteropServices.Marshal.AllocHGlobal(m_DataSizeInBytes);
            }

            unsafe
            {
                m_buf.attach((byte*)m_data,
                    (uint)(m_width + m_dilation * 2),
                    (uint)(m_height + m_dilation * 2),
                    (int)(m_width + m_dilation * 2),
                    32);

                int x, y;
                RGBA_Bytes* d1;
                RGBA_Bytes* d2;
                for (y = 0; y < m_height; y++)
                {
                    d1 = (RGBA_Bytes*)m_buf.GetPixelPointer(y + m_dilation) + m_dilation;
                    for (x = 0; x < m_width; x++)
                    {
                        *d1++ = *(RGBA_Bytes*)src.GetPixelPointer(x, y);
                    }
                }

                RGBA_Bytes* s1;
                RGBA_Bytes* s2;
                RGBA_Bytes noColor = (RGBA_Bytes)RGBA_Bytes.no_color();
                for (y = 0; y < m_dilation; y++)
                {
                    //s1 = m_buf.GetPixelPointer(m_height + m_dilation - 1) + m_dilation;
                    //s2 = m_buf.GetPixelPointer(m_dilation) + m_dilation;
                    d1 = (RGBA_Bytes*)m_buf.GetPixelPointer(m_dilation + m_height + y) + m_dilation;
                    d2 = (RGBA_Bytes*)m_buf.GetPixelPointer(m_dilation - y - 1) + m_dilation;
                    for (x = 0; x < m_width; x++)
                    {
                        //*d1++ = RGBA_Bytes(*s1++, 0);
                        //*d2++ = RGBA_Bytes(*s2++, 0);
                        *d1++ = noColor;
                        *d2++ = noColor;
                    }
                }

                int h = m_height + m_dilation * 2;
                for (y = 0; y < h; y++)
                {
                    s1 = (RGBA_Bytes*)m_buf.GetPixelPointer(y) + m_dilation;
                    s2 = (RGBA_Bytes*)m_buf.GetPixelPointer(y) + m_dilation + m_width;
                    d1 = (RGBA_Bytes*)m_buf.GetPixelPointer(y) + m_dilation + m_width;
                    d2 = (RGBA_Bytes*)m_buf.GetPixelPointer(y) + m_dilation;

                    for (x = 0; x < m_dilation; x++)
                    {
                        *d1++ = *s1++;
                        *--d2 = *--s2;
                    }
                }
            }
        }

        //--------------------------------------------------------------------
        public int pattern_width() { return m_width_hr; }
        public int line_width()    { return m_half_height_hr; }
        public double width()      { return m_height; }

        //--------------------------------------------------------------------
        public unsafe void pixel(RGBA_Bytes* p, int x, int y)
        {
            m_filter.pixel_high_res(m_buf, 
                                     p, 
                                     x % m_width_hr + m_dilation_hr,
                                     y + m_offset_y_hr);
        }

        //--------------------------------------------------------------------
        public IPatternFilter filter() { return m_filter; }
    };

    /*
    
    //=================================================line_image_pattern_pow2
    public class line_image_pattern_pow2 : 
        line_image_pattern<IPatternFilter>
    {
        uint m_mask;
	
        //--------------------------------------------------------------------
        public line_image_pattern_pow2(IPatternFilter filter) :
            line_image_pattern<IPatternFilter>(filter), m_mask(line_subpixel_mask) {}

        //--------------------------------------------------------------------
        public line_image_pattern_pow2(IPatternFilter filter, RasterBuffer src) :
            line_image_pattern<IPatternFilter>(filter), m_mask(line_subpixel_mask)
        {
            create(src);
        }
            
        //--------------------------------------------------------------------
        public void create(RasterBuffer src)
        {
            line_image_pattern<IPatternFilter>::create(src);
            m_mask = 1;
            while(m_mask < base_type::m_width) 
            {
                m_mask <<= 1;
                m_mask |= 1;
            }
            m_mask <<= line_subpixel_shift - 1;
            m_mask |=  line_subpixel_mask;
            base_type::m_width_hr = m_mask + 1;
        }

        //--------------------------------------------------------------------
        public void pixel(RGBA_Bytes* p, int x, int y)
        {
            base_type::m_filter->pixel_high_res(
                    base_type::m_buf.rows(), 
                    p,
                    (x & m_mask) + base_type::m_dilation_hr,
                    y + base_type::m_offset_y_hr);
        }
    };
     */    
    
    //===================================================distance_interpolator4
    public class distance_interpolator4
    {
        int m_dx;
        int m_dy;
        int m_dx_start;
        int m_dy_start;
        int m_dx_pict;
        int m_dy_pict;
        int m_dx_end;
        int m_dy_end;

        int m_dist;
        int m_dist_start;
        int m_dist_pict;
        int m_dist_end;
        int m_len;

        //---------------------------------------------------------------------
        public distance_interpolator4() {}
        public distance_interpolator4(int x1,  int y1, int x2, int y2,
                               int sx,  int sy, int ex, int ey, 
                               int len, double scale, int x, int y)
        {
            m_dx=(x2 - x1);
            m_dy=(y2 - y1);
            m_dx_start = (LineAABasics.line_mr(sx) - LineAABasics.line_mr(x1));
            m_dy_start = (LineAABasics.line_mr(sy) - LineAABasics.line_mr(y1));
            m_dx_end = (LineAABasics.line_mr(ex) - LineAABasics.line_mr(x2));
            m_dy_end = (LineAABasics.line_mr(ey) - LineAABasics.line_mr(y2));

            m_dist = (agg_basics.iround((double)(x + LineAABasics.line_subpixel_scale / 2 - x2) * (double)(m_dy) -
                          (double)(y + LineAABasics.line_subpixel_scale / 2 - y2) * (double)(m_dx)));

            m_dist_start = ((LineAABasics.line_mr(x + LineAABasics.line_subpixel_scale / 2) - LineAABasics.line_mr(sx)) * m_dy_start -
                         (LineAABasics.line_mr(y + LineAABasics.line_subpixel_scale / 2) - LineAABasics.line_mr(sy)) * m_dx_start);

            m_dist_end = ((LineAABasics.line_mr(x + LineAABasics.line_subpixel_scale / 2) - LineAABasics.line_mr(ex)) * m_dy_end -
                       (LineAABasics.line_mr(y + LineAABasics.line_subpixel_scale / 2) - LineAABasics.line_mr(ey)) * m_dx_end);
            m_len=(int)(agg_basics.uround(len / scale));

            double d = len * scale;
            int dx = agg_basics.iround(((x2 - x1) << LineAABasics.line_subpixel_shift) / d);
            int dy = agg_basics.iround(((y2 - y1) << LineAABasics.line_subpixel_shift) / d);
            m_dx_pict   = -dy;
            m_dy_pict   =  dx;
            m_dist_pict = ((x + LineAABasics.line_subpixel_scale / 2 - (x1 - dy)) * m_dy_pict -
                            (y + LineAABasics.line_subpixel_scale / 2 - (y1 + dx)) * m_dx_pict) >>
                           LineAABasics.line_subpixel_shift;

            m_dx <<= LineAABasics.line_subpixel_shift;
            m_dy <<= LineAABasics.line_subpixel_shift;
            m_dx_start <<= LineAABasics.line_mr_subpixel_shift;
            m_dy_start <<= LineAABasics.line_mr_subpixel_shift;
            m_dx_end <<= LineAABasics.line_mr_subpixel_shift;
            m_dy_end <<= LineAABasics.line_mr_subpixel_shift;
        }

        //---------------------------------------------------------------------
        public void inc_x() 
        { 
            m_dist += m_dy; 
            m_dist_start += m_dy_start; 
            m_dist_pict += m_dy_pict; 
            m_dist_end += m_dy_end; 
        }

        //---------------------------------------------------------------------
        public void dec_x() 
        { 
            m_dist -= m_dy; 
            m_dist_start -= m_dy_start; 
            m_dist_pict -= m_dy_pict; 
            m_dist_end -= m_dy_end; 
        }

        //---------------------------------------------------------------------
        public void inc_y() 
        { 
            m_dist -= m_dx; 
            m_dist_start -= m_dx_start; 
            m_dist_pict -= m_dx_pict; 
            m_dist_end -= m_dx_end; 
        }

        //---------------------------------------------------------------------
        public void dec_y() 
        { 
            m_dist += m_dx; 
            m_dist_start += m_dx_start; 
            m_dist_pict += m_dx_pict; 
            m_dist_end += m_dx_end; 
        }

        //---------------------------------------------------------------------
        public void inc_x(int dy)
        {
            m_dist       += m_dy; 
            m_dist_start += m_dy_start; 
            m_dist_pict  += m_dy_pict; 
            m_dist_end   += m_dy_end;
            if(dy > 0)
            {
                m_dist       -= m_dx; 
                m_dist_start -= m_dx_start; 
                m_dist_pict  -= m_dx_pict; 
                m_dist_end   -= m_dx_end;
            }
            if(dy < 0)
            {
                m_dist       += m_dx; 
                m_dist_start += m_dx_start; 
                m_dist_pict  += m_dx_pict; 
                m_dist_end   += m_dx_end;
            }
        }

        //---------------------------------------------------------------------
        public void dec_x(int dy)
        {
            m_dist       -= m_dy; 
            m_dist_start -= m_dy_start; 
            m_dist_pict  -= m_dy_pict; 
            m_dist_end   -= m_dy_end;
            if(dy > 0)
            {
                m_dist       -= m_dx; 
                m_dist_start -= m_dx_start; 
                m_dist_pict  -= m_dx_pict; 
                m_dist_end   -= m_dx_end;
            }
            if(dy < 0)
            {
                m_dist       += m_dx; 
                m_dist_start += m_dx_start; 
                m_dist_pict  += m_dx_pict; 
                m_dist_end   += m_dx_end;
            }
        }

        //---------------------------------------------------------------------
        public void inc_y(int dx)
        {
            m_dist       -= m_dx; 
            m_dist_start -= m_dx_start; 
            m_dist_pict  -= m_dx_pict; 
            m_dist_end   -= m_dx_end;
            if(dx > 0)
            {
                m_dist       += m_dy; 
                m_dist_start += m_dy_start; 
                m_dist_pict  += m_dy_pict; 
                m_dist_end   += m_dy_end;
            }
            if(dx < 0)
            {
                m_dist       -= m_dy; 
                m_dist_start -= m_dy_start; 
                m_dist_pict  -= m_dy_pict; 
                m_dist_end   -= m_dy_end;
            }
        }

        //---------------------------------------------------------------------
        public void dec_y(int dx)
        {
            m_dist       += m_dx; 
            m_dist_start += m_dx_start; 
            m_dist_pict  += m_dx_pict; 
            m_dist_end   += m_dx_end;
            if(dx > 0)
            {
                m_dist       += m_dy; 
                m_dist_start += m_dy_start; 
                m_dist_pict  += m_dy_pict; 
                m_dist_end   += m_dy_end;
            }
            if(dx < 0)
            {
                m_dist       -= m_dy; 
                m_dist_start -= m_dy_start; 
                m_dist_pict  -= m_dy_pict; 
                m_dist_end   -= m_dy_end;
            }
        }

        //---------------------------------------------------------------------
        public int dist()       { return m_dist;       }
        public int dist_start() { return m_dist_start; }
        public int dist_pict()  { return m_dist_pict;  }
        public int dist_end()   { return m_dist_end;   }

        //---------------------------------------------------------------------
        public int dx()       { return m_dx;       }
        public int dy()       { return m_dy;       }
        public int dx_start() { return m_dx_start; }
        public int dy_start() { return m_dy_start; }
        public int dx_pict()  { return m_dx_pict;  }
        public int dy_pict()  { return m_dy_pict;  }
        public int dx_end()   { return m_dx_end;   }
        public int dy_end()   { return m_dy_end;   }
        public int len()      { return m_len;      }
    };


    //==================================================line_interpolator_image
    public class line_interpolator_image
    {
        line_parameters m_lp;
        dda2_line_interpolator m_li;
        distance_interpolator4 m_di; 
        IPixelFormat         m_ren;
        int m_plen;
        int m_x;
        int m_y;
        int m_old_x;
        int m_old_y;
        int m_count;
        int m_width;
        int m_max_extent;
        int m_start;
        int m_step;
        int[] m_dist_pos = new int[max_half_width + 1];
        RGBA_Bytes[] m_colors = new RGBA_Bytes[max_half_width * 2 + 4];

        //---------------------------------------------------------------------
        public const int max_half_width = 64;

        //---------------------------------------------------------------------
        public line_interpolator_image(IPixelFormat ren, line_parameters lp,
                                int sx, int sy, int ex, int ey, 
                                int pattern_start,
                                double scale_x)
        {
            m_lp=(lp);
            m_li = new dda2_line_interpolator(lp.vertical ? line_dbl_hr(lp.x2 - lp.x1) :
                               line_dbl_hr(lp.y2 - lp.y1),
                 lp.vertical ? abs(lp.y2 - lp.y1) : 
                               abs(lp.x2 - lp.x1) + 1);
            m_di = new distance_interpolator4(lp.x1, lp.y1, lp.x2, lp.y2, sx, sy, ex, ey, lp.len, scale_x,
                 lp.x1 & ~line_subpixel_mask, lp.y1 & ~line_subpixel_mask);
            m_ren=(ren);
            m_x=(lp.x1 >> line_subpixel_shift);
            m_y=(lp.y1 >> line_subpixel_shift);
            m_old_x=(m_x);
            m_old_y=(m_y);
            m_count=((lp.vertical ? abs((lp.y2 >> line_subpixel_shift) - m_y) :
                                   abs((lp.x2 >> line_subpixel_shift) - m_x)));
            m_width=(ren.subpixel_width());
            //m_max_extent(m_width >> (line_subpixel_shift - 2));
            m_max_extent=((m_width + LineAABasics.line_subpixel_scale) >> line_subpixel_shift);
            m_start=(pattern_start + (m_max_extent + 2) * ren.pattern_width());
            m_step=(0);

            dda2_line_interpolator li = new dda2_line_interpolator(0, lp.vertical ? 
                                              (lp.dy << LineAABasics.line_subpixel_shift) :
                                              (lp.dx << LineAABasics.line_subpixel_shift),
                                           lp.len);

            uint i;
            int stop = m_width + LineAABasics.line_subpixel_scale * 2;
            for(i = 0; i < max_half_width; ++i)
            {
                m_dist_pos[i] = li.y();
                if(m_dist_pos[i] >= stop) break;
                ++li;
            }
            m_dist_pos[i] = 0x7FFF0000;

            int dist1_start;
            int dist2_start;
            int npix = 1;

            if(lp.vertical)
            {
                do
                {
                    --m_li;
                    m_y -= lp.inc;
                    m_x = (m_lp.x1 + m_li.y()) >> line_subpixel_shift;

                    if(lp.inc > 0) m_di.dec_y(m_x - m_old_x);
                    else           m_di.inc_y(m_x - m_old_x);

                    m_old_x = m_x;

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
                    while(m_dist_pos[dx] <= m_width);
                    if(npix == 0) break;

                    npix = 0;
                }
                while(--m_step >= -m_max_extent);
            }
            else
            {
                do
                {
                    --m_li;

                    m_x -= lp.inc;
                    m_y = (m_lp.y1 + m_li.y()) >> line_subpixel_shift;

                    if(lp.inc > 0) m_di.dec_x(m_y - m_old_y);
                    else           m_di.inc_x(m_y - m_old_y);

                    m_old_y = m_y;

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
                    while(m_dist_pos[dy] <= m_width);
                    if(npix == 0) break;

                    npix = 0;
                }
                while(--m_step >= -m_max_extent);
            }
            m_li.adjust_forward();
            m_step -= m_max_extent;
        }

        //---------------------------------------------------------------------
        public bool step_hor()
        {
            ++m_li;
            m_x += m_lp.inc;
            m_y = (m_lp.y1 + m_li.y()) >> line_subpixel_shift;

            if(m_lp.inc > 0) m_di.inc_x(m_y - m_old_y);
            else             m_di.dec_x(m_y - m_old_y);

            m_old_y = m_y;

            int s1 = m_di.dist() / m_lp.len;
            int s2 = -s1;

            if(m_lp.inc < 0) s1 = -s1;

            int dist_start;
            int dist_pict;
            int dist_end;
            int dy;
            int dist;

            dist_start = m_di.dist_start();
            dist_pict  = m_di.dist_pict() + m_start;
            dist_end   = m_di.dist_end();
            RGBA_Bytes* p0 = m_colors + max_half_width + 2;
            RGBA_Bytes* p1 = p0;

            int npix = 0;
            p1->clear();
            if(dist_end > 0)
            {
                if(dist_start <= 0)
                {
                    m_ren.pixel(p1, dist_pict, s2);
                }
                ++npix;
            }
            ++p1;

            dy = 1;
            while((dist = m_dist_pos[dy]) - s1 <= m_width)
            {
                dist_start -= m_di.dx_start();
                dist_pict  -= m_di.dx_pict();
                dist_end   -= m_di.dx_end();
                p1->clear();
                if(dist_end > 0 && dist_start <= 0)
                {   
                    if(m_lp.inc > 0) dist = -dist;
                    m_ren.pixel(p1, dist_pict, s2 - dist);
                    ++npix;
                }
                ++p1;
                ++dy;
            }

            dy = 1;
            dist_start = m_di.dist_start();
            dist_pict  = m_di.dist_pict() + m_start;
            dist_end   = m_di.dist_end();
            while((dist = m_dist_pos[dy]) + s1 <= m_width)
            {
                dist_start += m_di.dx_start();
                dist_pict  += m_di.dx_pict();
                dist_end   += m_di.dx_end();
                --p0;
                p0->clear();
                if(dist_end > 0 && dist_start <= 0)
                {   
                    if(m_lp.inc > 0) dist = -dist;
                    m_ren.pixel(p0, dist_pict, s2 + dist);
                    ++npix;
                }
                ++dy;
            }
            m_ren.blend_color_vspan(m_x, 
                                    m_y - dy + 1, 
                                    (uint)(p1 - p0), 
                                    p0); 
            return npix && ++m_step < m_count;
        }

        //---------------------------------------------------------------------
        public bool step_ver()
        {
            ++m_li;
            m_y += m_lp.inc;
            m_x = (m_lp.x1 + m_li.y()) >> line_subpixel_shift;

            if(m_lp.inc > 0) m_di.inc_y(m_x - m_old_x);
            else             m_di.dec_y(m_x - m_old_x);

            m_old_x = m_x;

            int s1 = m_di.dist() / m_lp.len;
            int s2 = -s1;

            if(m_lp.inc > 0) s1 = -s1;

            int dist_start;
            int dist_pict;
            int dist_end;
            int dist;
            int dx;

            dist_start = m_di.dist_start();
            dist_pict  = m_di.dist_pict() + m_start;
            dist_end   = m_di.dist_end();
            RGBA_Bytes* p0 = m_colors + max_half_width + 2;
            RGBA_Bytes* p1 = p0;

            int npix = 0;
            p1->clear();
            if(dist_end > 0)
            {
                if(dist_start <= 0)
                {
                    m_ren.pixel(p1, dist_pict, s2);
                }
                ++npix;
            }
            ++p1;

            dx = 1;
            while((dist = m_dist_pos[dx]) - s1 <= m_width)
            {
                dist_start += m_di.dy_start();
                dist_pict  += m_di.dy_pict();
                dist_end   += m_di.dy_end();
                p1->clear();
                if(dist_end > 0 && dist_start <= 0)
                {   
                    if(m_lp.inc > 0) dist = -dist;
                    m_ren.pixel(p1, dist_pict, s2 + dist);
                    ++npix;
                }
                ++p1;
                ++dx;
            }

            dx = 1;
            dist_start = m_di.dist_start();
            dist_pict  = m_di.dist_pict() + m_start;
            dist_end   = m_di.dist_end();
            while((dist = m_dist_pos[dx]) + s1 <= m_width)
            {
                dist_start -= m_di.dy_start();
                dist_pict  -= m_di.dy_pict();
                dist_end   -= m_di.dy_end();
                --p0;
                p0->clear();
                if(dist_end > 0 && dist_start <= 0)
                {   
                    if(m_lp.inc > 0) dist = -dist;
                    m_ren.pixel(p0, dist_pict, s2 - dist);
                    ++npix;
                }
                ++dx;
            }
            m_ren.blend_color_hspan(m_x - dx + 1, 
                                    m_y, 
                                    (uint)(p1 - p0), 
                                    p0);
            return npix && ++m_step < m_count;
        }


        //---------------------------------------------------------------------
        public int  pattern_end() { return m_start + m_di.len(); }

        //---------------------------------------------------------------------
        public bool vertical() { return m_lp.vertical; }
        public int  width() { return m_width; }
        public int  count() { return m_count; }
    };

    //===================================================renderer_outline_image
    //template<class BaseRenderer, class ImagePattern> 
    public class renderer_outline_image
    {
        IPixelFormat m_ren;
        line_image_pattern m_pattern;
        int                 m_start;
        double              m_scale_x;
        rect_i              m_clip_box;
        bool                m_clipping;

        //---------------------------------------------------------------------
        //typedef renderer_outline_image<BaseRenderer, ImagePattern> self_type;

        //---------------------------------------------------------------------
        public renderer_outline_image(IPixelFormat ren, line_image_pattern patt)
        {
            m_ren=ren;
            m_pattern=patt;
            m_start=(0);
            m_scale_x=(1.0);
            m_clip_box=new rect_i(0,0,0,0);
            m_clipping=(false);
        }

        public void attach(IPixelFormat ren) { m_ren = ren; }

        //---------------------------------------------------------------------
        public void pattern(line_image_pattern p) { m_pattern = p; }
        public line_image_pattern pattern() { return m_pattern; }

        //---------------------------------------------------------------------
        public void reset_clipping() { m_clipping = false; }

        public void clip_box(double x1, double y1, double x2, double y2)
        {
            m_clip_box.x1 = line_coord_sat.conv(x1);
            m_clip_box.y1 = line_coord_sat.conv(y1);
            m_clip_box.x2 = line_coord_sat.conv(x2);
            m_clip_box.y2 = line_coord_sat.conv(y2);
            m_clipping = true;
        }

        //---------------------------------------------------------------------
        public void   scale_x(double s) { m_scale_x = s; }
        public double scale_x()   { return m_scale_x; }

        //---------------------------------------------------------------------
        public void   start_x(double s) { m_start = agg_basics.iround(s * LineAABasics.line_subpixel_scale); }
        public double start_x() { return (double)(m_start) / LineAABasics.line_subpixel_scale; }

        //---------------------------------------------------------------------
        public int subpixel_width() { return m_pattern.line_width(); }
        public int pattern_width() { return m_pattern.pattern_width(); }
        public double width() { return (double)(subpixel_width()) / LineAABasics.line_subpixel_scale; }

        //-------------------------------------------------------------------------
        public unsafe void pixel(RGBA_Bytes* p, int x, int y)
        {
            m_pattern.pixel(p, x, y);
        }

        //-------------------------------------------------------------------------
        public unsafe void blend_color_hspan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            m_ren.blend_color_hspan(x, y, len, colors, null, 0);
        }

        //-------------------------------------------------------------------------
        public unsafe void blend_color_vspan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            m_ren.blend_color_vspan(x, y, len, colors, null, 0);
        }

        //-------------------------------------------------------------------------
        public static bool accurate_join_only() { return true; }

        /*
        //-------------------------------------------------------------------------
        public void semidot(Cmp, int, int, int, int)
        {
        }

        //-------------------------------------------------------------------------
        public void pie(int, int, int, int, int, int)
        {
        }

        //-------------------------------------------------------------------------
        public void line0(line_parameters)
        {
        }

        //-------------------------------------------------------------------------
        public void line1(line_parameters, int, int)
        {
        }

        //-------------------------------------------------------------------------
        public void line2(line_parameters, int, int)
        {
        }
         */

        //-------------------------------------------------------------------------
        public void line3_no_clip(line_parameters lp, 
                           int sx, int sy, int ex, int ey)
        {
            if(lp.len > LineAABasics.line_max_length)
            {
                line_parameters lp1, lp2;
                lp.divide(lp1, lp2);
                int mx = lp1.x2 + (lp1.y2 - lp1.y1);
                int my = lp1.y2 - (lp1.x2 - lp1.x1);
                line3_no_clip(lp1, (lp.x1 + sx) >> 1, (lp.y1 + sy) >> 1, mx, my);
                line3_no_clip(lp2, mx, my, (lp.x2 + ex) >> 1, (lp.y2 + ey) >> 1);
                return;
            }
            
            LineAABasics.fix_degenerate_bisectrix_start(lp, ref sx, ref sy);
            LineAABasics.fix_degenerate_bisectrix_end(lp, ref ex, ref ey);
            line_interpolator_image li = new line_interpolator_image(*this, lp, 
                                                  sx, sy, 
                                                  ex, ey, 
                                                  m_start, m_scale_x);
            if(li.vertical())
            {
                while(li.step_ver());
            }
            else
            {
                while(li.step_hor());
            }
            m_start += uround(lp.len / m_scale_x);
        }

        //-------------------------------------------------------------------------
        public void line3(line_parameters lp, 
                   int sx, int sy, int ex, int ey)
        {
            if(m_clipping)
            {
                int x1 = lp.x1;
                int y1 = lp.y1;
                int x2 = lp.x2;
                int y2 = lp.y2;
                uint flags = clip_line_segment(&x1, &y1, &x2, &y2, m_clip_box);
                int start = m_start;
                if((flags & 4) == 0)
                {
                    if(flags)
                    {
                        line_parameters lp2(x1, y1, x2, y2, 
                                           uround(calc_distance(x1, y1, x2, y2)));
                        if(flags & 1)
                        {
                            m_start += uround(calc_distance(lp.x1, lp.y1, x1, y1) / m_scale_x);
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
                m_start = start + uround(lp.len / m_scale_x);
            }
            else
            {
                line3_no_clip(lp, sx, sy, ex, ey);
            }
        }
    };
}
#endif