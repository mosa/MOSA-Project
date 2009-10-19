/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */


namespace Pictor
{
    //=============================================================scanline_u8
    //
    // Unpacked scanline container class
    //
    // This class is used to transfer Data from a scanline rasterizer 
    // to the rendering buffer. It's organized very simple. The class stores 
    // information of horizontal spans to render it into a Pixel-map buffer. 
    // Each Span has staring X, length, and an array of bytes that determine the 
    // cover-values for each Pixel. 
    // Before using this class you should know the minimal and maximal Pixel 
    // Coordinates of your scanline. The protocol of using is:
    // 1. Reset(MinX, MaxX)
    // 2. AddCell() / AddSpan() - accumulate scanline. 
    //    When forming one scanline the next X coordinate must be always greater
    //    than the last stored one, i.e. it works only with ordered Coordinates.
    // 3. Call Finalize(y) and render the scanline.
    // 3. Call ResetSpans() to Prepare for the new scanline.
    //    
    // 4. Rendering:
    // 
    // Scanline provides an iterator class that allows you to extract
    // the spans and the cover values for each Pixel. Be aware that clipping
    // has not been done yet, so you should perform it yourself.
    // Use scanline_u8::iterator to render spans:
    //-------------------------------------------------------------------------
    //
    // int y = sl.y();                    // Y-coordinate of the scanline
    //
    // ************************************
    // ...Perform vertical clipping here...
    // ************************************
    //
    // scanline_u8::const_iterator Span = sl.Begin();
    // 
    // unsigned char* row = m_rbuf->row(y); // The the address of the beginning 
    //                                      // of the current row
    // 
    // unsigned NumberOfSpans = sl.NumberOfSpans(); // Number of spans. It's guaranteed that
    //                                      // NumberOfSpans is always greater than 0.
    //
    // do
    // {
    //     const scanline_u8::cover_type* covers =
    //         Span->covers;                     // The array of the cover values
    //
    //     int num_pix = Span->len;              // Number of pixels of the Span.
    //                                           // Always greater than 0, still it's
    //                                           // better to use "int" instead of 
    //                                           // "unsigned" because it's more
    //                                           // convenient for clipping
    //     int x = Span->x;
    //
    //     **************************************
    //     ...Perform horizontal clipping here...
    //     ...you have x, covers, and pix_count..
    //     **************************************
    //
    //     unsigned char* dst = row + x;  // Calculate the Start address of the row.
    //                                    // In this case we assume a simple 
    //                                    // grayscale image 1-byte per Pixel.
    //     do
    //     {
    //         *dst++ = *covers++;        // Hypotetical rendering. 
    //     }
    //     while(--num_pix);
    //
    //     ++Span;
    // } 
    // while(--NumberOfSpans);  // NumberOfSpans cannot be 0, so this loop is quite safe
    //------------------------------------------------------------------------
    //
    // The question is: why should we accumulate the whole scanline when we
    // could render just separate spans when they're ready?
    // That's because using the scanline is generally faster. When is consists 
    // of more than one Span the conditions for the processor cash system
    // are better, because switching between two different areas of memory 
    // (that can be very large) occurs less frequently.
    //------------------------------------------------------------------------
    public sealed class UnpackedScanline : IScanline
    {
        private int m_min_x;
        private int m_last_x;
        private int m_y;
        private ArrayPOD<byte> m_covers;
        private ArrayPOD<ScanlineSpan> m_spans;
        private uint m_span_index;
        private uint m_interator_index;

        public ScanlineSpan GetNextScanlineSpan()
        {
            m_interator_index++;
            return m_spans.Array[m_interator_index - 1];
        }

        //--------------------------------------------------------------------
        public UnpackedScanline()
        {
            m_last_x = (0x7FFFFFF0);
            m_covers = new ArrayPOD<byte>(1000);
            m_spans = new ArrayPOD<ScanlineSpan>(1000);
        }

        //--------------------------------------------------------------------
        public void Reset(int min_x, int max_x)
        {
            int max_len = max_x - min_x + 2;
            if (max_len > m_spans.Size)
            {
                m_spans.Resize(max_len);
                m_covers.Resize(max_len);
            }
            m_last_x = 0x7FFFFFF0;
            m_min_x = min_x;
            m_span_index = 0;
        }

        //--------------------------------------------------------------------
        public void AddCell(int x, uint cover)
        {
            x -= m_min_x;
            m_covers.Array[x] = (byte)cover;
            if (x == m_last_x + 1)
            {
                m_spans.Array[m_span_index].len++;
            }
            else
            {
                m_span_index++;
                m_spans.Array[m_span_index].x = x + m_min_x;
                m_spans.Array[m_span_index].len = 1;
                m_spans.Array[m_span_index].cover_index = (uint)x;
            }
            m_last_x = x;
        }

        /*
        //--------------------------------------------------------------------
        unsafe public void add_cells(int x, uint len, byte* covers)
        {
            x -= m_min_x;
            for (uint i = 0; i < len; i++)
            {
                m_covers.Array[x + i] = covers[i];
            }
            if (x == m_last_x + 1)
            {
                m_spans.Array[m_span_index].len += (short)len;
            }
            else
            {
                m_span_index++;
                m_spans.Array[m_span_index].x = x + m_min_x;
                m_spans.Array[m_span_index].len = (short)len;
                m_spans.Array[m_span_index].cover_index = x;
            }
            m_last_x = x + (int)len - 1;
        }
         */

        //--------------------------------------------------------------------
        public void AddSpan(int x, int len, uint cover)
        {
            x -= m_min_x;
            for (uint i = 0; i < len; i++)
            {
                m_covers.Array[x + i] = (byte)cover;
            }

            if (x == m_last_x + 1)
            {
                m_spans.Array[m_span_index].len += (int)len;
            }
            else
            {
                m_span_index++;
                m_spans.Array[m_span_index].x = x + m_min_x;
                m_spans.Array[m_span_index].len = (int)len;
                m_spans.Array[m_span_index].cover_index = (uint)x;
            }
            m_last_x = x + (int)len - 1;
        }

        //--------------------------------------------------------------------
        public void Finalize(int y)
        {
            m_y = y;
        }

        //--------------------------------------------------------------------
        public void ResetSpans()
        {
            m_last_x = 0x7FFFFFF0;
            m_span_index = 0;
        }

        //--------------------------------------------------------------------
        public int y() { return m_y; }
        public uint NumberOfSpans 
        {
            get { return (uint)m_span_index; }
        }
        public ScanlineSpan Begin
        {
            get
            {
                m_interator_index = 1;
                return GetNextScanlineSpan();
            }
        }

        public byte[] GetCovers()
        {
            return m_covers.Array;
        }
    };
}
