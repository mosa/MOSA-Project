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
        protected Status status = Status.NullPointer;

        /// <summary>
        /// 
        /// </summary>
        protected Matrix matrix = new Matrix();

        /// <summary>
        /// 
        /// </summary>
        protected PatternType type = PatternType.Linear;

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
            pattern = IntPtr.Zero; // cairo_pattern_create_for_surface(surface.Handle);
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
            get 
            {
                return status;
                //return Status.ClipNotRepresentable; 
            }
            //get { return NativeMethods.cairo_pattern_status(pattern); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Matrix Matrix
        {
            set
            {
                matrix = value;
                //NativeMethods.cairo_pattern_set_matrix(pattern, value);
            }

            get
            {
                return matrix;
                //NativeMethods.cairo_pattern_get_matrix(pattern, m);
                //return m;
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
            get 
            { 
                return type; 
            }
            //get { return NativeMethods.cairo_pattern_get_type(pattern); }
        }
    }
}
