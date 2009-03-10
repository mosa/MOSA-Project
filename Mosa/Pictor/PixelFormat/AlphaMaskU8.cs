/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */
using System;

namespace Pictor
{
    public interface IAlphaMask
    {
        byte Pixel(int x, int y);
        byte CombinePixel(int x, int y, byte val);
        unsafe void FillHorizontalSpan(int x, int y, byte* dst, int num_pix);
        unsafe void FillVerticalSpan(int x, int y, byte* dst, int num_pix);
        unsafe void CombineHorizontalSpanFullCover(int x, int y, byte* dst, int num_pix);
        unsafe void CombineHorizontalSpan(int x, int y, byte* dst, int num_pix);
        unsafe void CombineVerticalSpan(int x, int y, byte* dst, int num_pix);
    };

    public sealed class AlphaMaskByteUnclipped : IAlphaMask
    {
        RasterBuffer m_rbuf;
        uint m_Step;
        uint m_Offset;

        public static int cover_shift = 8;
        public static int cover_none = 0;
        public static int cover_full = 255;

        public AlphaMaskByteUnclipped(RasterBuffer rbuf, uint Step, uint Offset)
        {
            m_Step = Step;
            m_Offset = Offset;
            m_rbuf = rbuf;
        }

        public void Attach(RasterBuffer rbuf) { m_rbuf = rbuf; }

        //--------------------------------------------------------------------
        public byte Pixel(int x, int y)
        {
            unsafe
            {
                return *(m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset);
            }
        }

        //--------------------------------------------------------------------
        public byte CombinePixel(int x, int y, byte val)
        {
            unsafe
            {
                return (byte)((cover_full + val * *(m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset)) >> cover_shift);
            }
        }


        //--------------------------------------------------------------------
        public unsafe void FillHorizontalSpan(int x, int y, byte* dst, int num_pix)
        {
            byte* mask = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
            do
            {
                *dst++ = *mask;
                mask += m_Step;
            }
            while (--num_pix != 0);
        }

        public unsafe void CombineHorizontalSpanFullCover(int x, int y, byte* dst, int num_pix)
        {
            byte* mask = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
            do
            {
                *dst = *mask;
                ++dst;
                mask += m_Step;
            }
            while (--num_pix != 0);
        }

        //--------------------------------------------------------------------
        public unsafe void CombineHorizontalSpan(int x, int y, byte* dst, int num_pix)
        {
            byte* mask = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
            do
            {
                *dst = (byte)((cover_full + (*dst) * (*mask)) >> cover_shift);
                ++dst;
                mask += m_Step;
            }
            while (--num_pix != 0);
        }


        //--------------------------------------------------------------------
        public unsafe void FillVerticalSpan(int x, int y, byte* dst, int num_pix)
        {
            byte* mask = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
            do
            {
                *dst++ = *mask;
                mask += m_rbuf.StrideInBytes();
            }
            while (--num_pix != 0);
        }


        //--------------------------------------------------------------------
        public unsafe void CombineVerticalSpan(int x, int y, byte* dst, int num_pix)
        {
            byte* mask = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
            do
            {
                *dst = (byte)((cover_full + (*dst) * (*mask)) >> cover_shift);
                ++dst;
                mask += m_rbuf.StrideInBytes();
            }
            while (--num_pix != 0);
        }
    };

    public sealed class AlphaMaskByteClipped : IAlphaMask
    {
        RasterBuffer m_rbuf;
        uint m_Step;
        uint m_Offset;

        public static int cover_shift = 8;
        public static int cover_none = 0;
        public static int cover_full = 255;

        public AlphaMaskByteClipped(RasterBuffer rbuf, uint Step, uint Offset)
        {
            m_Step = Step;
            m_Offset = Offset;
            m_rbuf = rbuf;
        }

        public void Attach(RasterBuffer rbuf) { m_rbuf = rbuf; }


        //--------------------------------------------------------------------
        public byte Pixel(int x, int y)
        {
            if (x >= 0 && y >= 0 &&
               x < (int)m_rbuf.Width() &&
               y < (int)m_rbuf.Height())
            {
                unsafe
                {
                    return *(m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset);
                }
            }
            return 0;
        }

        //--------------------------------------------------------------------
        public byte CombinePixel(int x, int y, byte val)
        {
            if (x >= 0 && y >= 0 &&
               x < (int)m_rbuf.Width() &&
               y < (int)m_rbuf.Height())
            {
                unsafe
                {
                    return (byte)((cover_full + val * *(m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset)) >> cover_shift);
                }
            }
            return 0;
        }

        //--------------------------------------------------------------------
        public unsafe void FillHorizontalSpan(int x, int y, byte* dst, int num_pix)
        {
            int xmax = (int)m_rbuf.Width() - 1;
            int ymax = (int)m_rbuf.Height() - 1;

            int count = num_pix;
            byte* covers = dst;

            if (y < 0 || y > ymax)
            {
                Basics.MemClear(dst, num_pix);
                return;
            }

            if (x < 0)
            {
                count += x;
                if (count <= 0)
                {
                    Basics.MemClear(dst, num_pix);
                    return;
                }
                Basics.MemClear(covers, -x);
                covers -= x;
                x = 0;
            }

            if (x + count > xmax)
            {
                int rest = x + count - xmax - 1;
                count -= rest;
                if (count <= 0)
                {
                    Basics.MemClear(dst, num_pix);
                    return;
                }
                Basics.MemClear(covers + count, rest);
            }

            byte* mask = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
            do
            {
                *covers++ = *(mask);
                mask += m_Step;
            }
            while (--count != 0);
        }


        public unsafe void CombineHorizontalSpanFullCover(int x, int y, byte* dst, int num_pix)
        {
            int xmax = (int)m_rbuf.Width() - 1;
            int ymax = (int)m_rbuf.Height() - 1;

            int count = num_pix;
            byte* covers = dst;

            if (y < 0 || y > ymax)
            {
                Basics.MemClear(dst, num_pix);
                return;
            }

            if (x < 0)
            {
                count += x;
                if (count <= 0)
                {
                    Basics.MemClear(dst, num_pix);
                    return;
                }
                Basics.MemClear(covers, -x);
                covers -= x;
                x = 0;
            }

            if (x + count > xmax)
            {
                int rest = x + count - xmax - 1;
                count -= rest;
                if (count <= 0)
                {
                    Basics.MemClear(dst, num_pix);
                    return;
                }
                Basics.MemClear(covers + count, rest);
            }

            byte* mask = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
            do
            {
                *covers = *mask;
                ++covers;
                mask += m_Step;
            }
            while (--count != 0);
        }

        //--------------------------------------------------------------------
        public unsafe void CombineHorizontalSpan(int x, int y, byte* dst, int num_pix)
        {
            int xmax = (int)m_rbuf.Width() - 1;
            int ymax = (int)m_rbuf.Height() - 1;

            int count = num_pix;
            byte* covers = dst;

            if (y < 0 || y > ymax)
            {
                Basics.MemClear(dst, num_pix);
                return;
            }

            if (x < 0)
            {
                count += x;
                if (count <= 0)
                {
                    Basics.MemClear(dst, num_pix);
                    return;
                }
                Basics.MemClear(covers, -x);
                covers -= x;
                x = 0;
            }

            if (x + count > xmax)
            {
                int rest = x + count - xmax - 1;
                count -= rest;
                if (count <= 0)
                {
                    Basics.MemClear(dst, num_pix);
                    return;
                }
                Basics.MemClear(covers + count, rest);
            }

            byte* mask = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
            do
            {
                *covers = (byte)((cover_full + (*covers) *
                                       *mask) >>
                                       cover_shift);
                ++covers;
                mask += m_Step;
            }
            while (--count != 0);
        }

        //--------------------------------------------------------------------
        public unsafe void FillVerticalSpan(int x, int y, byte* dst, int num_pix)
        {
            int xmax = (int)m_rbuf.Width() - 1;
            int ymax = (int)m_rbuf.Height() - 1;

            int count = num_pix;
            byte* covers = dst;

            if (x < 0 || x > xmax)
            {
                Basics.MemClear(dst, num_pix);
                return;
            }

            if (y < 0)
            {
                count += y;
                if (count <= 0)
                {
                    Basics.MemClear(dst, num_pix);
                    return;
                }
                Basics.MemClear(covers, -y);
                covers -= y;
                y = 0;
            }

            if (y + count > ymax)
            {
                int rest = y + count - ymax - 1;
                count -= rest;
                if (count <= 0)
                {
                    Basics.MemClear(dst, num_pix);
                    return;
                }
                Basics.MemClear(covers + count, rest);
            }

            byte* mask = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
            do
            {
                *covers++ = *mask;
                mask += m_rbuf.StrideInBytes();
            }
            while (--count != 0);
        }

        //--------------------------------------------------------------------
        public unsafe void CombineVerticalSpan(int x, int y, byte* dst, int num_pix)
        {
            int xmax = (int)m_rbuf.Width() - 1;
            int ymax = (int)m_rbuf.Height() - 1;

            int count = num_pix;
            byte* covers = dst;

            if (x < 0 || x > xmax)
            {
                Basics.MemClear(dst, num_pix);
                return;
            }

            if (y < 0)
            {
                count += y;
                if (count <= 0)
                {
                    Basics.MemClear(dst, num_pix);
                    return;
                }
                Basics.MemClear(covers, -y);
                covers -= y;
                y = 0;
            }

            if (y + count > ymax)
            {
                int rest = y + count - ymax - 1;
                count -= rest;
                if (count <= 0)
                {
                    Basics.MemClear(dst, num_pix);
                    return;
                }
                Basics.MemClear(covers + count, rest);
            }

            byte* mask = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
            do
            {
                *covers = (byte)((cover_full + (*covers) * (*mask)) >> cover_shift);
                ++covers;
                mask += m_rbuf.StrideInBytes();
            }
            while (--count != 0);
        }
    };
}