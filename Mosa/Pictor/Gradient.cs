using System;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class Gradient : Pattern
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        protected Gradient(IntPtr handle)
            : base(handle)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected Gradient()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int ColorStopCount
        {
            get
            {
                int cnt = 0;
                //NativeMethods.cairo_pattern_get_color_stop_count(pattern, out cnt);
                return cnt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public Status AddColorStop(double offset, Pictor.Color c)
        {
            //NativeMethods.cairo_pattern_add_color_stop_rgba(pattern, offset, c.R, c.G, c.B, c.A);
            return Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public Status AddColorStopRgb(double offset, Pictor.Color c)
        {
            // NativeMethods.cairo_pattern_add_color_stop_rgb(pattern, offset, c.R, c.G, c.B);
            return Status;
        }
    }
}