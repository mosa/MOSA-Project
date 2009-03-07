using System;
using System.Runtime.InteropServices;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class ScaledFont : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected IntPtr handle = IntPtr.Zero;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        internal ScaledFont(IntPtr handle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontFace"></param>
        /// <param name="matrix"></param>
        /// <param name="ctm"></param>
        /// <param name="options"></param>
        public ScaledFont(FontFace fontFace, Matrix matrix, Matrix ctm, FontOptions options)
        {
            //handle = NativeMethods.cairo_scaled_font_create(fontFace.Handle, matrix, ctm, options.Handle);
        }

        /// <summary>
        /// 
        /// </summary>
        ~ScaledFont()
        {
            Dispose(false);
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
        public FontExtents FontExtents
        {
            get
            {
                FontExtents extents = new FontExtents();
                //NativeMethods.cairo_scaled_font_extents(handle, out extents);
                return extents;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Matrix FontMatrix
        {
            get
            {
                Matrix m = new Matrix();
                //NativeMethods.cairo_scaled_font_get_font_matrix(handle, out m);
                return m;
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
                //return NativeMethods.cairo_scaled_font_get_type(handle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="glyphs"></param>
        /// <returns></returns>
        public TextExtents GlyphExtents(Glyph[] glyphs)
        {
            IntPtr ptr = Context.FromGlyphToUnManagedMemory(glyphs);
            TextExtents extents = new TextExtents();

            //NativeMethods.cairo_scaled_font_glyph_extents(handle, ptr, glyphs.Length, out extents);

            Marshal.FreeHGlobal(ptr);
            return extents;
        }


        /// <summary>
        /// 
        /// </summary>
        public Status Status
        {
            get { return Status.ClipNotRepresentable; }
            //get { return NativeMethods.cairo_scaled_font_status(handle); }
        }

        /// <summary>
        /// 
        /// </summary>
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
            if (disposing)
            {
                //NativeMethods.cairo_scaled_font_destroy(handle);
                handle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Reference()
        {
            //NativeMethods.cairo_scaled_font_reference(handle);
        }
    }
}