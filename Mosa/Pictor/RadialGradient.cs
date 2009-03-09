using System;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class RadialGradient : Gradient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        internal RadialGradient(IntPtr handle)
            : base(handle)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cx0"></param>
        /// <param name="cy0"></param>
        /// <param name="radius0"></param>
        /// <param name="cx1"></param>
        /// <param name="cy1"></param>
        /// <param name="radius1"></param>
        public RadialGradient(double cx0, double cy0, double radius0, double cx1, double cy1, double radius1)
        {
            pattern = IntPtr.Zero; // NativeMethods.cairo_pattern_create_radial(cx0, cy0, radius0, cx1, cy1, radius1);
        }
    }
}

