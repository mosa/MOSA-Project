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
	//=============================================================scanline_p8
	// 
	// This is a general purpose scanline container which supports the interface 
	// used in the rasterizer::render(). See description of scanline_u8
	// for details.
	// 
	//------------------------------------------------------------------------
	public sealed class Scanline : IScanline
	{
		//typedef scanline_p8 self_type;
		//typedef byte       cover_type;
		//typedef short       coord_type;

		private int m_last_x;
		private int m_y;
		private ArrayPOD<byte> m_covers;
		private uint m_cover_index;
		private ArrayPOD<ScanlineSpan> m_spans;
		private uint m_span_index;
		private uint m_interator_index;

		public ScanlineSpan GetNextScanlineSpan()
		{
			m_interator_index++;
			unsafe
			{
				return m_spans.Array[m_interator_index - 1];
			}
		}

		public Scanline()
		{
			m_last_x = 0x7FFFFFF0;
			m_covers = new ArrayPOD<byte>(1000);
			//m_cover_ptr = null;
			m_spans = new ArrayPOD<ScanlineSpan>(1000);
			//m_cur_span = null;
		}

		//--------------------------------------------------------------------
		public void Reset(int min_x, int max_x)
		{
			int max_len = max_x - min_x + 3;
			if (max_len > m_spans.Size)
			{
				m_spans.Resize(max_len);
				m_covers.Resize(max_len);
			}
			m_last_x = 0x7FFFFFF0;
			m_cover_index = 0;
			m_span_index = 0;
			m_spans.Array[m_span_index].len = 0;
		}

		//--------------------------------------------------------------------
		public void AddCell(int x, uint cover)
		{
			m_covers.Array[m_cover_index] = (byte)cover;
			if (x == m_last_x + 1 && m_spans.Array[m_span_index].len > 0)
			{
				m_spans.Array[m_span_index].len++;
			}
			else
			{
				m_span_index++;
				m_spans.Array[m_span_index].cover_index = m_cover_index;
				m_spans.Array[m_span_index].x = (short)x;
				m_spans.Array[m_span_index].len = 1;
			}
			m_last_x = x;
			m_cover_index++;
		}

		//--------------------------------------------------------------------
		unsafe public void add_cells(int x, int len, byte* covers)
		{
			for (uint i = 0; i < len; i++)
			{
				m_covers.Array[m_cover_index + i] = covers[i];
			}

			if (x == m_last_x + 1 && m_spans.Array[m_span_index].len > 0)
			{
				m_spans.Array[m_span_index].len += (short)len;
			}
			else
			{
				m_span_index++;
				m_spans.Array[m_span_index].cover_index = m_cover_index;
				m_spans.Array[m_span_index].x = (short)x;
				m_spans.Array[m_span_index].len = (short)len;
			}

			m_cover_index += (uint)len;
			m_last_x = x + (int)len - 1;
		}

		//--------------------------------------------------------------------
		public void AddSpan(int x, int len, uint cover)
		{
			if (x == m_last_x + 1
				&& m_spans.Array[m_span_index].len < 0
				&& cover == m_spans.Array[m_span_index].cover_index)
			{
				m_spans.Array[m_span_index].len -= (short)len;
			}
			else
			{
				m_covers.Array[m_cover_index] = (byte)cover;
				m_span_index++;
				m_spans.Array[m_span_index].cover_index = m_cover_index++;
				m_spans.Array[m_span_index].x = (short)x;
				m_spans.Array[m_span_index].len = (short)(-(int)(len));
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
			m_cover_index = 0;
			m_span_index = 0;
			m_spans.Array[m_span_index].len = 0;
		}

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

