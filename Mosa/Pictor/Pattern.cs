using System;
using System.Collections;
using System.Text;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class Pattern : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected IntPtr pattern = IntPtr.Zero;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        internal static Pattern Lookup(IntPtr pattern)
        {
            if (pattern == IntPtr.Zero)
                return null;

            object x = patterns[pattern];
            if (x != null)
                return (Pattern)x;

            PatternType pt = PatternType.Linear;// = NativeMethods.cairo_pattern_get_type(pattern);
            switch (pt)
            {
                /*case PatternType.Solid:
                    return new SolidPattern(pattern);
                case PatternType.Surface:
                    return new SurfacePattern(pattern);
                case PatternType.Linear:
                    return new LinearGradient(pattern);
                case PatternType.Radial:
                    return new RadialGradient(pattern);*/
                default:
                    return new Pattern(pattern);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Pattern()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        static Hashtable patterns = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        internal Pattern(IntPtr ptr)
        {
            lock (patterns)
            {
                patterns[ptr] = this;
            }
            pattern = ptr;
        }

        /// <summary>
        /// 
        /// </summary>
        ~Pattern()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        [Obsolete("Use the SurfacePattern constructor")]
        public Pattern(Surface surface)
        {
            //pattern = NativeMethods.cairo_pattern_create_for_surface(surface.Handle);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Reference()
        {
            //NativeMethods.cairo_pattern_reference(pattern);
        }

        /// <summary>
        /// 
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Destroy();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Destroy()
        {
            if (pattern != IntPtr.Zero)
            {
                //NativeMethods.cairo_pattern_destroy(pattern);
                pattern = IntPtr.Zero;
            }
            lock (patterns)
            {
                patterns.Remove(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Status Status
        {
            get { return Status.ClipNotRepresentable; }
            //get { return NativeMethods.cairo_pattern_status(pattern); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Matrix Matrix
        {
            set
            {
                //NativeMethods.cairo_pattern_set_matrix(pattern, value);
            }

            get
            {
                Matrix m = new Matrix();
                //NativeMethods.cairo_pattern_get_matrix(pattern, m);
                return m;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IntPtr Pointer
        {
            get { return pattern; }
        }

        /// <summary>
        /// 
        /// </summary>
        public PatternType PatternType
        {
            get { return PatternType.Linear; }
            //get { return NativeMethods.cairo_pattern_get_type(pattern); }
        }
    }
}
