/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Pictor.PixelFormat
{
	public interface IBlenderGray : IBlender
	{
		unsafe void BlendPixel(byte* p, uint cv, uint alpha);
		unsafe void BlendPixel(byte* p, uint cv, uint alpha, uint cover);
	}

	//============================================================BlenderGray
	//template<class ColorT> 
	public struct BlenderGray : IBlenderGray
	{
		public uint NumPixelBits { get { return 8; } }

		public int OrderR { get { return 0; } }
		public int OrderG { get { return 0; } }
		public int OrderB { get { return 0; } }
		public int OrderA { get { return 0; } }

		const int base_shift = 8;

		public unsafe void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			BlendPixel(p, cr, alpha);
		}
		public unsafe void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			BlendPixel(p, cr, alpha);
		}

		public unsafe void BlendPixel(byte* p, uint cv, uint alpha)
		{
			unchecked
			{
				*p = (byte)((((cv - (uint)(*p)) * alpha) + ((int)(*p) << base_shift)) >> base_shift);
			}
		}

		public unsafe void BlendPixel(byte* p, uint cv, uint alpha, uint cover)
		{
			BlendPixel(p, cv, alpha);
		}
	};

	/*

	//======================================================blender_gray_pre
	//template<class ColorT> 
	struct blender_gray_pre
	{
		typedef ColorT color_type;
		typedef typename color_type::value_type value_type;
		typedef typename color_type::calc_type calc_type;
		enum base_scale_e { BaseShift = color_type::BaseShift };

		static void BlendPixel(value_type* p, uint cv,
										 uint alpha, uint cover)
		{
			alpha = color_type::BaseMask - alpha;
			cover = (cover + 1) << (BaseShift - 8);
			*p = (value_type)((*p * alpha + cv * cover) >> BaseShift);
		}

		static void BlendPixel(value_type* p, uint cv,
										 uint alpha)
		{
			*p = (value_type)(((*p * (color_type::BaseMask - alpha)) >> BaseShift) + cv);
		}
	};
	


	//=====================================================apply_gamma_dir_gray
	//template<class ColorT, class GammaLut> 
	class apply_gamma_dir_gray
	{
	public:
		typedef typename ColorT::value_type value_type;

		apply_gamma_dir_gray(GammaLut& Gamma) : m_gamma(Gamma) {}

		void operator () (byte* p)
		{
			*p = m_gamma.Dir(*p);
		}

	private:
		GammaLut& m_gamma;
	};



	//=====================================================apply_gamma_inv_gray
	//template<class ColorT, class GammaLut> 
	class apply_gamma_inv_gray
	{
	public:
		typedef typename ColorT::value_type value_type;

		apply_gamma_inv_gray(GammaLut& Gamma) : m_gamma(Gamma) {}

		void operator () (byte* p)
		{
			*p = m_gamma.Inv(*p);
		}

	private:
		GammaLut& m_gamma;
	};

	 */


	//=================================================pixfmt_alpha_blend_gray
	//template<class Blender, class RenBuf, uint Step=1, uint Offset=0>
	public sealed class FormatGray : IPixelFormat
	{
		RasterBuffer m_rbuf;
		uint m_Step;
		uint m_Offset;

		IBlenderGray m_Blender;

		const int base_shift = 8;
		const uint base_scale = (uint)(1 << base_shift);
		const uint base_mask = base_scale - 1;

		//--------------------------------------------------------------------
		//--------------------------------------------------------------------
		public FormatGray(RasterBuffer rb, IBlenderGray blender, uint step, uint offset)
		{
			m_rbuf = rb;
			Blender = blender;
			m_Step = step;
			m_Offset = offset;
		}

		public IBlender Blender
		{
			get
			{
				return m_Blender;
			}

			set
			{
				if (value.NumPixelBits != 8)
				{
					throw new System.NotSupportedException("pixfmt_alpha_blend_gray requires your Blender to be 8 bit. Change the Blender or the Pixel Format");
				}
				m_Blender = (IBlenderGray)value;
			}
		}

		public RasterBuffer RenderingBuffer
		{
			get
			{
				return m_rbuf;
			}
		}

		public uint PixelWidthInBytes
		{
			get
			{
				return 1;
			}
		}

		public void attach(RasterBuffer rb) { m_rbuf = rb; }
		//--------------------------------------------------------------------

		/*
		//template<class PixFmt>
		public bool Attach(PixFmt& pixf, int x1, int y1, int x2, int y2)
		{
			RectI r(x1, y1, x2, y2);
			if(r.Clip(RectI(0, 0, pixf.Width()-1, pixf.Height()-1)))
			{
				int Stride = pixf.Stride();
				m_rbuf.Attach(pixf.PixelPointer(r.x1, Stride < 0 ? r.y2 : r.y1), 
							   (r.x2 - r.x1) + 1,
							   (r.y2 - r.y1) + 1,
							   Stride);
				return true;
			}
			return false;
		}
		 */

		//--------------------------------------------------------------------
		public uint Width
		{
			get { return m_rbuf.Width(); }
		}
		public uint Height
		{
			get { return m_rbuf.Height(); }
		}
		public int Stride
		{
			get { return m_rbuf.StrideInBytes(); }
		}

		//--------------------------------------------------------------------
		unsafe public byte* RowPointer(int y) { return m_rbuf.GetPixelPointer(y); }
		//public row_data     row(int y)     { return m_rbuf.row(y); }

		unsafe public byte* PixelPointer(int x, int y)
		{
			return m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
		}

		//--------------------------------------------------------------------
		unsafe public void MakePixel(byte* p, IColorType c)
		{
			p[0] = (byte)c.A_Byte;
		}

		public RGBA_Bytes Pixel(int x, int y)
		{
			unsafe
			{
				byte* p = m_rbuf.GetPixelPointer(y);
				if (p != null)
				{
					p += x * m_Step + m_Offset;
					return new RGBA_Bytes(*p, *p, *p, 255);
				}
				return new RGBA_Bytes();
			}
		}
		/*
	   //--------------------------------------------------------------------
	   public color_type Pixel(int x, int y)
	   {
		   byte* p = (byte*)m_rbuf.RowPointer(y) + x * Step + Offset;
		   return color_type(*p);
	   }
		 */

		//--------------------------------------------------------------------
		unsafe public void CopyPixel(int x, int y, byte* c)
		{
			*((byte*)m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset) = c[0];
		}

		//--------------------------------------------------------------------
		public void BlendPixel(int x, int y, RGBA_Bytes c, byte cover)
		{
			throw new System.NotImplementedException();
			/*
			unsafe
			{
				CopyOrBlendPixel(m_rbuf.RowPointer(x, y, 1) + x * Step + Offset,
								  c,
								  cover);
			}
			 */
		}

		//--------------------------------------------------------------------
		public void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c)
		{
			unsafe
			{
				byte* p = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;

				do
				{
					*p = c.m_R;
					p += m_Step;
				}
				while (--len != 0);
			}
		}

		//--------------------------------------------------------------------
		public void CopyVerticalLine(int x, int y, uint len, RGBA_Bytes c)
		{
			int ScanWidth = m_rbuf.StrideInBytes();
			unsafe
			{
				byte* p = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;

				do
				{
					*p = c.m_R;
					p = &p[ScanWidth];
				}
				while (--len != 0);
			}
		}

		//--------------------------------------------------------------------
		public void BlendHorizontalLine(int x1, int y, int x2, RGBA_Bytes c, byte cover)
		{
			unsafe
			{
				if (c.m_A != 0)
				{
					int len = x2 - x1 + 1;
					byte* p = m_rbuf.GetPixelPointer(y) + x1 * m_Step + m_Offset;

					uint alpha = (uint)((c.m_A) * (cover + 1)) >> 8;
					if (alpha == base_mask)
					{
						do
						{
							*p = c.m_R;
							p += m_Step;
						}
						while (--len != 0);
					}
					else
					{
						do
						{
							m_Blender.BlendPixel(p, c.m_R, alpha, cover);
							p += m_Step;
						}
						while (--len != 0);
					}
				}
			}
		}


		//--------------------------------------------------------------------
		public void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover)
		{
			unsafe
			{
				if (c.m_A != 0)
				{
					int len = y2 - y1 + 1;
					int ScanWidth = m_rbuf.StrideInBytes();
					byte* p = m_rbuf.GetPixelPointer(y1) + x * m_Step + m_Offset;

					uint alpha = (uint)((c.m_A) * (cover + 1)) >> 8;
					if (alpha == base_mask)
					{
						do
						{
							*p = c.m_R;
							p = &p[ScanWidth];
						}
						while (--len != 0);
					}
					else
					{
						do
						{
							m_Blender.BlendPixel(p, c.m_R, alpha, cover);
							p = &p[ScanWidth];
						}
						while (--len != 0);
					}
				}
			}
		}

		//--------------------------------------------------------------------
		unsafe public void BlendSolidHorizontalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
		{
			if (c.m_A != 0)
			{
				byte* p = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;

				do
				{
					uint alpha = (uint)((c.m_A) * (uint)((*covers) + 1)) >> 8;
					if (alpha == base_mask)
					{
						*p = c.m_R;
					}
					else
					{
						m_Blender.BlendPixel(p, c.m_R, alpha, *covers);
					}
					p += m_Step;
					++covers;
				}
				while (--len != 0);
			}
		}

		public unsafe void BlendSolidVerticalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
		{
			if (c.m_A != 0)
			{
				int ScanWidth = m_rbuf.StrideInBytes();
				byte* p = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;

				do
				{
					uint alpha = (uint)((c.m_A) * (uint)((*covers) + 1)) >> 8;
					if (alpha == base_mask)
					{
						*p = c.m_R;
					}
					else
					{
						m_Blender.BlendPixel(p, c.m_R, alpha, *covers);
					}
					p = &p[ScanWidth];
					++covers;
				}
				while (--len != 0);
			}
		}

		unsafe public void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
		{
			byte* p = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;

			do
			{
				*p = colors[0].m_R;
				p += m_Step;
				++colors;
			}
			while (--len != 0);
		}

		//--------------------------------------------------------------------
		unsafe public void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
		{
			byte* p = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;
			int ScanWidth = m_rbuf.StrideInBytes();
			do
			{
				*p = colors[0].m_R;
				p = &p[ScanWidth];
				++colors;
			}
			while (--len != 0);
		}

		//--------------------------------------------------------------------
		unsafe public void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
		{
			byte* p = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;

			if (covers != null)
			{
				do
				{
					copy_or_blend_pix(p, *colors++, *covers++);
					p += m_Step;
				}
				while (--len != 0);
			}
			else
			{
				if (cover == 255)
				{
					do
					{
						if (colors[0].m_A == (byte)base_mask)
						{
							*p = colors[0].m_A;
						}
						else
						{
							copy_or_blend_pix(p, *colors);
						}
						p += m_Step;
						++colors;
					}
					while (--len != 0);
				}
				else
				{
					do
					{
						copy_or_blend_pix(p, *colors++, cover);
						p += m_Step;
					}
					while (--len != 0);
				}
			}
		}

		//--------------------------------------------------------------------
		private unsafe void copy_or_blend_pix(byte* p, RGBA_Bytes c, uint cover)
		{
			if (c.m_A != 0)
			{
				uint alpha = (uint)((c.m_A) * (cover + 1)) >> 8;
				if (alpha == base_mask)
				{
					*p = c.m_A;
				}
				else
				{
					m_Blender.BlendPixel(p, c.m_A, alpha, cover);
				}
			}
		}


		private unsafe void copy_or_blend_pix(byte* p, RGBA_Bytes c)
		{
			if (c.m_A != 0)
			{
				if (c.m_A == base_mask)
				{
					*p = c.m_A;
				}
				else
				{
					m_Blender.BlendPixel(p, c.m_A, c.A_Byte);
				}
			}
		}

		public unsafe void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
		{
			int ScanWidth = m_rbuf.StrideInBytes();
			byte* p = m_rbuf.GetPixelPointer(y) + x * m_Step + m_Offset;

			if (covers != null)
			{
				do
				{
					copy_or_blend_pix(p, *colors++, *covers++);
					p = &p[ScanWidth];
				}
				while (--len != 0);
			}
			else
			{
				if (cover == 255)
				{
					do
					{
						if (colors[0].m_A == (byte)base_mask)
						{
							*p = colors[0].m_A;
						}
						else
						{
							copy_or_blend_pix(p, *colors);
						}
						p = &p[ScanWidth];
						++colors;
					}
					while (--len != 0);
				}
				else
				{
					do
					{
						copy_or_blend_pix(p, *colors++, cover);
						p = &p[ScanWidth];
					}
					while (--len != 0);
				}
			}
		}

		public void CopyFrom(RasterBuffer sourceBuffer, int xdst, int ydst, int xsrc, int ysrc, uint len)
		{
			unsafe
			{
				byte* pSource = sourceBuffer.GetPixelPointer(ysrc);
				if (pSource != null)
				{
					int BytesPerScanLine = Stride;
					Basics.memmove(m_rbuf.GetPixelPointer(ydst) + xdst * BytesPerScanLine,
							pSource + xsrc * BytesPerScanLine,
							(int)len * BytesPerScanLine);
				}
			}
		}
		/*

								//--------------------------------------------------------------------
								//template<class Function> 
								public void for_each_pixel(Function f)
								{
									uint y;
									for(y = 0; y < Height(); ++y)
									{
										row_data r = m_rbuf.row(y);
										if(r.ptr)
										{
											uint len = r.x2 - r.x1 + 1;

											byte* p = (byte*)
												m_rbuf.RowPointer(r.x1, y, len) + r.x1 * Step + Offset;

											do
											{
												f(p);
												p += Step;
											}
											while(--len);
										}
									}
								}

								//--------------------------------------------------------------------
								//template<class GammaLut> 
								public void apply_gamma_dir(GammaLut& g)
								{
									for_each_pixel(apply_gamma_dir_gray<color_type, GammaLut>(g));
								}

								//--------------------------------------------------------------------
								//template<class GammaLut> 
								public void apply_gamma_inv(GammaLut& g)
								{
									for_each_pixel(apply_gamma_inv_gray<color_type, GammaLut>(g));
								}

								//--------------------------------------------------------------------
								//template<class RenBuf2>

								//--------------------------------------------------------------------
								//template<class SrcPixelFormatRenderer>
								public void blend_from_color(SrcPixelFormatRenderer& from, 
													  color_type& Color,
													  int xdst, int ydst,
													  int xsrc, int ysrc,
													  uint len,
													  byte cover)
								{
									typedef typename SrcPixelFormatRenderer::value_type src_value_type;
									src_value_type* psrc = (src_value_type*)from.RowPointer(ysrc);
									if(psrc)
									{
										byte* pdst = 
											(byte*)m_rbuf.RowPointer(xdst, ydst, len) + xdst;
										do 
										{
											CopyOrBlendPixel(pdst, Color, (*psrc * cover + BaseMask) >> BaseShift);
											++psrc;
											++pdst;
										}
										while(--len);
									}
								}

								//--------------------------------------------------------------------
								//template<class SrcPixelFormatRenderer>
								public void blend_from_lut(SrcPixelFormatRenderer& from, 
													color_type* color_lut,
													int xdst, int ydst,
													int xsrc, int ysrc,
													uint len,
													byte cover)
								{
									typedef typename SrcPixelFormatRenderer::value_type src_value_type;
									src_value_type* psrc = (src_value_type*)from.RowPointer(ysrc);
									if(psrc)
									{
										byte* pdst = 
											(byte*)m_rbuf.RowPointer(xdst, ydst, len) + xdst;
										do 
										{
											CopyOrBlendPixel(pdst, color_lut[*psrc], cover);
											++psrc;
											++pdst;
										}
										while(--len);
									}
								}

								 */
	};
}
