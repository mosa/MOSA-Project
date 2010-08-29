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
	//----------------------------------------------------------SpanAllocator
	public class SpanAllocator
	{
		private ArrayPOD<RGBA_Bytes> m_span;

		public SpanAllocator()
		{
			m_span = new ArrayPOD<RGBA_Bytes>(255);
		}

		//--------------------------------------------------------------------
		public ArrayPOD<RGBA_Bytes> Allocate(uint span_len)
		{
			if (span_len > m_span.Size)
			{
				// To reduce the number of reallocs we align the 
				// span_len to 256 Color elements. 
				// Well, I just like this number and it looks reasonable.
				//-----------------------
				m_span.Resize((((int)span_len + 255) >> 8) << 8);
			}
			return m_span;
		}

		public ArrayPOD<RGBA_Bytes> Span
		{
			get { return m_span; }
		}
		public uint MaximalSpanLength
		{
			get { return m_span.Size; }
		}
	};
}
