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
	public interface IRasterBuffer
	{
		uint BitsPerPixel
		{
			get;
		}

		unsafe byte* GetBuffer();

		uint Width();
		uint Height();
		int StrideInBytes();

		uint StrideInBytesAbs();

		unsafe byte* GetPixelPointer(int x, int y);

		unsafe byte* GetPixelPointer(int y);

		void CopyFrom(IRasterBuffer src);
	};

	public sealed class RasterBuffer : IRasterBuffer
	{
		unsafe byte* m_BufferPointer;    // Pointer to rendering buffer Data
		unsafe byte* m_FirstPixelPointer;  // Pointer to first Pixel depending on Stride 
		uint m_Width;  // Width in pixels
		uint m_Height; // Height in pixels
		uint m_BitsPerPixel;
		uint m_BytesPerPixel;
		int m_StrideInBytes; // Number of bytes per row. Can be < 0

		//-------------------------------------------------------------------
		public RasterBuffer()
		{
		}

		//--------------------------------------------------------------------
		public unsafe RasterBuffer(byte* buf, uint width, uint height, int stride, uint BytesPerPixel)
		{
			Attach(buf, width, height, stride, BytesPerPixel);
		}

		public uint BitsPerPixel
		{
			get
			{
				return m_BitsPerPixel;
			}
		}

		//--------------------------------------------------------------------
		public unsafe void Attach(byte* buf, uint width, uint height, int stride, uint BitsPerPixel)
		{
			m_BufferPointer = m_FirstPixelPointer = buf;
			m_Width = width;
			m_Height = height;
			m_StrideInBytes = stride;
			m_BitsPerPixel = BitsPerPixel;
			m_BytesPerPixel = BitsPerPixel / 8;
			if (stride < 0)
			{
				int addAmount = -((int)((int)height - 1) * stride);
				m_FirstPixelPointer = &m_BufferPointer[addAmount];
			}
		}

		public void dettachBuffer()
		{
			unsafe
			{
				m_BitsPerPixel = 0;
				m_BufferPointer = m_FirstPixelPointer = null;
				m_Width = m_Height = 0;
				m_StrideInBytes = 0;
			}
		}

		//--------------------------------------------------------------------
		public unsafe byte* GetBuffer() { return m_BufferPointer; }

		public uint Width() { return m_Width; }
		public uint Height() { return m_Height; }
		public int StrideInBytes() { return m_StrideInBytes; }

		public uint StrideInBytesAbs()
		{
			return (m_StrideInBytes < 0) ? (uint)(-m_StrideInBytes) : (uint)(m_StrideInBytes);
		}

		public unsafe byte* GetPixelPointer(int x, int y)
		{
			return &m_FirstPixelPointer[y * m_StrideInBytes + x * m_BytesPerPixel];
		}

		public unsafe byte* GetPixelPointer(int y)
		{
			return &m_FirstPixelPointer[y * m_StrideInBytes];
		}

		public void CopyFrom(IRasterBuffer src)
		{
			uint h = Height();
			if (src.Height() < h) h = src.Height();

			uint StrideABS = StrideInBytesAbs();
			if (src.StrideInBytesAbs() < StrideABS)
			{
				StrideABS = src.StrideInBytesAbs();
			}

			uint y;
			unsafe
			{
				for (y = 0; y < h; y++)
				{
					Basics.memcpy(GetPixelPointer((int)y), src.GetPixelPointer((int)y), (int)StrideABS);
				}
			}
		}
	};
}