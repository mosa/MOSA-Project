/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */
using System;
using image_subpixel_scale_e = Pictor.ImageFilterLookUpTable.EImageSubpixelScale;
using image_filter_scale_e = Pictor.ImageFilterLookUpTable.EImageFilterScale;

namespace Pictor
{
    //===============================================span_image_filter_rgb_nn
    public class span_image_filter_rgb_nn : SpanImageFilter
    {
        private int OrderR;
        private int OrderG;
        private int OrderB;
        private int OrderA;

        const int base_shift = 8;
        const uint base_scale = (uint)(1 << base_shift);
        const uint base_mask = base_scale - 1;

        //--------------------------------------------------------------------
        public span_image_filter_rgb_nn(IRasterBufferAccessor src, ISpanInterpolator inter)
            : base(src, inter, null) 
        {
            OrderR = src.PixelFormat.Blender.OrderR;
            OrderG = src.PixelFormat.Blender.OrderG;
            OrderB = src.PixelFormat.Blender.OrderB;
            OrderA = src.PixelFormat.Blender.OrderA;
        }

        //--------------------------------------------------------------------
        public unsafe override void Generate(RGBA_Bytes* span, int x, int y, uint len)
        {
            RasterBuffer SourceRenderingBuffer = base.source().PixelFormat.RenderingBuffer;
            ISpanInterpolator spanInterpolator = base.interpolator();
            spanInterpolator.Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), len);
            do
            {
                int x_hr;
                int y_hr;
                spanInterpolator.Coordinates(out x_hr, out y_hr);
                int x_lr = x_hr >> (int)image_subpixel_scale_e.Shift;
                int y_lr = y_hr >> (int)image_subpixel_scale_e.Shift;
                byte* fg_ptr = SourceRenderingBuffer.GetPixelPointer(y_lr) + (x_lr * 3);
                //byte* fg_ptr = spanInterpolator.Span(x_lr, y_lr, 1);
                (*span).m_R = fg_ptr[OrderR];
                (*span).m_G = fg_ptr[OrderG];
                (*span).m_B = fg_ptr[OrderB];
                (*span).m_A = 255;
                ++span;
                spanInterpolator.Next();
            } while(--len != 0);
        }
    };

    //==========================================span_image_filter_rgb_bilinear
    public class span_image_filter_rgb_bilinear : SpanImageFilter
    {
        private int OrderR;
        private int OrderG;
        private int OrderB;
        private int OrderA;

        const int base_shift = 8;
        const uint base_scale = (uint)(1 << base_shift);
        const uint base_mask = base_scale - 1;

        //--------------------------------------------------------------------
        public span_image_filter_rgb_bilinear(IRasterBufferAccessor src, 
                                            ISpanInterpolator inter) : base(src, inter, null)
        {
            OrderR = src.PixelFormat.Blender.OrderR;
            OrderG = src.PixelFormat.Blender.OrderG;
            OrderB = src.PixelFormat.Blender.OrderB;
            OrderA = src.PixelFormat.Blender.OrderA;
        }

#if use_timers
        static CNamedTimer Generate_Span = new CNamedTimer("Generate_Span rgb");
#endif
        //--------------------------------------------------------------------
        public override unsafe void Generate(RGBA_Bytes* span, int x, int y, uint len)
        {
#if use_timers
            Generate_Span.Start();
#endif
            base.interpolator().Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), len);

            uint* fg = stackalloc uint[3];
            uint src_alpha;

            RasterBuffer SourceRenderingBuffer = base.source().PixelFormat.RenderingBuffer;
            ISpanInterpolator spanInterpolator = base.interpolator();

            unchecked
            {
                do
                {
                    int x_hr;
                    int y_hr;
                    
                    spanInterpolator.Coordinates(out x_hr, out y_hr);

                    x_hr -= base.filter_dx_int();
                    y_hr -= base.filter_dy_int();

                    int x_lrX3 = (x_hr >> (int)image_subpixel_scale_e.Shift) * 3;
                    int y_lr = y_hr >> (int)image_subpixel_scale_e.Shift;
                    uint weight;

                    fg[0] = 
                    fg[1] =
                    fg[2] = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / 2;

                    x_hr &= (int)image_subpixel_scale_e.Mask;
                    y_hr &= (int)image_subpixel_scale_e.Mask;

                    byte* fg_ptr = SourceRenderingBuffer.GetPixelPointer(y_lr) + x_lrX3;

                    weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) *
                             ((int)image_subpixel_scale_e.Scale - y_hr));
                    fg[0] += weight * fg_ptr[0];
                    fg[1] += weight * fg_ptr[1];
                    fg[2] += weight * fg_ptr[2];

                    weight = (uint)(x_hr * ((int)image_subpixel_scale_e.Scale - y_hr));
                    fg[0] += weight * fg_ptr[3];
                    fg[1] += weight * fg_ptr[4];
                    fg[2] += weight * fg_ptr[5];

                    ++y_lr;
                    fg_ptr = SourceRenderingBuffer.GetPixelPointer(y_lr) + x_lrX3;

                    weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) * y_hr);
                    fg[0] += weight * fg_ptr[0];
                    fg[1] += weight * fg_ptr[1];
                    fg[2] += weight * fg_ptr[2];

                    weight = (uint)(x_hr * y_hr);
                    fg[0] += weight * fg_ptr[3];
                    fg[1] += weight * fg_ptr[4];
                    fg[2] += weight * fg_ptr[5];

                    fg[0] >>= (int)image_subpixel_scale_e.Shift * 2;
                    fg[1] >>= (int)image_subpixel_scale_e.Shift * 2;
                    fg[2] >>= (int)image_subpixel_scale_e.Shift * 2;
                    src_alpha = base_mask;

                    (*span).m_R = (byte)fg[OrderR];
                    (*span).m_G = (byte)fg[OrderG];
                    (*span).m_B = (byte)fg[OrderB];
                    (*span).m_A = (byte)src_alpha;
                    ++span;
                    spanInterpolator.Next();

                } while(--len != 0);
            }
#if use_timers
            Generate_Span.Stop();
#endif
        }

        unsafe private void BlendInFilterPixel(uint* fg, ref uint src_alpha, uint back_r, uint back_g, uint back_b, uint back_a, RasterBuffer SourceRenderingBuffer, int maxx, int maxy, int x_lr, int y_lr, uint weight)
        {
            byte* fg_ptr;
            unchecked
            {
                if ((uint)x_lr <= (uint)maxx && (uint)y_lr <= (uint)maxy)
                {
                    fg_ptr = SourceRenderingBuffer.GetPixelPointer(y_lr) + x_lr * 3;

                    fg[0] += weight * fg_ptr[0];
                    fg[1] += weight * fg_ptr[1];
                    fg[2] += weight * fg_ptr[2];
                    src_alpha += weight * base_mask;
                }
                else
                {
                    fg[OrderR] += back_r * weight;
                    fg[OrderG] += back_g * weight;
                    fg[OrderB] += back_b * weight;
                    src_alpha += back_a * weight;
                }
            }
        }
    };

    //=====================================span_image_filter_rgb_bilinear_clip
    public class span_image_filter_rgb_bilinear_clip : SpanImageFilter
    {
        private int OrderR;
        private int OrderG;
        private int OrderB;
        private int OrderA;
        private RGBA_Bytes m_back_color;

        const int base_shift = 8;
        const uint base_scale = (uint)(1 << base_shift);
        const uint base_mask = base_scale - 1;

        //--------------------------------------------------------------------
        public span_image_filter_rgb_bilinear_clip(IRasterBufferAccessor src, 
                                            IColorType back_color,
                                            ISpanInterpolator inter) : base(src, inter, null)
        {
            m_back_color = back_color.GetAsRGBA_Bytes();
            OrderR = src.PixelFormat.Blender.OrderR;
            OrderG = src.PixelFormat.Blender.OrderG;
            OrderB = src.PixelFormat.Blender.OrderB;
            OrderA = src.PixelFormat.Blender.OrderA;
        }
        public IColorType background_color() { return m_back_color; }
        public void background_color(IColorType v) { m_back_color = v.GetAsRGBA_Bytes(); }

#if use_timers
        static CNamedTimer Generate_Span = new CNamedTimer("Generate_Span rgb");
#endif
        //--------------------------------------------------------------------
        public override unsafe void Generate(RGBA_Bytes* span, int x, int y, uint len)
        {
#if use_timers
            Generate_Span.Start();
#endif
            base.interpolator().Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), len);

            uint* fg = stackalloc uint[3];
            uint src_alpha;

            uint back_r = m_back_color.m_R;
            uint back_g = m_back_color.m_G;
            uint back_b = m_back_color.m_B;
            uint back_a = m_back_color.m_A;

            byte *fg_ptr;

            RasterBuffer SourceRenderingBuffer = base.source().PixelFormat.RenderingBuffer;
            int maxx = (int)SourceRenderingBuffer.Width() - 1;
            int maxy = (int)SourceRenderingBuffer.Height() - 1;
            ISpanInterpolator spanInterpolator = base.interpolator();

            unchecked
            {
                do
                {
                    int x_hr;
                    int y_hr;
                    
                    spanInterpolator.Coordinates(out x_hr, out y_hr);

                    x_hr -= base.filter_dx_int();
                    y_hr -= base.filter_dy_int();

                    int x_lr = x_hr >> (int)image_subpixel_scale_e.Shift;
                    int y_lr = y_hr >> (int)image_subpixel_scale_e.Shift;
                    uint weight;

                    if(x_lr >= 0    && y_lr >= 0 &&
                       x_lr <  maxx && y_lr <  maxy) 
                    {
                        fg[0] = 
                        fg[1] =
                        fg[2] = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / 2;

                        x_hr &= (int)image_subpixel_scale_e.Mask;
                        y_hr &= (int)image_subpixel_scale_e.Mask;

                        fg_ptr = SourceRenderingBuffer.GetPixelPointer(y_lr) + x_lr * 3;

                        weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) *
                                 ((int)image_subpixel_scale_e.Scale - y_hr));
                        fg[0] += weight * fg_ptr[0];
                        fg[1] += weight * fg_ptr[1];
                        fg[2] += weight * fg_ptr[2];

                        weight = (uint)(x_hr * ((int)image_subpixel_scale_e.Scale - y_hr));
                        fg[0] += weight * fg_ptr[3];
                        fg[1] += weight * fg_ptr[4];
                        fg[2] += weight * fg_ptr[5];

                        ++y_lr;
                        fg_ptr = SourceRenderingBuffer.GetPixelPointer(y_lr) + x_lr * 3;

                        weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) * y_hr);
                        fg[0] += weight * fg_ptr[0];
                        fg[1] += weight * fg_ptr[1];
                        fg[2] += weight * fg_ptr[2];

                        weight = (uint)(x_hr * y_hr);
                        fg[0] += weight * fg_ptr[3];
                        fg[1] += weight * fg_ptr[4];
                        fg[2] += weight * fg_ptr[5];

                        fg[0] >>= (int)image_subpixel_scale_e.Shift * 2;
                        fg[1] >>= (int)image_subpixel_scale_e.Shift * 2;
                        fg[2] >>= (int)image_subpixel_scale_e.Shift * 2;
                        src_alpha = base_mask;
                    }
                    else
                    {
                        if(x_lr < -1   || y_lr < -1 ||
                           x_lr > maxx || y_lr > maxy)
                        {
                            fg[OrderR] = back_r;
                            fg[OrderG] = back_g;
                            fg[OrderB] = back_b;
                            src_alpha         = back_a;
                        }
                        else
                        {
                            fg[0] = 
                            fg[1] =
                            fg[2] = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / 2;
                            src_alpha = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / 2;

                            x_hr &= (int)image_subpixel_scale_e.Mask;
                            y_hr &= (int)image_subpixel_scale_e.Mask;

                            weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) *
                                     ((int)image_subpixel_scale_e.Scale - y_hr));
                            BlendInFilterPixel(fg, ref src_alpha, back_r, back_g, back_b, back_a, SourceRenderingBuffer, maxx, maxy, x_lr, y_lr, weight);

                            x_lr++;

                            weight = (uint)(x_hr * ((int)image_subpixel_scale_e.Scale - y_hr));
                            BlendInFilterPixel(fg, ref src_alpha, back_r, back_g, back_b, back_a, SourceRenderingBuffer, maxx, maxy, x_lr, y_lr, weight);

                            x_lr--;
                            y_lr++;

                            weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) * y_hr);
                            BlendInFilterPixel(fg, ref src_alpha, back_r, back_g, back_b, back_a, SourceRenderingBuffer, maxx, maxy, x_lr, y_lr, weight);

                            x_lr++;

                            weight = (uint)(x_hr * y_hr);
                            BlendInFilterPixel(fg, ref src_alpha, back_r, back_g, back_b, back_a, SourceRenderingBuffer, maxx, maxy, x_lr, y_lr, weight);

                            fg[0] >>= (int)image_subpixel_scale_e.Shift * 2;
                            fg[1] >>= (int)image_subpixel_scale_e.Shift * 2;
                            fg[2] >>= (int)image_subpixel_scale_e.Shift * 2;
                            src_alpha >>= (int)image_subpixel_scale_e.Shift * 2;
                        }
                    }

                    (*span).m_R = (byte)fg[0];
                    (*span).m_G = (byte)fg[1];
                    (*span).m_B = (byte)fg[2];
                    (*span).m_A = (byte)src_alpha;
                    ++span;
                    spanInterpolator.Next();

                } while(--len != 0);
            }
#if use_timers
            Generate_Span.Stop();
#endif
        }

        unsafe private void BlendInFilterPixel(uint* fg, ref uint src_alpha, uint back_r, uint back_g, uint back_b, uint back_a, RasterBuffer SourceRenderingBuffer, int maxx, int maxy, int x_lr, int y_lr, uint weight)
        {
            byte* fg_ptr;
            unchecked
            {
                if ((uint)x_lr <= (uint)maxx && (uint)y_lr <= (uint)maxy)
                {
                    fg_ptr = SourceRenderingBuffer.GetPixelPointer(y_lr) + x_lr * 3;

                    fg[0] += weight * fg_ptr[0];
                    fg[1] += weight * fg_ptr[1];
                    fg[2] += weight * fg_ptr[2];
                    src_alpha += weight * base_mask;
                }
                else
                {
                    fg[OrderR] += back_r * weight;
                    fg[OrderG] += back_g * weight;
                    fg[OrderB] += back_b * weight;
                    src_alpha += back_a * weight;
                }
            }
        }
    };

    //===================================================span_image_filter_rgb
    public class span_image_filter_rgb : SpanImageFilter
    {
        private int OrderR;
        private int OrderG;
        private int OrderB;
        private int OrderA;
        private uint base_mask = 255;

        //--------------------------------------------------------------------
        public span_image_filter_rgb(IRasterBufferAccessor src, ISpanInterpolator inter, ImageFilterLookUpTable filter)
            : base(src, inter, filter) 
        {
            if(src.PixelFormat.PixelWidthInBytes != 3)
            {
                throw new System.NotSupportedException("span_image_filter_rgb must have a 24 bit PixelFormat");
            }
            OrderR = src.PixelFormat.Blender.OrderR;
            OrderG = src.PixelFormat.Blender.OrderG;
            OrderB = src.PixelFormat.Blender.OrderB;
            OrderA = src.PixelFormat.Blender.OrderA;
        }

        //--------------------------------------------------------------------
        public unsafe override void Generate(RGBA_Bytes* span, int x, int y, uint len)
        {
            base.interpolator().Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), len);

            int* fg = stackalloc int[3];

            byte* fg_ptr;

            uint     diameter     = m_filter.diameter();
            int          start        = m_filter.start();
            short[] weight_array = m_filter.weight_array();

            int x_count; 
            int weight_y;

            ISpanInterpolator spanInterpolator = base.interpolator();

            do
            {
                spanInterpolator.Coordinates(out x, out y);

                x -= base.filter_dx_int();
                y -= base.filter_dy_int();

                int x_hr = x; 
                int y_hr = y;

                int x_lr = x_hr >> (int)image_subpixel_scale_e.Shift;
                int y_lr = y_hr >> (int)image_subpixel_scale_e.Shift;

                fg[0] = fg[1] = fg[2] = (int)image_filter_scale_e.Scale / 2;

                int x_fract = x_hr & (int)image_subpixel_scale_e.Mask;
                uint y_count = diameter;

                y_hr = (int)image_subpixel_scale_e.Mask - (y_hr & (int)image_subpixel_scale_e.Mask);
                fg_ptr = source().Span(x_lr + start, y_lr + start, diameter);
                for(;;)
                {
                    x_count  = (int)diameter;
                    weight_y = weight_array[y_hr];
                    x_hr = (int)image_subpixel_scale_e.Mask - x_fract;
                    for(;;)
                    {
                        int weight = (weight_y * weight_array[x_hr] +
                                     (int)image_filter_scale_e.Scale / 2) >>
                                     (int)image_filter_scale_e.Shift;

                        fg[0] += weight * *fg_ptr++;
                        fg[1] += weight * *fg_ptr++;
                        fg[2] += weight * *fg_ptr;

                        if(--x_count == 0) break;
                        x_hr += (int)image_subpixel_scale_e.Scale;
                        fg_ptr = source().NextX;
                    }

                    if(--y_count == 0) break;
                    y_hr += (int)image_subpixel_scale_e.Scale;
                    fg_ptr = source().NextY;
                }

                fg[0] >>= (int)image_filter_scale_e.Shift;
                fg[1] >>= (int)image_filter_scale_e.Shift;
                fg[2] >>= (int)image_filter_scale_e.Shift;

                if(fg[0] < 0) fg[0] = 0;
                if(fg[1] < 0) fg[1] = 0;
                if(fg[2] < 0) fg[2] = 0;

                if(fg[OrderR] > base_mask) fg[OrderR] = (int)base_mask;
                if (fg[OrderG] > base_mask) fg[OrderG] = (int)base_mask;
                if (fg[OrderB] > base_mask) fg[OrderB] = (int)base_mask;

                span->R_Byte = (Byte)fg[OrderR];
                span->G_Byte = (Byte)fg[OrderG];
                span->B_Byte = (Byte)fg[OrderB];
                span->A_Byte = base_mask;

                ++span;
                spanInterpolator.Next();

            } while(--len != 0);
        }
    };

    //===============================================span_image_filter_rgb_2x2
    public class span_image_filter_rgb_2x2 : SpanImageFilter
    {
        private int OrderR;
        private int OrderG;
        private int OrderB;
        private int OrderA;
        private const int base_mask = 255;

        //--------------------------------------------------------------------
        public span_image_filter_rgb_2x2(IRasterBufferAccessor src, ISpanInterpolator inter, ImageFilterLookUpTable filter)
            : base(src, inter, filter) 
        {
            OrderR = src.PixelFormat.Blender.OrderR;
            OrderG = src.PixelFormat.Blender.OrderG;
            OrderB = src.PixelFormat.Blender.OrderB;
            OrderA = src.PixelFormat.Blender.OrderA;
        }


        //--------------------------------------------------------------------
        public unsafe override void Generate(RGBA_Bytes* span, int x, int y, uint len)
        {
            ISpanInterpolator spanInterpolator = base.interpolator();
            spanInterpolator.Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), len);

            int* fg = stackalloc int[3];

            byte* fg_ptr;
            fixed (short* pWeightArray = filter().weight_array())
            {
                short* weight_array = pWeightArray;
                weight_array = &pWeightArray[((filter().diameter() / 2 - 1) << (int)image_subpixel_scale_e.Shift)];

                do
                {
                    int x_hr;
                    int y_hr;

                    spanInterpolator.Coordinates(out x_hr, out y_hr);

                    x_hr -= filter_dx_int();
                    y_hr -= filter_dy_int();

                    int x_lr = x_hr >> (int)image_subpixel_scale_e.Shift;
                    int y_lr = y_hr >> (int)image_subpixel_scale_e.Shift;

                    uint weight;
                    fg[0] = fg[1] = fg[2] = (int)image_filter_scale_e.Scale / 2;

                    x_hr &= (int)image_subpixel_scale_e.Mask;
                    y_hr &= (int)image_subpixel_scale_e.Mask;

                    fg_ptr = source().Span(x_lr, y_lr, 2);
                    weight = (uint)((weight_array[x_hr + (int)image_subpixel_scale_e.Scale] *
                              weight_array[y_hr + (int)image_subpixel_scale_e.Scale] +
                              (int)image_filter_scale_e.Scale / 2) >>
                              (int)image_filter_scale_e.Shift);
                    fg[0] += (int)weight * *fg_ptr++;
                    fg[1] += (int)weight * *fg_ptr++;
                    fg[2] += (int)weight * *fg_ptr;

                    fg_ptr = source().NextX;
                    weight = (uint)((weight_array[x_hr] *
                              weight_array[y_hr + (int)image_subpixel_scale_e.Scale] +
                              (int)image_filter_scale_e.Scale / 2) >>
                              (int)image_filter_scale_e.Shift);
                    fg[0] += (int)weight * *fg_ptr++;
                    fg[1] += (int)weight * *fg_ptr++;
                    fg[2] += (int)weight * *fg_ptr;

                    fg_ptr = source().NextY;
                    weight = (uint)((weight_array[x_hr + (int)image_subpixel_scale_e.Scale] *
                              weight_array[y_hr] +
                              (int)image_filter_scale_e.Scale / 2) >>
                              (int)image_filter_scale_e.Shift);
                    fg[0] += (int)weight * *fg_ptr++;
                    fg[1] += (int)weight * *fg_ptr++;
                    fg[2] += (int)weight * *fg_ptr;

                    fg_ptr = source().NextX;
                    weight = (uint)((weight_array[x_hr] *
                              weight_array[y_hr] +
                              (int)image_filter_scale_e.Scale / 2) >>
                              (int)image_filter_scale_e.Shift);
                    fg[0] += (int)weight * *fg_ptr++;
                    fg[1] += (int)weight * *fg_ptr++;
                    fg[2] += (int)weight * *fg_ptr;

                    fg[0] >>= (int)image_filter_scale_e.Shift;
                    fg[1] >>= (int)image_filter_scale_e.Shift;
                    fg[2] >>= (int)image_filter_scale_e.Shift;

                    if (fg[OrderR] > base_mask) fg[OrderR] = base_mask;
                    if (fg[OrderG] > base_mask) fg[OrderG] = base_mask;
                    if (fg[OrderB] > base_mask) fg[OrderB] = base_mask;

                    span->R_Byte = (byte)fg[OrderR];
                    span->G_Byte = (byte)fg[OrderG];
                    span->B_Byte = (byte)fg[OrderB];
                    span->A_Byte = (uint)base_mask;

                    ++span;
                    spanInterpolator.Next();

                } while (--len != 0);
            }
        }
    };

    /*
    //==========================================span_image_resample_rgb_affine
    template<class Source> 
    class span_image_resample_rgb_affine : 
    public span_image_resample_affine<Source>
    {
    public:
        typedef Source source_type;
        typedef typename source_type::color_type color_type;
        typedef typename source_type::order_type order_type;
        typedef span_image_resample_affine<source_type> base_type;
        typedef typename base_type::interpolator_type interpolator_type;
        typedef typename color_type::value_type value_type;
        typedef typename color_type::long_type long_type;
        enum base_scale_e
        {
            base_shift      = 8,//color_type::base_shift,
            base_mask       = 255,//color_type::base_mask,
            downscale_shift = Shift
        };

        //--------------------------------------------------------------------
        span_image_resample_rgb_affine() {}
        span_image_resample_rgb_affine(source_type& src, 
                                       interpolator_type& inter,
                                       const ImageFilterLookUpTable& filter) :
            base(src, inter, filter) 
        {}


        //--------------------------------------------------------------------
        void Generate(color_type* Span, int x, int y, unsigned len)
        {
            base_type::Interpolator().Begin(x + base_type::filter_dx_dbl(), 
                                            y + base_type::filter_dy_dbl(), len);

            long_type fg[3];

            int diameter     = base_type::filter().diameter();
            int filter_scale = diameter << Shift;
            int radius_x     = (diameter * base_type::m_rx) >> 1;
            int radius_y     = (diameter * base_type::m_ry) >> 1;
            int len_x_lr     = 
                (diameter * base_type::m_rx + Mask) >> 
                    Shift;

            const int16* weight_array = base_type::filter().weight_array();

            do
            {
                base_type::Interpolator().Coordinates(&x, &y);

                x += base_type::filter_dx_int() - radius_x;
                y += base_type::filter_dy_int() - radius_y;

                fg[0] = fg[1] = fg[2] = Scale / 2;

                int y_lr = y >> Shift;
                int y_hr = ((Mask - (y & Mask)) * 
                                base_type::m_ry_inv) >> 
                                    Shift;
                int total_weight = 0;
                int x_lr = x >> Shift;
                int x_hr = ((Mask - (x & Mask)) * 
                                base_type::m_rx_inv) >> 
                                    Shift;

                int x_hr2 = x_hr;
                const value_type* fg_ptr = 
                    source().PixelPointer(x_lr, y_lr, len_x_lr);
                for(;;)
                {
                    int weight_y = weight_array[y_hr];
                    x_hr = x_hr2;
                    for(;;)
                    {
                        int weight = (weight_y * weight_array[x_hr] + 
                                     Scale / 2) >> 
                                     downscale_shift;

                        fg[0] += *fg_ptr++ * weight;
                        fg[1] += *fg_ptr++ * weight;
                        fg[2] += *fg_ptr   * weight;
                        total_weight += weight;
                        x_hr  += base_type::m_rx_inv;
                        if(x_hr >= filter_scale) break;
                        fg_ptr = SourceRenderingBuffer.NextX();
                    }
                    y_hr += base_type::m_ry_inv;
                    if(y_hr >= filter_scale) break;
                    fg_ptr = SourceRenderingBuffer.NextY();
                }

                fg[0] /= total_weight;
                fg[1] /= total_weight;
                fg[2] /= total_weight;

                if(fg[0] < 0) fg[0] = 0;
                if(fg[1] < 0) fg[1] = 0;
                if(fg[2] < 0) fg[2] = 0;

                if(fg[order_type::R] > base_mask) fg[order_type::R] = base_mask;
                if(fg[order_type::G] > base_mask) fg[order_type::G] = base_mask;
                if(fg[order_type::B] > base_mask) fg[order_type::B] = base_mask;

                Span->r = (value_type)fg[order_type::R];
                Span->g = (value_type)fg[order_type::G];
                Span->b = (value_type)fg[order_type::B];
                Span->a = base_mask;

                ++Span;
                ++base_type::Interpolator();
            } while(--len);
        }
    };
     */


    //=================================================SpanImageResampleRGB
    public class SpanImageResampleRGB
        : SpanImageResample
    {
        private int OrderR;
        private int OrderG;
        private int OrderB;
        private const int base_mask = 255;
        private const int downscale_shift = (int)ImageFilterLookUpTable.EImageFilterScale.Shift;

        //--------------------------------------------------------------------
        public SpanImageResampleRGB(IRasterBufferAccessor src, 
                            ISpanInterpolator inter,
                            ImageFilterLookUpTable filter) :
            base(src, inter, filter)
        {
            if(src.PixelFormat.Blender.NumPixelBits != 24)
            {
                throw new System.FormatException("You have to use a rgb blender with SpanImageResampleRGB");
            }
            OrderR = src.PixelFormat.Blender.OrderR;
            OrderG = src.PixelFormat.Blender.OrderG;
            OrderB = src.PixelFormat.Blender.OrderB;
        }

        //--------------------------------------------------------------------
        public unsafe override void Generate(RGBA_Bytes* span, int x, int y, uint len)
        {
            ISpanInterpolator spanInterpolator = base.interpolator();
            spanInterpolator.Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), len);

            int* fg = stackalloc int[3];

            byte* fg_ptr;
            fixed (short* pWeightArray = filter().weight_array())
            {
                int diameter = (int)base.filter().diameter();
                int filter_scale = diameter << (int)image_subpixel_scale_e.Shift;

                short* weight_array = pWeightArray;

                do
                {
                    int rx;
                    int ry;
                    int rx_inv = (int)image_subpixel_scale_e.Scale;
                    int ry_inv = (int)image_subpixel_scale_e.Scale;
                    spanInterpolator.Coordinates(out x, out y);
                    spanInterpolator.LocalScale(out rx, out ry);
                    base.AdjustScale(ref rx, ref ry);

                    rx_inv = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / rx;
                    ry_inv = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / ry;

                    int radius_x = (diameter * rx) >> 1;
                    int radius_y = (diameter * ry) >> 1;
                    int len_x_lr =
                        (diameter * rx + (int)image_subpixel_scale_e.Mask) >>
                            (int)(int)image_subpixel_scale_e.Shift;

                    x += base.filter_dx_int() - radius_x;
                    y += base.filter_dy_int() - radius_y;

                    fg[0] = fg[1] = fg[2] = (int)image_filter_scale_e.Scale / 2;

                    int y_lr = y >> (int)(int)image_subpixel_scale_e.Shift;
                    int y_hr = (((int)image_subpixel_scale_e.Mask - (y & (int)image_subpixel_scale_e.Mask)) * 
                                   ry_inv) >>
                                       (int)(int)image_subpixel_scale_e.Shift;
                    int total_weight = 0;
                    int x_lr = x >> (int)(int)image_subpixel_scale_e.Shift;
                    int x_hr = (((int)image_subpixel_scale_e.Mask - (x & (int)image_subpixel_scale_e.Mask)) * 
                                   rx_inv) >>
                                       (int)(int)image_subpixel_scale_e.Shift;
                    int x_hr2 = x_hr;
                    fg_ptr = base.source().Span(x_lr, y_lr, (uint)len_x_lr);

                    for(;;)
                    {
                        int weight_y = weight_array[y_hr];
                        x_hr = x_hr2;
                        for(;;)
                        {
                            int weight = (weight_y * weight_array[x_hr] +
                                         (int)image_filter_scale_e.Scale / 2) >> 
                                         downscale_shift;
                            fg[0] += *fg_ptr++ * weight;
                            fg[1] += *fg_ptr++ * weight;
                            fg[2] += *fg_ptr++ * weight;
                            total_weight += weight;
                            x_hr  += rx_inv;
                            if(x_hr >= filter_scale) break;
                            fg_ptr = base.source().NextX;
                        }
                        y_hr += ry_inv;
                        if (y_hr >= filter_scale)
                        {
                            break;
                        }

                        fg_ptr = base.source().NextY;
                    }

                    fg[0] /= total_weight;
                    fg[1] /= total_weight;
                    fg[2] /= total_weight;

                    if(fg[0] < 0) fg[0] = 0;
                    if(fg[1] < 0) fg[1] = 0;
                    if(fg[2] < 0) fg[2] = 0;

                    if(fg[0] > fg[0]) fg[0] = fg[0];
                    if(fg[1] > fg[1]) fg[1] = fg[1];
                    if(fg[2] > fg[2]) fg[2] = fg[2];

                    span->R_Byte = (byte)fg[OrderR];
                    span->G_Byte = (byte)fg[OrderG];
                    span->B_Byte = (byte)fg[OrderB];
                    span->A_Byte = (byte)base_mask;

                    ++span;
                    interpolator().Next();
                } while(--len != 0);
            }
        }
    };
}