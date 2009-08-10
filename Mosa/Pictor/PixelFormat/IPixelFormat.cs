/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */
namespace Pictor.PixelFormat
{
    public interface IPixelFormat
    {
        ///<summary>
        ///</summary>
        uint Width
        {
            get;
        }
        ///<summary>
        ///</summary>
        uint Height
        {
            get;
        }
        ///<summary>
        ///</summary>
        int Stride
        {
            get;
        }

        ///<summary>
        ///</summary>
        IBlender Blender
        {
            get;
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<returns></returns>
        RGBA_Bytes Pixel(int x, int y);
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="c"></param>
        unsafe void CopyPixel(int x, int y, byte* c);
        ///<summary>
        ///</summary>
        ///<param name="from"></param>
        ///<param name="xdst"></param>
        ///<param name="ydst"></param>
        ///<param name="xsrc"></param>
        ///<param name="ysrc"></param>
        ///<param name="len"></param>
        void CopyFrom(RasterBuffer from, int xdst, int ydst, int xsrc, int ysrc, uint len);

        ///<summary>
        ///</summary>
        ///<param name="p"></param>
        ///<param name="c"></param>
        unsafe void MakePixel(byte* p, IColorType c);
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="c"></param>
        ///<param name="cover"></param>
        void BlendPixel(int x, int y, RGBA_Bytes c, byte cover);

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="c"></param>
        void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c);
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="c"></param>
        void CopyVerticalLine(int x, int y, uint len, RGBA_Bytes c);

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="x2"></param>
        ///<param name="c"></param>
        ///<param name="cover"></param>
        void BlendHorizontalLine(int x, int y, int x2, RGBA_Bytes c, byte cover);
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y1"></param>
        ///<param name="y2"></param>
        ///<param name="c"></param>
        ///<param name="cover"></param>
        void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover);

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="colors"></param>
        unsafe void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors);
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="colors"></param>
        unsafe void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors);

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="c"></param>
        ///<param name="covers"></param>
        unsafe void BlendSolidHorizontalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers);
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="c"></param>
        ///<param name="covers"></param>
        unsafe void BlendSolidVerticalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers);
        
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="colors"></param>
        ///<param name="covers"></param>
        ///<param name="cover"></param>
        unsafe void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover);
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="colors"></param>
        ///<param name="covers"></param>
        ///<param name="cover"></param>
        unsafe void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover);

        ///<summary>
        ///</summary>
        RasterBuffer RenderingBuffer
        {
            get;
        }

        ///<summary>
        ///</summary>
        ///<param name="y"></param>
        ///<returns></returns>
        unsafe byte* RowPointer(int y);
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<returns></returns>
        unsafe byte* PixelPointer(int x, int y);

        ///<summary>
        ///</summary>
        uint PixelWidthInBytes
        {
            get;
        }
    };

    ///<summary>
    ///</summary>
    public abstract class PixelFormatProxy : IPixelFormat
    {
        protected IPixelFormat PixelFormat;

        ///<summary>
        ///</summary>
        ///<param name="pixf"></param>
        public PixelFormatProxy(IPixelFormat pixf)
        {
            PixelFormat = pixf;
        }

        ///<summary>
        ///</summary>
        ///<param name="pixf"></param>
        public virtual void Attach(IPixelFormat pixf)
        {
            PixelFormat = pixf;
        }

        public virtual uint Width
        {
            get { return PixelFormat.Width; }
        }

        public virtual uint Height
        {
            get { return PixelFormat.Height; }
        }

        public virtual int Stride
        {
            get { return PixelFormat.Stride; }
        }

        public IBlender Blender
        {
            get
            {
                return PixelFormat.Blender;
            }
        }

        public virtual RGBA_Bytes Pixel(int x, int y)
        {
            return PixelFormat.Pixel(y, x);
        }

        public unsafe virtual void CopyPixel(int x, int y, byte* c)
        {
            PixelFormat.CopyPixel(x, y, c);
        }

        public virtual void CopyFrom(RasterBuffer from, int xdst, int ydst, int xsrc, int ysrc, uint len)
        {
            PixelFormat.CopyFrom(from, xdst, ydst, xsrc, ysrc, len);
        }

        public unsafe virtual void MakePixel(byte* p, IColorType c)
        {
            PixelFormat.MakePixel(p, c);
        }

        public virtual void BlendPixel(int x, int y, RGBA_Bytes c, byte cover)
        {
            PixelFormat.BlendPixel(x, y, c, cover);
        }

        public virtual void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c)
        {
            PixelFormat.CopyHorizontalLine(x, y, len, c);
        }

        public virtual void CopyVerticalLine(int x, int y, uint len, RGBA_Bytes c)
        {
            PixelFormat.CopyVerticalLine(x, y, len, c);
        }

        public virtual void BlendHorizontalLine(int x1, int y, int x2, RGBA_Bytes c, byte cover)
        {
            PixelFormat.BlendHorizontalLine(x1, y, x2, c, cover);
        }

        public virtual void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover)
        {
            PixelFormat.BlendVerticalLine(x, y1, y2, c, cover);
        }

        public unsafe virtual void BlendSolidHorizontalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
        {
            PixelFormat.BlendSolidHorizontalSpan(x, y, len, c, covers);
        }

        public unsafe virtual void BlendSolidVerticalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
        {
            PixelFormat.BlendSolidVerticalSpan(x, y, len, c, covers);
        }

        public unsafe virtual void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            PixelFormat.CopyHorizontalColorSpan(x, y, len, colors);
        }

        public unsafe virtual void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            PixelFormat.CopyVerticalColorSpan(x, y, len, colors);
        }

        public unsafe virtual void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            PixelFormat.BlendHorizontalColorSpan(x, y, len, colors, covers, cover);
        }

        public unsafe virtual void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            PixelFormat.BlendVerticalColorSpan(x, y, len, colors, covers, cover);
        }

        public virtual RasterBuffer RenderingBuffer
        {
            get
            {
                return PixelFormat.RenderingBuffer;
            }
        }

        public unsafe byte* RowPointer(int y)
        {
            return PixelFormat.RowPointer(y);
        }

        public unsafe virtual byte* PixelPointer(int x, int y)
        {
            return PixelFormat.PixelPointer(x, y);
        }

        public virtual uint PixelWidthInBytes
        {
            get
            {
                return PixelFormat.PixelWidthInBytes;
            }
        }
    };
}
