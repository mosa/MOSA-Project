using System;
using System.Runtime.InteropServices;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class Path : IDisposable
    {
        internal IntPtr handle = IntPtr.Zero;

        internal Path(IntPtr handle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// 
        /// </summary>
        ~Path()
        {
            Dispose(false);
        }


        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                Console.Error.WriteLine("Cairo.Context: called from finalization thread, programmer is missing a call to Dispose");
                return;
            }

            if (handle == IntPtr.Zero)
                return;

            //NativeMethods.cairo_path_destroy(handle);
            handle = IntPtr.Zero;
        }
    }
}
