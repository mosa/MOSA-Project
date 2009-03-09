using System;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class SurfacePattern : Pattern
    {
        /// <summary>
        /// 
        /// </summary>
        protected Filter filter = Filter.Good;

        /// <summary>
        /// 
        /// </summary>
        protected Extend extend = Extend.None;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        public SurfacePattern(Surface surface)
        {
            pattern = IntPtr.Zero;// cairo_pattern_create_for_surface(surface.Handle);
        }

        /// <summary>
        /// 
        /// </summary>
        public Extend Extend
        {
            set 
            {
                extend = value;
                //NativeMethods.cairo_pattern_set_extend(pattern, value); 
            }
            get 
            {
                return extend;
                //return NativeMethods.cairo_pattern_get_extend(pattern); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Filter Filter
        {
            set 
            {
                filter = value;
                //NativeMethods.cairo_pattern_set_filter(pattern, value); 
            }
            get
            {
                return filter;
                //return NativeMethods.cairo_pattern_get_filter(pattern); }
            }
        }
    }
}