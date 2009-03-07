using System;
using System.Collections.Generic;
using System.Text;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class FontOptions : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        IntPtr handle;

        /// <summary>
        /// 
        /// </summary>
        bool disposed;

        /// <summary>
        /// 
        /// </summary>
        public FontOptions()
        {
            //handle = NativeMethods.cairo_font_options_create();
        }

        /// <summary>
        /// 
        /// </summary>
        ~FontOptions()
        {
            Dispose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        internal FontOptions(IntPtr handle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FontOptions Copy()
        {
            return new FontOptions();
            //return new FontOptions(NativeMethods.cairo_font_options_copy(handle));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Destroy()
        {
            //NativeMethods.cairo_font_options_destroy(handle);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                Destroy();
                handle = IntPtr.Zero;
            }
            disposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator ==(FontOptions options, FontOptions other)
        {
            return Equals(options, other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator !=(FontOptions options, FontOptions other)
        {
            return !(options == other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            return Equals(other as FontOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Equals(FontOptions options)
        {
            return options != null;// && NativeMethods.cairo_font_options_equal(Handle, options.Handle);
        }

        /// <summary>
        /// 
        /// </summary>
        public IntPtr Handle
        {
            get { return handle; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 0;
            //return (int)NativeMethods.cairo_font_options_hash(handle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void Merge(FontOptions other)
        {
            if (other == null)
                throw new ArgumentNullException("other");
            //NativeMethods.cairo_font_options_merge(handle, other.Handle);
        }

        /// <summary>
        /// 
        /// </summary>
        public Antialias Antialias
        {
            get { return Antialias.Default; }
            set {}
            //get { return NativeMethods.cairo_font_options_get_antialias(handle); }
            //set { NativeMethods.cairo_font_options_set_antialias(handle, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public HintMetrics HintMetrics
        {
            get { return HintMetrics.Default; }
            //get { return NativeMethods.cairo_font_options_get_hint_metrics(handle); }
            set {}
            //set { NativeMethods.cairo_font_options_set_hint_metrics(handle, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public HintStyle HintStyle
        {
            get { return HintStyle.Default; }
            set { }
            //get { return NativeMethods.cairo_font_options_get_hint_style(handle); }
            //set { NativeMethods.cairo_font_options_set_hint_style(handle, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Status Status
        {
            get { return Status.ClipNotRepresentable; }
            //get { return NativeMethods.cairo_font_options_status(handle); }
        }

        /// <summary>
        /// 
        /// </summary>
        public SubpixelOrder SubpixelOrder
        {
            get { return SubpixelOrder.Bgr; }
            set { }
            //get { return NativeMethods.cairo_font_options_get_subpixel_order(handle); }
            //set { NativeMethods.cairo_font_options_set_subpixel_order(handle, value); }
        }
    }
}
