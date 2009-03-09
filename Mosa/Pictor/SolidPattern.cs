using System;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class SolidPattern : Pattern
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        internal SolidPattern(IntPtr handle)
            : base(handle)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        public SolidPattern(Color color)
        {
            pattern = IntPtr.Zero;// NativeMethods.cairo_pattern_create_rgba(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public SolidPattern(double r, double g, double b)
        {
            pattern = IntPtr.Zero;// NativeMethods.cairo_pattern_create_rgb(r, g, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public SolidPattern(double r, double g, double b, double a)
        {
            //NativeMethods.cairo_pattern_create_rgba(r, g, b, a);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="solid"></param>
        public SolidPattern(Color color, bool solid)
        {
            if (solid)
                pattern = IntPtr.Zero;//NativeMethods.cairo_pattern_create_rgb(color.R, color.G, color.B);
            else
                pattern = IntPtr.Zero;// NativeMethods.cairo_pattern_create_rgba(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get
            {
                double red = 0.0, green = 0.0, blue = 0.0, alpha = 0.0;

                //NativeMethods.cairo_pattern_get_rgba(pattern, out red, out green, out blue, out alpha);
                return new Color(red, green, blue, alpha);
            }
        }
    }
}