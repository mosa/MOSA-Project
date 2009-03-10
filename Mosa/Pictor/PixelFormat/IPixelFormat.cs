/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */
using Pictor;

namespace Pictor.PixelFormat
{
    public interface IPixelFormat
    {
        uint Width
        {
            get;
        }
        uint Height
        {
            get;
        }
        int Stride
        {
            get;
        }

        IBlender Blender
        {
            get;
        }

        RGBA_Bytes Pixel(int x, int y);
        unsafe void CopyPixel(int x, int y, byte* c);
        void CopyFrom(RasterBuffer from, int xdst, int ydst, int xsrc, int ysrc, uint len);

        unsafe void MakePixel(byte* p, IColorType c);
        void BlendPixel(int x, int y, RGBA_Bytes c, byte cover);

        // Line stuff
        void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c);
        void CopyVerticalLine(int x, int y, uint len, RGBA_Bytes c);

        void BlendHorizontalLine(int x, int y, int x2, RGBA_Bytes c, byte cover);
        void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover);

        // Color stuff
        unsafe void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors);
        unsafe void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors);

        unsafe void BlendSolidHorizontalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers);
        unsafe void BlendSolidVerticalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers);
        
        unsafe void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover);
        unsafe void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover);

        RasterBuffer RenderingBuffer
        {
            get;
        }

        unsafe byte* RowPointer(int y);
        unsafe byte* PixelPointer(int x, int y);

        uint PixelWidthInBytes
        {
            get;
        }
    };

    public abstract class PixelFormatProxy : IPixelFormat
    {
        protected IPixelFormat m_pixf;

        public PixelFormatProxy(IPixelFormat pixf)
        {
            m_pixf = pixf;
        }

        public virtual void Attach(IPixelFormat pixf)
        {
            m_pixf = pixf;
        }

        public virtual uint Width
        {
            get { return m_pixf.Width; }
        }

        public virtual uint Height
        {
            get { return m_pixf.Height; }
        }

        public virtual int Stride
        {
            get { return m_pixf.Stride; }
        }

        public IBlender Blender
        {
            get
            {
                return m_pixf.Blender;
            }
        }

        public virtual RGBA_Bytes Pixel(int x, int y)
        {
            return m_pixf.Pixel(y, x);
        }

        public unsafe virtual void CopyPixel(int x, int y, byte* c)
        {
            m_pixf.CopyPixel(x, y, c);
        }

        public virtual void CopyFrom(RasterBuffer from, int xdst, int ydst, int xsrc, int ysrc, uint len)
        {
            m_pixf.CopyFrom(from, xdst, ydst, xsrc, ysrc, len);
        }

        public unsafe virtual void MakePixel(byte* p, IColorType c)
        {
            m_pixf.MakePixel(p, c);
        }

        public virtual void BlendPixel(int x, int y, RGBA_Bytes c, byte cover)
        {
            m_pixf.BlendPixel(x, y, c, cover);
        }

        public virtual void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c)
        {
            m_pixf.CopyHorizontalLine(x, y, len, c);
        }

        public virtual void CopyVerticalLine(int x, int y, uint len, RGBA_Bytes c)
        {
            m_pixf.CopyVerticalLine(x, y, len, c);
        }

        public virtual void BlendHorizontalLine(int x1, int y, int x2, RGBA_Bytes c, byte cover)
        {
            m_pixf.BlendHorizontalLine(x1, y, x2, c, cover);
        }

        public virtual void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover)
        {
            m_pixf.BlendVerticalLine(x, y1, y2, c, cover);
        }

        public unsafe virtual void BlendSolidHorizontalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
        {
            m_pixf.BlendSolidHorizontalSpan(x, y, len, c, covers);
        }

        public unsafe virtual void BlendSolidVerticalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
        {
            m_pixf.BlendSolidVerticalSpan(x, y, len, c, covers);
        }

        public unsafe virtual void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            m_pixf.CopyHorizontalColorSpan(x, y, len, colors);
        }

        public unsafe virtual void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            m_pixf.CopyVerticalColorSpan(x, y, len, colors);
        }

        public unsafe virtual void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            m_pixf.BlendHorizontalColorSpan(x, y, len, colors, covers, cover);
        }

        public unsafe virtual void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            m_pixf.BlendVerticalColorSpan(x, y, len, colors, covers, cover);
        }

        public virtual RasterBuffer RenderingBuffer
        {
            get
            {
                return m_pixf.RenderingBuffer;
            }
        }

        public unsafe byte* RowPointer(int y)
        {
            return m_pixf.RowPointer(y);
        }

        public unsafe virtual byte* PixelPointer(int x, int y)
        {
            return m_pixf.PixelPointer(x, y);
        }

        public virtual uint PixelWidthInBytes
        {
            get
            {
                return m_pixf.PixelWidthInBytes;
            }
        }
    };
}
