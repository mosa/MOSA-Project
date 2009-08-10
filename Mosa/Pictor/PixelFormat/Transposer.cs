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
    //=======================================================pixfmt_transposer
    public sealed class FormatTransposer : PixelFormatProxy
    {
        //--------------------------------------------------------------------
        public FormatTransposer(IPixelFormat pixelFormat)
            : base(pixelFormat)
        {
        }


        //--------------------------------------------------------------------
        public override uint Width 
        { 
            get { return PixelFormat.Height; } 
        }
        public override uint Height
        {
            get { return PixelFormat.Width; }
        }

        //--------------------------------------------------------------------
        public override RGBA_Bytes Pixel(int x, int y)
        {
            return PixelFormat.Pixel(y, x);
        }

        //--------------------------------------------------------------------
        unsafe public override void CopyPixel(int x, int y, byte* c)
        {
            PixelFormat.CopyPixel(y, x, c);
        }

        //--------------------------------------------------------------------
        public override void BlendPixel(int x, int y, RGBA_Bytes c, byte cover)
        {
            PixelFormat.BlendPixel(y, x, c, cover);
        }

        //--------------------------------------------------------------------
        public override void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c)
        {
            PixelFormat.CopyVerticalLine(y, x, len, c);
        }

        //--------------------------------------------------------------------
        public override void CopyVerticalLine(int x, int y,
                                   uint len,
                                   RGBA_Bytes c)
        {
            PixelFormat.CopyHorizontalLine(y, x, len, c);
        }

        //--------------------------------------------------------------------
        public override void BlendHorizontalLine(int x1, int y,int x2, RGBA_Bytes c, byte cover)
        {
            PixelFormat.BlendVerticalLine(y, x1, x2, c, cover);
        }

        //--------------------------------------------------------------------
        public override void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover)
        {
            PixelFormat.BlendHorizontalLine(y1, x, y2, c, cover);
        }

        //--------------------------------------------------------------------
        unsafe public override void BlendSolidHorizontalSpan(int x, int y,
                                          uint len,
                                          RGBA_Bytes c,
                                          byte* covers)
        {
            PixelFormat.BlendSolidVerticalSpan(y, x, len, c, covers);
        }

        //--------------------------------------------------------------------
        unsafe public override void BlendSolidVerticalSpan(int x, int y,
                                          uint len,
                                          RGBA_Bytes c,
                                          byte* covers)
        {
            PixelFormat.BlendSolidHorizontalSpan(y, x, len, c, covers);
        }

        public unsafe override void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            PixelFormat.CopyVerticalColorSpan(y, x, len, colors);
        }

        public unsafe override void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            PixelFormat.CopyHorizontalColorSpan(y, x, len, colors);
        }

        //--------------------------------------------------------------------
        unsafe public override void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            PixelFormat.BlendVerticalColorSpan(y, x, len, colors, covers, cover);
        }

        //--------------------------------------------------------------------
        unsafe public override void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            PixelFormat.BlendHorizontalColorSpan(y, x, len, colors, covers, cover);
        }
    };
}
