
using System;
using System.Runtime.InteropServices;

namespace Pictor
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageSurface : Surface
    {
        /// <summary>
        /// 
        /// </summary>
        protected Format format = Format.Argb32;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="owns"></param>
        internal ImageSurface(IntPtr handle, bool owns)
            : base(handle, owns)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public ImageSurface(Format format, int width, int height)
        {
            surface = IntPtr.Zero;// NativeMethods.cairo_image_surface_create(format, width, height);
            lock (surfaces.SyncRoot)
            {
                surfaces[surface] = this;
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
        public ImageSurface(ref byte[] data, Pictor.Format format, int width, int height, int stride)
        {
            surface = IntPtr.Zero;// NativeMethods.cairo_image_surface_create_for_data(data, format, width, height, stride);
            lock (surfaces.SyncRoot)
            {
                surfaces[surface] = this;
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
        public ImageSurface(IntPtr data, Pictor.Format format, int width, int height, int stride)
        {
            surface = IntPtr.Zero;// NativeMethods.cairo_image_surface_create_for_data(data, format, width, height, stride);
            lock (surfaces.SyncRoot)
            {
                surfaces[surface] = this;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public ImageSurface(string filename)
        {
            surface = IntPtr.Zero;// NativeMethods.cairo_image_surface_create_from_png(filename);
            lock (surfaces.SyncRoot)
            {
                surfaces[surface] = this;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get 
            { 
                return 0;//return NativeMethods.cairo_image_surface_get_width(surface); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get 
            {
                return 0;// NativeMethods.cairo_image_surface_get_height(surface); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Data
        {
            get
            {
                IntPtr ptr = IntPtr.Zero;// NativeMethods.cairo_image_surface_get_data(surface);
                int length = Height * Stride;
                byte[] data = new byte[length];
                Marshal.Copy(ptr, data, 0, length);
                return data;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IntPtr DataPtr
        {
            get
            {
                return IntPtr.Zero; // NativeMethods.cairo_image_surface_get_data(surface);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Format Format
        {
            get 
            {
                return format;
                //return NativeMethods.cairo_image_surface_get_format(surface); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Stride
        {
            get 
            {
                return 0;
                //return NativeMethods.cairo_image_surface_get_stride(surface); 
            }
        }
    }
}