/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
using System;

namespace Pictor
{

    //=============================================================BinaryScanline
    // 
    // This is binary scaline container which supports the interface 
    // used in the rasterizer::render(). See description of agg_scanline_u8 
    // for details.
    // 
    //------------------------------------------------------------------------
    public sealed class BinaryScanline : IScanline
    {
        private int             m_last_x;
        private int             m_y;
        private ArrayPOD<ScanlineSpan> m_spans;
        private uint m_span_index;
        private uint m_interator_index;

        public ScanlineSpan GetNextScanlineSpan()
        {
            m_interator_index++;
            return m_spans.Array[m_interator_index - 1];
        }

        //--------------------------------------------------------------------
        public BinaryScanline()
        {
            m_last_x=(0x7FFFFFF0);
            m_spans = new ArrayPOD<ScanlineSpan>(1000);
            m_span_index = 0;
        }

        //--------------------------------------------------------------------
        public void Reset(int min_x, int max_x)
        {
            int max_len = max_x - min_x + 3;
            if(max_len > m_spans.Size)
            {
                m_spans.Resize(max_len);
            }
            m_last_x   = 0x7FFFFFF0;
            m_span_index = 0;
        }

        //--------------------------------------------------------------------
        public void AddCell(int x, uint cover)
        {
            if(x == m_last_x+1)
            {
                m_spans.Array[m_span_index].len++;
            }
            else
            {
                m_span_index++;
                m_spans.Array[m_span_index].x = (short)x;
                m_spans.Array[m_span_index].len = 1;
            }
            m_last_x = x;
        }

        //--------------------------------------------------------------------
        public void AddSpan(int x, int len, uint cover)
        {
            if(x == m_last_x+1)
            {
                m_spans.Array[m_span_index].len += (short)len;
            }
            else
            {
                m_span_index++;
                m_spans.Array[m_span_index].x = x;
                m_spans.Array[m_span_index].len = (int)len;
            }
            m_last_x = x + len - 1;
        }

        /*
        //--------------------------------------------------------------------
        public void add_cells(int x, uint len, void*)
        {
            AddSpan(x, len, 0);
        }
         */

        //--------------------------------------------------------------------
        public void Finalize(int y) 
        { 
            m_y = y; 
        }

        //--------------------------------------------------------------------
        public void ResetSpans()
        {
            m_last_x    = 0x7FFFFFF0;
            m_span_index = 0;
        }

        //--------------------------------------------------------------------
        public int            y()         { return m_y; }
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
            return null;
        }
    };


        /*
    //===========================================================scanline32_bin
    class scanline32_bin
    {
    public:
        typedef int32 coord_type;

        //--------------------------------------------------------------------
        struct Span
        {
            Span() {}
            Span(coord_type x_, coord_type len_) : x(x_), len(len_) {}

            coord_type x;
            coord_type len;
        };
        typedef pod_bvector<Span, 4> span_array_type;


        //--------------------------------------------------------------------
        class_iterator
        {
        public:
           _iterator(span_array_type& spans) :
                m_spans(spans),
                m_span_idx(0)
            {}

            Span& operator*()  { return m_spans[m_span_idx];  }
            Span* operator->() { return &m_spans[m_span_idx]; }

            void operator ++ () { ++m_span_idx; }

        private:
            span_array_type& m_spans;
            uint               m_span_idx;
        };


        //--------------------------------------------------------------------
        scanline32_bin() : m_max_len(0), m_last_x(0x7FFFFFF0) {}

        //--------------------------------------------------------------------
        void Reset(int MinX, int MaxX)
        {
            m_last_x = 0x7FFFFFF0;
            m_spans.RemoveAll();
        }

        //--------------------------------------------------------------------
        void AddCell(int x, uint)
        {
            if(x == m_last_x+1)
            {
                m_spans.last().len++;
            }
            else
            {
                m_spans.Add(Span(coord_type(x), 1));
            }
            m_last_x = x;
        }

        //--------------------------------------------------------------------
        void AddSpan(int x, uint len, uint)
        {
            if(x == m_last_x+1)
            {
                m_spans.last().len += coord_type(len);
            }
            else
            {
                m_spans.Add(Span(coord_type(x), coord_type(len)));
            }
            m_last_x = x + len - 1;
        }

        //--------------------------------------------------------------------
        void add_cells(int x, uint len, void*)
        {
            AddSpan(x, len, 0);
        }

        //--------------------------------------------------------------------
        void Finalize(int y) 
        { 
            m_y = y; 
        }

        //--------------------------------------------------------------------
        void ResetSpans()
        {
            m_last_x = 0x7FFFFFF0;
            m_spans.RemoveAll();
        }

        //--------------------------------------------------------------------
        int            y()         { return m_y; }
        uint       NumberOfSpans() { return m_spans.Size(); }
       _iterator Begin()     { return_iterator(m_spans); }

    private:
        scanline32_bin(scanline32_bin&);
        scanline32_bin operator = (scanline32_bin&);

        uint        m_max_len;
        int             m_last_x;
        int             m_y;
        span_array_type m_spans;
    };
         */
}
