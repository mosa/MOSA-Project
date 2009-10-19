/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
using System;
using image_subpixel_scale_e = Pictor.ImageFilterLookUpTable.EImageSubpixelScale;

namespace Pictor
{
    public interface ISpanGenerator
    {
        void Prepare();
        unsafe void Generate(RGBA_Bytes* span, int x, int y, uint len);
    };

    //-------------------------------------------------------SpanImageFilter
    public abstract class SpanImageFilter : ISpanGenerator
    {
        public SpanImageFilter() {}
        public SpanImageFilter(IRasterBufferAccessor src,
            ISpanInterpolator interpolator) : this(src, interpolator, null)
        {

        }

        public SpanImageFilter(IRasterBufferAccessor src,
            ISpanInterpolator interpolator, ImageFilterLookUpTable filter)
        {
            m_src = src;
            m_interpolator = interpolator;
            m_filter = (filter);
            m_dx_dbl = (0.5);
            m_dy_dbl = (0.5);
            m_dx_int = ((int)image_subpixel_scale_e.Scale / 2);
            m_dy_int = ((int)image_subpixel_scale_e.Scale / 2);
        }
        public void attach(IRasterBufferAccessor v) { m_src = v; }

        public unsafe abstract void Generate(RGBA_Bytes* span, int x, int y, uint len);

        //--------------------------------------------------------------------
        public IRasterBufferAccessor source() { return m_src; }
        public ImageFilterLookUpTable filter() { return m_filter; }
        public int    filter_dx_int()            { return (int)m_dx_int; }
        public int filter_dy_int() { return (int)m_dy_int; }
        public double filter_dx_dbl()            { return m_dx_dbl; }
        public double filter_dy_dbl()            { return m_dy_dbl; }

        //--------------------------------------------------------------------
        public void interpolator(ISpanInterpolator v) { m_interpolator = v; }
        public void filter(ImageFilterLookUpTable v)   { m_filter = v; }
        public void filter_offset(double dx, double dy)
        {
            m_dx_dbl = dx;
            m_dy_dbl = dy;
            m_dx_int = (uint)Basics.Round(dx * (int)image_subpixel_scale_e.Scale);
            m_dy_int = (uint)Basics.Round(dy * (int)image_subpixel_scale_e.Scale);
        }
        public void filter_offset(double d) { filter_offset(d, d); }

        //--------------------------------------------------------------------
        public ISpanInterpolator interpolator() { return m_interpolator; }

        //--------------------------------------------------------------------
        public void Prepare() {}

        //--------------------------------------------------------------------
        private IRasterBufferAccessor m_src;
        private ISpanInterpolator m_interpolator;
        protected ImageFilterLookUpTable m_filter;
        private double   m_dx_dbl;
        private double   m_dy_dbl;
        private uint m_dx_int;
        private uint m_dy_int;
    };

    /*


    //==============================================span_image_resample_affine
    //template<class Source> 
    public class span_image_resample_affine : 
        SpanImageFilter//<Source, LinearSpanInterpolator<trans_affine> >
    {
        //typedef Source IImageAccessor;
        //typedef LinearSpanInterpolator<trans_affine> ISpanInterpolator;
        //typedef SpanImageFilter<source_type, ISpanInterpolator> base_type;

        //--------------------------------------------------------------------
        public span_image_resample_affine()
        {
            m_scale_limit=(200.0);
            m_blur_x=(1.0);
            m_blur_y=(1.0);
        }

        //--------------------------------------------------------------------
        public span_image_resample_affine(IImageAccessor src, 
                                   ISpanInterpolator inter,
                                   ImageFilterLookUpTable filter) : base(src, inter, filter)
        {
            m_scale_limit(200.0);
            m_blur_x(1.0);
            m_blur_y(1.0);
        }


        //--------------------------------------------------------------------
        public int  ScaleLimit() { return UnsignedRound(m_scale_limit); }
        public void ScaleLimit(int v)  { m_scale_limit = v; }

        //--------------------------------------------------------------------
        public double xBlur() { return m_blur_x; }
        public double BlurY() { return m_blur_y; }
        public void xBlur(double v) { m_blur_x = v; }
        public void BlurY(double v) { m_blur_y = v; }
        public void Blur(double v) { m_blur_x = m_blur_y = v; }

        //--------------------------------------------------------------------
        public void Prepare() 
        {
            double xScale;
            double yScale;

            base_type::Interpolator().Transformer().ScalingAbs(&xScale, &yScale);

            if(xScale * yScale > m_scale_limit)
            {
                xScale = xScale * m_scale_limit / (xScale * yScale);
                yScale = yScale * m_scale_limit / (xScale * yScale);
            }

            if(xScale < 1) xScale = 1;
            if(yScale < 1) yScale = 1;

            if(xScale > m_scale_limit) xScale = m_scale_limit;
            if(yScale > m_scale_limit) yScale = m_scale_limit;

            xScale *= m_blur_x;
            yScale *= m_blur_y;

            if(xScale < 1) xScale = 1;
            if(yScale < 1) yScale = 1;

            m_rx     = UnsignedRound(    xScale * (double)(Scale));
            m_rx_inv = UnsignedRound(1.0/xScale * (double)(Scale));

            m_ry     = UnsignedRound(    yScale * (double)(Scale));
            m_ry_inv = UnsignedRound(1.0/yScale * (double)(Scale));
        }

        protected int m_rx;
        protected int m_ry;
        protected int m_rx_inv;
        protected int m_ry_inv;

        private double m_scale_limit;
        private double m_blur_x;
        private double m_blur_y;
    };

     */


    //=====================================================SpanImageResample
    public abstract class SpanImageResample 
        : SpanImageFilter
    {
        public SpanImageResample(IRasterBufferAccessor src, 
                            ISpanInterpolator inter,
                            ImageFilterLookUpTable filter) : base(src, inter, filter)
        {
            m_scale_limit=(20);
            m_blur_x = ((int)image_subpixel_scale_e.Scale);
            m_blur_y = ((int)image_subpixel_scale_e.Scale);
        }

        //public abstract void Prepare();
        //public abstract unsafe void Generate(rgba8* Span, int x, int y, uint len);

        //--------------------------------------------------------------------
        int ScaleLimit
        {
            get { return m_scale_limit; }
            set { m_scale_limit = value; }
        }

        //--------------------------------------------------------------------
        double xBlur
        {
            get { return (double)(m_blur_x) / (double)((int)image_subpixel_scale_e.Scale); }
            set { m_blur_x = (int)Basics.UnsignedRound(value * (double)((int)image_subpixel_scale_e.Scale)); }
        }
        double yBlur
        {
            get { return (double)(m_blur_y) / (double)((int)image_subpixel_scale_e.Scale); }
            set { m_blur_y = (int)Basics.UnsignedRound(value * (double)((int)image_subpixel_scale_e.Scale)); }
        }
        public double Blur
        {
            set { m_blur_x = m_blur_y = (int)Basics.UnsignedRound(value * (double)((int)image_subpixel_scale_e.Scale)); }
        }

        protected void AdjustScale(ref int rx, ref int ry)
        {
            if (rx < (int)image_subpixel_scale_e.Scale) rx = (int)image_subpixel_scale_e.Scale;
            if (ry < (int)image_subpixel_scale_e.Scale) ry = (int)image_subpixel_scale_e.Scale;
            if (rx > (int)image_subpixel_scale_e.Scale * m_scale_limit) 
            {
                rx = (int)image_subpixel_scale_e.Scale * m_scale_limit;
            }
            if (ry > (int)image_subpixel_scale_e.Scale * m_scale_limit) 
            {
                ry = (int)image_subpixel_scale_e.Scale * m_scale_limit;
            }
            rx = (rx * m_blur_x) >> (int)image_subpixel_scale_e.Shift;
            ry = (ry * m_blur_y) >> (int)image_subpixel_scale_e.Shift;
            if (rx < (int)image_subpixel_scale_e.Scale) rx = (int)image_subpixel_scale_e.Scale;
            if (ry < (int)image_subpixel_scale_e.Scale) ry = (int)image_subpixel_scale_e.Scale;
        }

        int m_scale_limit;
        int m_blur_x;
        int m_blur_y;
    };
}
