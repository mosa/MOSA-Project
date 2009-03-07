using System;
using System.Collections.Generic;
using System.Text;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class FontFace : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        IntPtr handle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static FontFace Lookup(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                return null;

            //NativeMethods.cairo_font_face_reference(handle);

            return new FontFace(handle);
        }

        /// <summary>
        /// 
        /// </summary>
        ~FontFace()
        {
            // Since Cairo is not thread safe, we can not unref the
            // font_face here, the programmer must do this with IDisposable.Dispose

            Console.Error.WriteLine("Programmer forgot to call Dispose on the FontFace");
            Dispose(false);
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
            //if (disposing)
            //    NativeMethods.cairo_font_face_destroy(handle);
            handle = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }

        // TODO: make non-public when all entry points are complete in binding
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        public FontFace(IntPtr handle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// 
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return handle;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Status Status
        {
            get
            {
                return Status.ClipNotRepresentable;
                //return NativeMethods.cairo_font_face_status(handle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public FontType FontType
        {
            get
            {
                return FontType.Atsui;
                //return NativeMethods.cairo_font_face_get_type(handle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint ReferenceCount
        {
            get { return 0; }
            //get { return NativeMethods.cairo_font_face_get_reference_count(handle); }
        }
    }
}
