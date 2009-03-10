/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */
using System;
using Pictor;

namespace Pictor.PixelFormat
{
    //==================================================pixfmt_amask_adaptor
    public sealed class AlphaMaskAdaptor : PixelFormatProxy
    {
        IAlphaMask m_mask;
        ArrayPOD<byte> m_span;

        enum span_extra_tail_e { span_extra_tail = 256 };
        static byte cover_full = 255;

        void ReallocateSpan(int len)
        {
            if(len > m_span.Size)
            {
                m_span.Resize(len + (int)span_extra_tail_e.span_extra_tail);
            }
        }

        void InitSpan(int len)
        {
            InitSpan(len, cover_full);
        }

        void InitSpan(int len, byte cover)
        {
            ReallocateSpan(len);
            unsafe
            {
                fixed (byte* pBuffer = m_span.Array)
                {
                    Basics.memset(pBuffer, cover, (int)len);
                }
            }
        }

        unsafe void InitSpan(int len, byte* covers)
        {
            ReallocateSpan(len);
            unsafe
            {
            	byte[] array = m_span.Array;
                for (int i = 0; i < (int)len; i++)
                {
                    array[i] = *covers++;
                }
            }
        }


        public AlphaMaskAdaptor(IPixelFormat pixf, IAlphaMask mask)
            : base(pixf)
        {
            m_pixf = pixf;
            m_mask = mask;
            m_span = new ArrayPOD<byte>(255);
        }

        public void attach_pixfmt(IPixelFormat pixf)
        {
            m_pixf = pixf; 
        }
        public void attach_alpha_mask(IAlphaMask mask) 
        {
            m_mask = mask; 
        }

        //--------------------------------------------------------------------
        public void copy_pixel(int x, int y, RGBA_Bytes c)
        {
            m_pixf.BlendPixel(x, y, c, m_mask.Pixel(x, y));
        }

        //--------------------------------------------------------------------
        public override void BlendPixel(int x, int y, RGBA_Bytes c, byte cover)
        {
            m_pixf.BlendPixel(x, y, c, m_mask.CombinePixel(x, y, cover));
        }

        //--------------------------------------------------------------------
        public override void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c)
        {
            ReallocateSpan((int)len);
            unsafe
            {
                fixed (byte* pBuffer = m_span.Array)
                {
                    m_mask.FillHorizontalSpan(x, y, pBuffer, (int)len);
                    m_pixf.BlendSolidHorizontalSpan(x, y, len, c, pBuffer);
                }
            }
        }

        //--------------------------------------------------------------------
        public override void BlendHorizontalLine(int x1, int y, int x2, RGBA_Bytes c, byte cover)
        {
            int len = x2 - x1 + 1;
            if (cover == cover_full)
            {
                ReallocateSpan(len);
                unsafe
                {
                    fixed (byte* pBuffer = m_span.Array)
                    {
                        m_mask.CombineHorizontalSpanFullCover(x1, y, pBuffer, (int)len);
                        m_pixf.BlendSolidHorizontalSpan(x1, y, (uint)len, c, pBuffer);
                    }
                }
            }
            else
            {
                InitSpan(len, cover);
                unsafe
                {
                    fixed (byte* pBuffer = m_span.Array)
                    {
                        m_mask.CombineHorizontalSpan(x1, y, pBuffer, (int)len);
                        m_pixf.BlendSolidHorizontalSpan(x1, y, (uint)len, c, pBuffer);
                    }
                }
            }
        }

        //--------------------------------------------------------------------
        public override void CopyVerticalLine(int x, int y, uint len, RGBA_Bytes c)
        {
            ReallocateSpan((int)len);
            unsafe
            {
                fixed (byte* pBuffer = m_span.Array)
                {
                    m_mask.FillVerticalSpan(x, y, pBuffer, (int)len);
                    m_pixf.BlendSolidVerticalSpan(x, y, len, c, pBuffer);
                }
            }
        }

        //--------------------------------------------------------------------
        public override void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover)
        {
            int len = y2 - y1 + 1;
            InitSpan(len, cover);
            unsafe
            {
                fixed (byte* pBuffer = m_span.Array)
                {
                    m_mask.CombineVerticalSpan(x, y1, pBuffer, len);
                    throw new System.NotImplementedException("BlendSolidVerticalSpan does not take a y2 yet");
                    //m_pixf.BlendSolidVerticalSpan(x, y1, y2, c, pBuffer);
                }
            }
        }

        //--------------------------------------------------------------------
        public override unsafe void BlendSolidHorizontalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
        {
            //InitSpan((int)len, covers);
            unsafe
            {
                fixed (byte* pBuffer = m_span.Array)
                {
                    //m_mask.CombineHorizontalSpan(x, y, pBuffer, (int)len);
                    //m_pixf.BlendSolidHorizontalSpan(x, y, len, c, pBuffer);
                    m_mask.CombineHorizontalSpan(x, y, covers, (int)len);
                    m_pixf.BlendSolidHorizontalSpan(x, y, len, c, covers);
                }
            }
        }


        //--------------------------------------------------------------------
        public override unsafe void BlendSolidVerticalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
        {
            InitSpan((int)len, covers);
            unsafe
            {
                fixed (byte* pBuffer = m_span.Array)
                {
                    m_mask.CombineVerticalSpan(x, y, pBuffer, (int)len);
                    m_pixf.BlendSolidVerticalSpan(x, y, len, c, pBuffer);
                }
            }
        }


        //--------------------------------------------------------------------
        public override unsafe void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            ReallocateSpan((int)len);
            unsafe
            {
                fixed (byte* pBuffer = m_span.Array)
                {
                    m_mask.FillHorizontalSpan(x, y, pBuffer, (int)len);
                    m_pixf.BlendHorizontalColorSpan(x, y, len, colors, pBuffer, cover_full);
                }
            }
        }

        //--------------------------------------------------------------------
        public override unsafe void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
        {
            ReallocateSpan((int)len);
            unsafe
            {
                fixed (byte* pBuffer = m_span.Array)
                {
                    m_mask.FillVerticalSpan(x, y, pBuffer, (int)len);
                    m_pixf.BlendVerticalColorSpan(x, y, len, colors, pBuffer, cover_full);
                }
            }
        }

        //--------------------------------------------------------------------
        public override unsafe void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            unsafe
            {
                fixed (byte* pBuffer = m_span.Array)
                {
                    if (covers != null)
                    {
                        InitSpan((int)len, covers);
                        m_mask.CombineHorizontalSpan(x, y, pBuffer, (int)len);
                    }
                    else
                    {
                        ReallocateSpan((int)len);
                        m_mask.FillHorizontalSpan(x, y, pBuffer, (int)len);
                    }
                    m_pixf.BlendHorizontalColorSpan(x, y, len, colors, pBuffer, cover);
                }
            }
        }


        //--------------------------------------------------------------------
        public override unsafe void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
        {
            unsafe
            {
                fixed (byte* pBuffer = m_span.Array)
                {
                    if (covers != null)
                    {
                        InitSpan((int)len, covers);
                        m_mask.CombineVerticalSpan(x, y, pBuffer, (int)len);
                    }
                    else
                    {
                        ReallocateSpan((int)len);
                        m_mask.FillVerticalSpan(x, y, pBuffer, (int)len);
                    }
                    m_pixf.BlendVerticalColorSpan(x, y, len, colors, pBuffer, cover);
                }
            }
        }
    };
}
