/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
using System.Runtime.InteropServices;
using Pictor.PixelFormat;

namespace Pictor
{
    public interface IRasterBufferAccessor
    {
        unsafe byte* Span(int x, int y, uint len);
        unsafe byte* NextX();
        unsafe byte* NextY();

        IPixelFormat PixelFormat
        {
            get;
        }
    };

    //-----------------------------------------------------image_accessor_clip
    public sealed class RasterBufferAccessorClip : IRasterBufferAccessor
    {
        private IPixelFormat m_pixf;
        unsafe private byte* m_pBackBufferColor;
        private int m_x, m_x0, m_y;
        unsafe private byte* m_pix_ptr;
        uint m_PixelWidthInBytes;

        public RasterBufferAccessorClip(IPixelFormat pixf, RGBA_Doubles bk)
        {
            m_pixf = pixf;
            m_PixelWidthInBytes = m_pixf.PixelWidthInBytes;
            //pixfmt_alpha_blend_bgra32.MakePixel(m_bk_buf, bk);
            unsafe
            {
                m_pBackBufferColor = (byte*)Marshal.AllocHGlobal(16);
            }
        }

        ~RasterBufferAccessorClip()
        {
            unsafe
            {
                Marshal.FreeHGlobal((System.IntPtr)m_pBackBufferColor);
            }
        }

        public IPixelFormat PixelFormat
        {
            get
            {
                return m_pixf;
            }
        }

        public void Attach(IPixelFormat pixf)
        {
            m_pixf = pixf;
        }

        IPixelFormat GetPixelFormat()
        {
            return m_pixf;
        }

        public void BackgroundColor(IColorType bk)
        {
            unsafe { m_pixf.MakePixel(m_pBackBufferColor, bk); }
        }

        unsafe private byte* Pixel
        {
            get
            {
                if (m_y >= 0 && m_y < (int)m_pixf.Height &&
                   m_x >= 0 && m_x < (int)m_pixf.Width)
                {
                    return m_pixf.PixelPointer(m_x, m_y);
                }

                return m_pBackBufferColor;
            }
        }

        unsafe public byte* Span(int x, int y, uint len)
        {
            m_x = m_x0 = x;
            m_y = y;
            if(y >= 0 && y < (int)m_pixf.Height &&
               x >= 0 && x+(int)len <= (int)m_pixf.Width)
            {
                return m_pix_ptr = m_pixf.PixelPointer(x, y);
            }
            m_pix_ptr = null;
            return Pixel;
        }

        unsafe public byte* NextX()
        {
                if (m_pix_ptr != null)
                {
                    return m_pix_ptr += (int)m_PixelWidthInBytes;
                }

                ++m_x;
                return Pixel;
        }

        unsafe public byte* NextY()
        {
                ++m_y;
                m_x = m_x0;
                if (m_pix_ptr != null
                    && m_y >= 0
                    && m_y < (int)m_pixf.Height)
                {
                    return m_pix_ptr = m_pixf.PixelPointer(m_x, m_y);
                }
                m_pix_ptr = null;
                return Pixel;
        }
    };

    /*
        //--------------------------------------------------image_accessor_no_clip
        template<class PixFmt> class image_accessor_no_clip
        {
        public:
            typedef PixFmt   pixfmt_type;
            typedef typename pixfmt_type::color_type color_type;
            typedef typename pixfmt_type::order_type order_type;
            typedef typename pixfmt_type::value_type value_type;
            enum pix_width_e { pix_width = pixfmt_type::pix_width };

            image_accessor_no_clip() {}
            explicit image_accessor_no_clip(pixfmt_type& pixf) : 
                PixelFormat(&pixf) 
            {}

            void Attach(pixfmt_type& pixf)
            {
                PixelFormat = &pixf;
            }

            byte* Span(int x, int y, uint)
            {
                m_x = x;
                m_y = y;
                return m_pix_ptr = PixelFormat->PixelPointer(x, y);
            }

            byte* NextX()
            {
                return m_pix_ptr += pix_width;
            }

            byte* NextY()
            {
                ++m_y;
                return m_pix_ptr = PixelFormat->PixelPointer(m_x, m_y);
            }

        private:
            pixfmt_type* PixelFormat;
            int                m_x, m_y;
            byte*       m_pix_ptr;
        };
     */

        public sealed class RasterBufferAccessorClamp : IRasterBufferAccessor
        {
            private IPixelFormat m_pixf;
            private int m_x, m_x0, m_y;
            unsafe private byte* m_pix_ptr;
            uint m_PixelWidthInBytes;

            public RasterBufferAccessorClamp(IPixelFormat pixf)
            {
                m_pixf = pixf;
                m_PixelWidthInBytes = m_pixf.PixelWidthInBytes;
            }

            void Attach(IPixelFormat pixf)
            {
                m_pixf = pixf;
            }

            public IPixelFormat PixelFormat
            {
                get
                {
                    return m_pixf;
                }
            }

            private unsafe byte* Pixel
            {
                get
                {
                    int x = m_x;
                    int y = m_y;
                    if (x < 0) x = 0;
                    if (y < 0) y = 0;
                    if (x >= (int)m_pixf.Width)
                    {
                        x = (int)m_pixf.Width - 1;
                    }

                    if (y >= (int)m_pixf.Height)
                    {
                        y = (int)m_pixf.Height - 1;
                    }

                    return m_pixf.PixelPointer(x, y);
                }
            }

            public unsafe byte* Span(int x, int y, uint len)
            {
                m_x = m_x0 = x;
                m_y = y;
                if(y >= 0 && y < (int)m_pixf.Height &&
                   x >= 0 && x+len <= (int)m_pixf.Width)
                {
                    return m_pix_ptr = m_pixf.PixelPointer(x, y);
                }
                m_pix_ptr = null;
                return Pixel;
            }

            public unsafe byte* NextX()
            {
                    if (m_pix_ptr != null)
                    {
                        return m_pix_ptr += m_PixelWidthInBytes;
                    }
                    ++m_x;
                    return Pixel;
            }

            public unsafe byte* NextY()
            {
                    ++m_y;
                    m_x = m_x0;
                    if (m_pix_ptr != null
                        && m_y >= 0
                        && m_y < (int)m_pixf.Height)
                    {
                        return m_pix_ptr = m_pixf.PixelPointer(m_x, m_y);
                    }
                    m_pix_ptr = null;
                    return Pixel;
            }
        };
    /*

        //-----------------------------------------------------image_accessor_wrap
        template<class PixFmt, class WrapX, class WrapY> class image_accessor_wrap
        {
        public:
            typedef PixFmt   pixfmt_type;
            typedef typename pixfmt_type::color_type color_type;
            typedef typename pixfmt_type::order_type order_type;
            typedef typename pixfmt_type::value_type value_type;
            enum pix_width_e { pix_width = pixfmt_type::pix_width };

            image_accessor_wrap() {}
            explicit image_accessor_wrap(pixfmt_type& pixf) : 
                PixelFormat(&pixf), 
                m_wrap_x(pixf.Width()), 
                m_wrap_y(pixf.Height())
            {}

            void Attach(pixfmt_type& pixf)
            {
                PixelFormat = &pixf;
            }

            byte* Span(int x, int y, uint)
            {
                m_x = x;
                m_row_ptr = PixelFormat->RowPointer(m_wrap_y(y));
                return m_row_ptr + m_wrap_x(x) * pix_width;
            }

            byte* NextX()
            {
                int x = ++m_wrap_x;
                return m_row_ptr + x * pix_width;
            }

            byte* NextY()
            {
                m_row_ptr = PixelFormat->RowPointer(++m_wrap_y);
                return m_row_ptr + m_wrap_x(m_x) * pix_width;
            }

        private:
            pixfmt_type* PixelFormat;
            byte*       m_row_ptr;
            int                m_x;
            WrapX              m_wrap_x;
            WrapY              m_wrap_y;
        };




        //--------------------------------------------------------wrap_mode_repeat
        class wrap_mode_repeat
        {
        public:
            wrap_mode_repeat() {}
            wrap_mode_repeat(uint Size) : 
                m_size(Size), 
                m_add(Size * (0x3FFFFFFF / Size)),
                m_value(0)
            {}

            uint operator() (int v)
            { 
                return m_value = (uint(v) + m_add) % m_size; 
            }

            uint operator++ ()
            {
                ++m_value;
                if(m_value >= m_size) m_value = 0;
                return m_value;
            }
        private:
            uint m_size;
            uint m_add;
            uint m_value;
        };


        //---------------------------------------------------wrap_mode_repeat_pow2
        class wrap_mode_repeat_pow2
        {
        public:
            wrap_mode_repeat_pow2() {}
            wrap_mode_repeat_pow2(uint Size) : m_value(0)
            {
                m_mask = 1;
                while(m_mask < Size) m_mask = (m_mask << 1) | 1;
                m_mask >>= 1;
            }
            uint operator() (int v)
            { 
                return m_value = uint(v) & m_mask;
            }
            uint operator++ ()
            {
                ++m_value;
                if(m_value > m_mask) m_value = 0;
                return m_value;
            }
        private:
            uint m_mask;
            uint m_value;
        };


        //----------------------------------------------wrap_mode_repeat_auto_pow2
        class wrap_mode_repeat_auto_pow2
        {
        public:
            wrap_mode_repeat_auto_pow2() {}
            wrap_mode_repeat_auto_pow2(uint Size) :
                m_size(Size),
                m_add(Size * (0x3FFFFFFF / Size)),
                m_mask((m_size & (m_size-1)) ? 0 : m_size-1),
                m_value(0)
            {}

            uint operator() (int v) 
            { 
                if(m_mask) return m_value = uint(v) & m_mask;
                return m_value = (uint(v) + m_add) % m_size;
            }
            uint operator++ ()
            {
                ++m_value;
                if(m_value >= m_size) m_value = 0;
                return m_value;
            }

        private:
            uint m_size;
            uint m_add;
            uint m_mask;
            uint m_value;
        };


        //-------------------------------------------------------wrap_mode_reflect
        class wrap_mode_reflect
        {
        public:
            wrap_mode_reflect() {}
            wrap_mode_reflect(uint Size) : 
                m_size(Size), 
                m_size2(Size * 2),
                m_add(m_size2 * (0x3FFFFFFF / m_size2)),
                m_value(0)
            {}

            uint operator() (int v)
            { 
                m_value = (uint(v) + m_add) % m_size2;
                if(m_value >= m_size) return m_size2 - m_value - 1;
                return m_value;
            }

            uint operator++ ()
            {
                ++m_value;
                if(m_value >= m_size2) m_value = 0;
                if(m_value >= m_size) return m_size2 - m_value - 1;
                return m_value;
            }
        private:
            uint m_size;
            uint m_size2;
            uint m_add;
            uint m_value;
        };



        //--------------------------------------------------wrap_mode_reflect_pow2
        class wrap_mode_reflect_pow2
        {
        public:
            wrap_mode_reflect_pow2() {}
            wrap_mode_reflect_pow2(uint Size) : m_value(0)
            {
                m_mask = 1;
                m_size = 1;
                while(m_mask < Size) 
                {
                    m_mask = (m_mask << 1) | 1;
                    m_size <<= 1;
                }
            }
            uint operator() (int v)
            { 
                m_value = uint(v) & m_mask;
                if(m_value >= m_size) return m_mask - m_value;
                return m_value;
            }
            uint operator++ ()
            {
                ++m_value;
                m_value &= m_mask;
                if(m_value >= m_size) return m_mask - m_value;
                return m_value;
            }
        private:
            uint m_size;
            uint m_mask;
            uint m_value;
        };



        //---------------------------------------------wrap_mode_reflect_auto_pow2
        class wrap_mode_reflect_auto_pow2
        {
        public:
            wrap_mode_reflect_auto_pow2() {}
            wrap_mode_reflect_auto_pow2(uint Size) :
                m_size(Size),
                m_size2(Size * 2),
                m_add(m_size2 * (0x3FFFFFFF / m_size2)),
                m_mask((m_size2 & (m_size2-1)) ? 0 : m_size2-1),
                m_value(0)
            {}

            uint operator() (int v) 
            { 
                m_value = m_mask ? uint(v) & m_mask : 
                                  (uint(v) + m_add) % m_size2;
                if(m_value >= m_size) return m_size2 - m_value - 1;
                return m_value;            
            }
            uint operator++ ()
            {
                ++m_value;
                if(m_value >= m_size2) m_value = 0;
                if(m_value >= m_size) return m_size2 - m_value - 1;
                return m_value;
            }

        private:
            uint m_size;
            uint m_size2;
            uint m_add;
            uint m_mask;
            uint m_value;
        };
     */
}