/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */
using System;

namespace Pictor.PixelFormat
{
    ///<summary>
    ///</summary>
    public sealed class AlphaMaskAdaptor : PixelFormatProxy
    {
        IAlphaMask _alphaMask;
        readonly ArrayPOD<byte> _span;

        enum ESpanExtraTail { SpanExtraTail = 256 };
        const byte CoverFull = 255;

        void ReallocateSpan(int len)
        {
            if(len > _span.Size)
            {
                _span.Resize(len + (int)ESpanExtraTail.SpanExtraTail);
            }
        }

        void InitSpan(int len)
        {
            InitSpan(len, CoverFull);
        }

        void InitSpan(int len, byte cover)
        {
            ReallocateSpan(len);
            unsafe
            {
                fixed (byte* pBuffer = _span.Array)
                {
                    Basics.memset(pBuffer, cover, len);
                }
            }
        }

        unsafe void InitSpan(int len, byte* covers)
        {
            ReallocateSpan(len);
            byte[] array = _span.Array;
            for (int i = 0; i < len; i++)
            {
                array[i] = *covers++;
            }
        }


        ///<summary>
        ///</summary>
        ///<param name="pixf"></param>
        ///<param name="mask"></param>
        public AlphaMaskAdaptor(IPixelFormat pixf, IAlphaMask mask)
            : base(pixf)
        {
            PixelFormat = pixf;
            _alphaMask = mask;
            _span = new ArrayPOD<byte>(255);
        }

        ///<summary>
        ///</summary>
        ///<param name="pixf"></param>
        public void AttachPixelFormat(IPixelFormat pixf)
        {
            PixelFormat = pixf; 
        }
        ///<summary>
        ///</summary>
        ///<param name="mask"></param>
        public void AttachAlphaMask(IAlphaMask mask) 
        {
            _alphaMask = mask; 
        }

        //--------------------------------------------------------------------
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="c"></param>
        public void CopyPixel(int x, int y, RGBA_Bytes c)
        {
            PixelFormat.BlendPixel(x, y, c, _alphaMask.Pixel(x, y));
        }

        //--------------------------------------------------------------------
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="c"></param>
        ///<param name="cover"></param>
        public override void BlendPixel(int x, int y, RGBA_Bytes c, byte cover)
        {
            PixelFormat.BlendPixel(x, y, c, _alphaMask.CombinePixel(x, y, cover));
        }

        //--------------------------------------------------------------------
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="c"></param>
        public override void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c)
        {
            ReallocateSpan((int)len);
            unsafe
            {
                fixed (byte* pBuffer = _span.Array)
                {
                    _alphaMask.FillHorizontalSpan(x, y, pBuffer, (int)len);
                    PixelFormat.BlendSolidHorizontalSpan(x, y, len, c, pBuffer);
                }
            }
        }

        //--------------------------------------------------------------------
        ///<summary>
        ///</summary>
        ///<param name="x1"></param>
        ///<param name="y"></param>
        ///<param name="x2"></param>
        ///<param name="c"></param>
        ///<param name="cover"></param>
        public override void BlendHorizontalLine(int x1, int y, int x2, RGBA_Bytes c, byte cover)
        {
            int len = x2 - x1 + 1;
            if (cover == CoverFull)
            {
                ReallocateSpan(len);
                unsafe
                {
                    fixed (byte* pBuffer = _span.Array)
                    {
                        _alphaMask.CombineHorizontalSpanFullCover(x1, y, pBuffer, len);
                        PixelFormat.BlendSolidHorizontalSpan(x1, y, (uint)len, c, pBuffer);
                    }
                }
            }
            else
            {
                InitSpan(len, cover);
                unsafe
                {
                    fixed (byte* pBuffer = _span.Array)
                    {
                        _alphaMask.CombineHorizontalSpan(x1, y, pBuffer, len);
                        PixelFormat.BlendSolidHorizontalSpan(x1, y, (uint)len, c, pBuffer);
                    }
                }
            }
        }

        //--------------------------------------------------------------------
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="c"></param>
        public override void CopyVerticalLine(int x, int y, uint len, RGBA_Bytes c)
        {
            ReallocateSpan((int)len);
            unsafe
            {
                fixed (byte* pBuffer = _span.Array)
                {
                    _alphaMask.FillVerticalSpan(x, y, pBuffer, (int)len);
                    PixelFormat.BlendSolidVerticalSpan(x, y, len, c, pBuffer);
                }
            }
        }

        //--------------------------------------------------------------------
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y1"></param>
        ///<param name="y2"></param>
        ///<param name="c"></param>
        ///<param name="cover"></param>
        ///<exception cref="NotImplementedException"></exception>
        public override void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover)
        {
            int len = y2 - y1 + 1;
            InitSpan(len, cover);
            unsafe
            {
                fixed (byte* pBuffer = _span.Array)
                {
                    _alphaMask.CombineVerticalSpan(x, y1, pBuffer, len);
                    throw new System.NotImplementedException("BlendSolidVerticalSpan does not take a y2 yet");
                    //PixelFormat.BlendSolidVerticalSpan(x, y1, y2, c, pBuffer);
                }
            }
        }

        //--------------------------------------------------------------------
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="c"></param>
        ///<param name="covers"></param>
        public override unsafe void BlendSolidHorizontalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
        {
            InitSpan((int)len, covers);
            fixed (byte* pBuffer = _span.Array)
            {
                _alphaMask.CombineHorizontalSpan(x, y, pBuffer, (int)len);
                PixelFormat.BlendSolidHorizontalSpan(x, y, len, c, pBuffer);
                _alphaMask.CombineHorizontalSpan(x, y, covers, (int)len);
                PixelFormat.BlendSolidHorizontalSpan(x, y, len, c, covers);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="c"></param>
        ///<param name="covers"></param>
        public override unsafe void BlendSolidVerticalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
        {
            InitSpan((int)len, covers);
            fixed (byte* pBuffer = _span.Array)
            {
                _alphaMask.CombineVerticalSpan(x, y, pBuffer, (int)len);
                PixelFormat.BlendSolidVerticalSpan(x, y, len, c, pBuffer);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="colors"></param>
        public override unsafe void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            ReallocateSpan((int)len);
            fixed (byte* pBuffer = _span.Array)
            {
                _alphaMask.FillHorizontalSpan(x, y, pBuffer, (int)len);
                PixelFormat.BlendHorizontalColorSpan(x, y, len, colors, pBuffer, CoverFull);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="colors"></param>
        public override unsafe void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            ReallocateSpan((int)len);
            fixed (byte* pBuffer = _span.Array)
            {
                _alphaMask.FillVerticalSpan(x, y, pBuffer, (int)len);
                PixelFormat.BlendVerticalColorSpan(x, y, len, colors, pBuffer, CoverFull);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="colors"></param>
        ///<param name="covers"></param>
        ///<param name="cover"></param>
        public override unsafe void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            fixed (byte* pBuffer = _span.Array)
            {
                if (covers != null)
                {
                    InitSpan((int)len, covers);
                    _alphaMask.CombineHorizontalSpan(x, y, pBuffer, (int)len);
                }
                else
                {
                    ReallocateSpan((int)len);
                    _alphaMask.FillHorizontalSpan(x, y, pBuffer, (int)len);
                }
                PixelFormat.BlendHorizontalColorSpan(x, y, len, colors, pBuffer, cover);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="len"></param>
        ///<param name="colors"></param>
        ///<param name="covers"></param>
        ///<param name="cover"></param>
        public override unsafe void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            fixed (byte* pBuffer = _span.Array)
            {
                if (covers != null)
                {
                    InitSpan((int)len, covers);
                    _alphaMask.CombineVerticalSpan(x, y, pBuffer, (int)len);
                }
                else
                {
                    ReallocateSpan((int)len);
                    _alphaMask.FillVerticalSpan(x, y, pBuffer, (int)len);
                }
                PixelFormat.BlendVerticalColorSpan(x, y, len, colors, pBuffer, cover);
            }
        }
    };
}
