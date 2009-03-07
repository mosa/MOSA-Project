using System;
using System.Collections;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class Surface : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected static Hashtable surfaces = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        internal IntPtr surface = IntPtr.Zero;

        /// <summary>
        /// 
        /// </summary>
        protected Surface()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="owns"></param>
        protected Surface(IntPtr ptr, bool owns)
        {
            surface = ptr;
            lock (surfaces.SyncRoot)
            {
                surfaces[ptr] = this;
            }
            //if (!owns)
                //NativeMethods.cairo_surface_reference(ptr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        static internal Surface LookupExternalSurface(IntPtr p)
        {
            lock (surfaces.SyncRoot)
            {
                object o = surfaces[p];
                if (o == null)
                {
                    return new Surface(p, false);
                }
                return (Surface)o;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        static internal Surface LookupSurface(IntPtr surface)
        {
            SurfaceType st = SurfaceType.BeOS;// = NativeMethods.cairo_surface_get_type(surface);
            switch (st)
            {
                /*case SurfaceType.Image:
                    return new ImageSurface(surface, true);
                case SurfaceType.Xlib:
                    return new XlibSurface(surface, true);
                case SurfaceType.Xcb:
                    return new XcbSurface(surface, true);
                case SurfaceType.Glitz:
                    return new GlitzSurface(surface, true);
                case SurfaceType.Win32:
                    return new Win32Surface(surface, true);

                case SurfaceType.Pdf:
                    return new PdfSurface(surface, true);
                case SurfaceType.PS:
                    return new PSSurface(surface, true);
                case SurfaceType.DirectFB:
                    return new DirectFBSurface(surface, true);
                case SurfaceType.Svg:
                    return new SvgSurface(surface, true);
                */
                default:
                    return Surface.LookupExternalSurface(surface);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="stride"></param>
        /// <returns></returns>
        [Obsolete("Use an ImageSurface constructor instead.")]
        public static Pictor.Surface CreateForImage(
                ref byte[] data, Pictor.Format format, int width, int height, int stride)
        {
            IntPtr p = new IntPtr();// = NativeMethods.cairo_image_surface_create_for_data(
                    //data, format, width, height, stride);

            return new Pictor.Surface(p, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [Obsolete("Use an ImageSurface constructor instead.")]
        public static Pictor.Surface CreateForImage(
                Pictor.Format format, int width, int height)
        {
            IntPtr p = new IntPtr();// = NativeMethods.cairo_image_surface_create(
                    //format, width, height);

            return new Pictor.Surface(p, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Pictor.Surface CreateSimilar(
                Pictor.Content content, int width, int height)
        {
            IntPtr p = new IntPtr();// = NativeMethods.cairo_surface_create_similar(
                    //this.Handle, content, width, height);

            return new Pictor.Surface(p, true);
        }

        /// <summary>
        /// 
        /// </summary>
        ~Surface()
        {
            Dispose(false);
        }

        //[Obsolete ("Use Context.SetSource() followed by Context.Paint()")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Show(Context gr, double x, double y)
        {
            //NativeMethods.cairo_set_source_surface(gr.Handle, surface, x, y);
            //NativeMethods.cairo_paint(gr.Handle);
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
            if (surface == IntPtr.Zero)
                return;

            lock (surfaces.SyncRoot)
                surfaces.Remove(surface);

            //NativeMethods.cairo_surface_destroy(surface);
            surface = IntPtr.Zero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Status Finish()
        {
            //NativeMethods.cairo_surface_finish(surface);
            return Status;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Flush()
        {
            //NativeMethods.cairo_surface_flush(surface);
        }

        /// <summary>
        /// 
        /// </summary>
        public void MarkDirty()
        {
            //NativeMethods.cairo_surface_mark_dirty(Handle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        public void MarkDirty(Rectangle rectangle)
        {
            //NativeMethods.cairo_surface_mark_dirty_rectangle(Handle, (int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return surface;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PointD DeviceOffset
        {
            get
            {
                double x = 0.0, y = 0.0;
                //NativeMethods.cairo_surface_get_device_offset(surface, out x, out y);
                return new PointD(x, y);
            }

            set
            {
                //NativeMethods.cairo_surface_set_device_offset(surface, value.X, value.Y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Destroy()
        {
            Dispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetFallbackResolution(double x, double y)
        {
            //NativeMethods.cairo_surface_set_fallback_resolution(surface, x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void WriteToPng(string filename)
        {
            //NativeMethods.cairo_surface_write_to_png(surface, filename);
        }

        /// <summary>
        /// 
        /// </summary>
        [Obsolete("Use Handle instead.")]
        public IntPtr Pointer
        {
            get
            {
                return surface;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Status Status
        {
            get { return Status.InvalidContent; }
            //get { return NativeMethods.cairo_surface_status(surface); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Content Content
        {
            get { return Content.Color; }
            //get { return NativeMethods.cairo_surface_get_content(surface); }
        }

        /// <summary>
        /// 
        /// </summary>
        public SurfaceType SurfaceType
        {
            get { return SurfaceType.BeOS; }
            //get { return NativeMethods.cairo_surface_get_type(surface); }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint ReferenceCount
        {
            get { return 0; }
            //get { return NativeMethods.cairo_surface_get_reference_count(surface); }
        }
    }
}
