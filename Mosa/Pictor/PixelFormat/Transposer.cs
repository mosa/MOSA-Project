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
            get { return m_pixf.Height; } 
        }
        public override uint Height
        {
            get { return m_pixf.Width; }
        }

        //--------------------------------------------------------------------
        public override RGBA_Bytes Pixel(int x, int y)
        {
            return m_pixf.Pixel(y, x);
        }

        //--------------------------------------------------------------------
        unsafe public override void CopyPixel(int x, int y, byte* c)
        {
            m_pixf.CopyPixel(y, x, c);
        }

        //--------------------------------------------------------------------
        public override void BlendPixel(int x, int y, RGBA_Bytes c, byte cover)
        {
            m_pixf.BlendPixel(y, x, c, cover);
        }

        //--------------------------------------------------------------------
        public override void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c)
        {
            m_pixf.CopyVerticalLine(y, x, len, c);
        }

        //--------------------------------------------------------------------
        public override void CopyVerticalLine(int x, int y,
                                   uint len,
                                   RGBA_Bytes c)
        {
            m_pixf.CopyHorizontalLine(y, x, len, c);
        }

        //--------------------------------------------------------------------
        public override void BlendHorizontalLine(int x1, int y,int x2, RGBA_Bytes c, byte cover)
        {
            m_pixf.BlendVerticalLine(y, x1, x2, c, cover);
        }

        //--------------------------------------------------------------------
        public override void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover)
        {
            m_pixf.BlendHorizontalLine(y1, x, y2, c, cover);
        }

        //--------------------------------------------------------------------
        unsafe public override void BlendSolidHorizontalSpan(int x, int y,
                                          uint len,
                                          RGBA_Bytes c,
                                          byte* covers)
        {
            m_pixf.BlendSolidVerticalSpan(y, x, len, c, covers);
        }

        //--------------------------------------------------------------------
        unsafe public override void BlendSolidVerticalSpan(int x, int y,
                                          uint len,
                                          RGBA_Bytes c,
                                          byte* covers)
        {
            m_pixf.BlendSolidHorizontalSpan(y, x, len, c, covers);
        }

        public unsafe override void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            m_pixf.CopyVerticalColorSpan(y, x, len, colors);
        }

        public unsafe override void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            m_pixf.CopyHorizontalColorSpan(y, x, len, colors);
        }

        //--------------------------------------------------------------------
        unsafe public override void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            m_pixf.BlendVerticalColorSpan(y, x, len, colors, covers, cover);
        }

        //--------------------------------------------------------------------
        unsafe public override void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            m_pixf.BlendHorizontalColorSpan(y, x, len, colors, covers, cover);
        }
    };
}
