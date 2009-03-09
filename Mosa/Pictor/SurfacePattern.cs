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
        /// <param name="handle"></param>
        internal SurfacePattern(IntPtr handle)
            : base(handle)
        {
        }

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
                //NativeMethods.cairo_pattern_set_extend(pattern, value); 
            }
            get 
            {
                return Extend.None;
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
                //NativeMethods.cairo_pattern_set_filter(pattern, value); 
            }
            get
            {
                return Filter.Good;
                //return NativeMethods.cairo_pattern_get_filter(pattern); }
            }
        }
    }
}