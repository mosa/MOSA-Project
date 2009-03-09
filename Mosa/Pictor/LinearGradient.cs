using System;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class LinearGradient : Gradient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        internal LinearGradient(IntPtr handle)
            : base(handle)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        public LinearGradient(double x0, double y0, double x1, double y1)
        {
            pattern = IntPtr.Zero;// NativeMethods.cairo_pattern_create_linear(x0, y0, x1, y1);
        }

        /// <summary>
        /// 
        /// </summary>
        public PointD[] LinearPoints
        {
            get
            {
                double x0 = 0.0, y0 = 0.0, x1 = 0.0, y1 = 0.0;
                PointD[] points = new PointD[2];

                // NativeMethods.cairo_pattern_get_linear_points(pattern, out x0, out y0, out x1, out y1);

                points[0] = new PointD(x0, y0);
                points[1] = new PointD(x1, y1);
                return points;
            }
        }

    }
}

