/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
using System;

namespace Pictor.PixelFormat
{
	///<summary>
	///</summary>
	public class BlenderBaseBGR
	{
		///<summary>
		///</summary>
		public uint NumPixelBits { get { return 24; } }
		///<summary>
		///</summary>
		public enum Order { R = 2, G = 1, B = 0 };

		///<summary>
		///</summary>
		public int OrderR { get { return (int)Order.R; } }
		///<summary>
		///</summary>
		public int OrderG { get { return (int)Order.G; } }
		///<summary>
		///</summary>
		public int OrderB { get { return (int)Order.B; } }
		///<summary>
		///</summary>
		public int OrderA { get { return -1; } }

		///<summary>
		///</summary>
		public const byte BaseMask = 255;
	};

	///<summary>
	///</summary>
	public sealed class BlenderBGR : BlenderBaseBGR, IBlender
	{
		///<summary>
		///</summary>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		///<param name="cover"></param>
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			BlendPixel(p, cr, cg, cb, alpha);
		}

		///<summary>
		///</summary>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			unchecked
			{
				uint r = p[(int)Order.R];
				uint g = p[(int)Order.G];
				uint b = p[(int)Order.B];
				p[(int)Order.R] = (byte)(((cr - r) * alpha + (r << (int)RGBA_Bytes.BaseShift)) >> (int)RGBA_Bytes.BaseShift);
				p[(int)Order.G] = (byte)(((cg - g) * alpha + (g << (int)RGBA_Bytes.BaseShift)) >> (int)RGBA_Bytes.BaseShift);
				p[(int)Order.B] = (byte)(((cb - b) * alpha + (b << (int)RGBA_Bytes.BaseShift)) >> (int)RGBA_Bytes.BaseShift);
			}
		}
	};

	///<summary>
	///</summary>
	public sealed class BlenderGammaBGR : BlenderBaseBGR, IBlender
	{
		private GammaLookupTable _gamma;

		///<summary>
		///</summary>
		public BlenderGammaBGR()
		{
			_gamma = new GammaLookupTable();
		}

		///<summary>
		///</summary>
		///<param name="g"></param>
		public BlenderGammaBGR(GammaLookupTable g)
		{
			_gamma = g;
		}

		///<summary>
		///</summary>
		///<param name="g"></param>
		public void Gamma(GammaLookupTable g)
		{
			_gamma = g;
		}

		///<summary>
		///</summary>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		///<param name="cover"></param>
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			BlendPixel(p, cr, cg, cb, alpha);
		}

		///<summary>
		///</summary>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			unchecked
			{
				uint r = p[(int)Order.R];
				uint g = p[(int)Order.G];
				uint b = p[(int)Order.B];
				p[(int)Order.R] = _gamma.Inv((byte)(((cr - r) * alpha + (r << RGBA_Bytes.BaseShift)) >> RGBA_Bytes.BaseShift));
				p[(int)Order.G] = _gamma.Inv((byte)(((cg - g) * alpha + (g << RGBA_Bytes.BaseShift)) >> RGBA_Bytes.BaseShift));
				p[(int)Order.B] = _gamma.Inv((byte)(((cb - b) * alpha + (b << RGBA_Bytes.BaseShift)) >> RGBA_Bytes.BaseShift));
			}
		}

	};

	///<summary>
	///</summary>
	public sealed class BlenderPreMultBGR : BlenderBaseBGR, IBlender
	{
		///<summary>
		///</summary>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		///<param name="cover"></param>
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			uint oneOverAlpha = BaseMask - alpha;
			unchecked
			{
				cover = (cover + 1) << (RGBA_Bytes.BaseShift - 8);
				p[(int)Order.R] = (byte)((p[(int)Order.R] * oneOverAlpha + cr * cover) >> RGBA_Bytes.BaseShift);
				p[(int)Order.G] = (byte)((p[(int)Order.G] * oneOverAlpha + cg * cover) >> RGBA_Bytes.BaseShift);
				p[(int)Order.B] = (byte)((p[(int)Order.B] * oneOverAlpha + cb * cover) >> RGBA_Bytes.BaseShift);
			}
		}

		///<summary>
		///</summary>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			uint oneOverAlpha = BaseMask - alpha;
			unchecked
			{
				p[(int)Order.R] = (byte)(((p[(int)Order.R] * oneOverAlpha) >> RGBA_Bytes.BaseShift) + cr);
				p[(int)Order.G] = (byte)(((p[(int)Order.G] * oneOverAlpha) >> RGBA_Bytes.BaseShift) + cg);
				p[(int)Order.B] = (byte)(((p[(int)Order.B] * oneOverAlpha) >> RGBA_Bytes.BaseShift) + cb);
			}
		}

	};

	///<summary>
	///</summary>
	public sealed class BlenderPreMultClampedBGR : BlenderBaseBGR, IBlender
	{
		///<summary>
		///</summary>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		///<param name="cover"></param>
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			uint oneOverAlpha = BaseMask - alpha;
			p[(int)Order.R] = (byte)System.Math.Min(((p[(int)Order.R] * oneOverAlpha + cr * cover) >> RGBA_Bytes.BaseShift), BaseMask);
			p[(int)Order.G] = (byte)System.Math.Min(((p[(int)Order.G] * oneOverAlpha + cg * cover) >> RGBA_Bytes.BaseShift), BaseMask);
			p[(int)Order.B] = (byte)System.Math.Min(((p[(int)Order.B] * oneOverAlpha + cb * cover) >> RGBA_Bytes.BaseShift), BaseMask);
		}

		///<summary>
		///</summary>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			uint oneOverAlpha = BaseMask - alpha;
			p[(int)Order.R] = (byte)System.Math.Min((((p[(int)Order.R] * oneOverAlpha) >> RGBA_Bytes.BaseShift) + cr), BaseMask);
			p[(int)Order.G] = (byte)System.Math.Min((((p[(int)Order.G] * oneOverAlpha) >> RGBA_Bytes.BaseShift) + cg), BaseMask);
			p[(int)Order.B] = (byte)System.Math.Min((((p[(int)Order.B] * oneOverAlpha) >> RGBA_Bytes.BaseShift) + cb), BaseMask);
		}

	};

	///<summary>
	///</summary>
	public sealed class BlenderAddativeBGR : BlenderBaseBGR, IBlender
	{
		///<summary>
		///</summary>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		///<param name="cover"></param>
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			p[(int)Order.R] = (byte)System.Math.Min(((p[(int)Order.R] + cr * cover) >> RGBA_Bytes.BaseShift), BaseMask);
			p[(int)Order.G] = (byte)System.Math.Min(((p[(int)Order.G] + cg * cover) >> RGBA_Bytes.BaseShift), BaseMask);
			p[(int)Order.B] = (byte)System.Math.Min(((p[(int)Order.B] + cb * cover) >> RGBA_Bytes.BaseShift), BaseMask);
		}

		///<summary>
		///</summary>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			p[(int)Order.R] = (byte)System.Math.Min((p[(int)Order.R] + cr), BaseMask);
			p[(int)Order.G] = (byte)System.Math.Min((p[(int)Order.G] + cg), BaseMask);
			p[(int)Order.B] = (byte)System.Math.Min((p[(int)Order.B] + cb), BaseMask);
		}
	};

	///<summary>
	///</summary>
	public static class CopyOrBlendBGRWrapper
	{
		enum Order
		{
			R = 2,
			G = 1,
			B = 0,
		};

		const byte BaseMask = 255;

		///<summary>
		///</summary>
		///<param name="blender"></param>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		public unsafe static void CopyOrBlendPixel(IBlender blender, byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			if (blender == null) throw new ArgumentNullException("blender");

			if (alpha == 0) return;

			if (alpha == BaseMask)
			{
				p[(int)Order.R] = (byte)cr;
				p[(int)Order.G] = (byte)cg;
				p[(int)Order.B] = (byte)cb;
			}
			else
			{
				blender.BlendPixel(p, cr, cg, cb, alpha);
			}
		}

		///<summary>
		///</summary>
		///<param name="blender"></param>
		///<param name="p"></param>
		///<param name="cr"></param>
		///<param name="cg"></param>
		///<param name="cb"></param>
		///<param name="alpha"></param>
		///<param name="cover"></param>
		public unsafe static void CopyOrBlendPixel(IBlender blender, byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			if (blender == null) throw new ArgumentNullException("Blender");

			if (cover == 255)
			{
				CopyOrBlendPixel(blender, p, cr, cg, cb, alpha);
			}
			else
			{
				if (alpha != 0)
				{
					alpha = (alpha * (cover + 1)) >> 8;
					if (alpha == BaseMask)
					{
						p[(int)Order.R] = (byte)cr;
						p[(int)Order.G] = (byte)cg;
						p[(int)Order.B] = (byte)cb;
					}
					else
					{
						blender.BlendPixel(p, cr, cg, cb, alpha, cover);
					}
				}
			}
		}
	};

	///<summary>
	///</summary>
	public sealed class FormatRGB : IPixelFormat
	{

		private RasterBuffer _rasterBuffer;
		private IBlender _blender;
		private int _orderR;
		private int _orderG;
		private int _orderB;

		const byte BaseMask = 255;
		const int PixelWidth = 3;

		public uint PixelWidthInBytes
		{
			get { return 3; }
		}

		///<summary>
		///</summary>
		///<param name="rb"></param>
		///<param name="blender"></param>
		public FormatRGB(RasterBuffer rb, IBlender blender)
		{
			_rasterBuffer = rb;
			Blender = blender;
		}

		///<summary>
		///</summary>
		///<param name="rb"></param>
		///<param name="blender"></param>
		///<param name="gammaTable"></param>
		public FormatRGB(RasterBuffer rb, IBlender blender, GammaLookupTable gammaTable)
		{
			_rasterBuffer = rb;
			Blender = blender;
		}

		public IBlender Blender
		{
			get
			{
				return _blender;
			}

			set
			{
				if (value.NumPixelBits != 24)
				{
					throw new NotSupportedException("pixfmt_alpha_blend_rgb requires your Blender to be 24 bit. Change the Blender or the Pixel Format");
				}
				_blender = value;
				_orderR = _blender.OrderR;
				_orderG = _blender.OrderG;
				_orderB = _blender.OrderB;
			}
		}

		public void Attach(RasterBuffer rb) { _rasterBuffer = rb; }

		///<summary>
		///</summary>
		///<param name="pixf"></param>
		///<param name="x1"></param>
		///<param name="y1"></param>
		///<param name="x2"></param>
		///<param name="y2"></param>
		///<returns></returns>
		public bool Attach(IPixelFormat pixf, int x1, int y1, int x2, int y2)
		{
			RectI r = new RectI(x1, y1, x2, y2);
			if (r.Clip(new RectI(0, 0, (int)pixf.Width - 1, (int)pixf.Height - 1)))
			{
				int stride = pixf.Stride;
				unsafe
				{
					_rasterBuffer.Attach(pixf.PixelPointer(r.x1, stride < 0 ? r.y2 : r.y1),
						(uint)(r.x2 - r.x1) + 1,
						(uint)(r.y2 - r.y1) + 1,
						stride, 3);
				}
				return true;
			}
			return false;
		}

		public uint Width
		{
			get { return _rasterBuffer.Width(); }
		}
		public uint Height
		{
			get { return _rasterBuffer.Height(); }
		}
		public int Stride
		{
			get { return _rasterBuffer.StrideInBytes(); }
		}

		//--------------------------------------------------------------------
		public RasterBuffer RenderingBuffer
		{
			get { return _rasterBuffer; }
		}
		unsafe public byte* RowPointer(int y) { return _rasterBuffer.GetPixelPointer(y); }

		//--------------------------------------------------------------------
		unsafe public byte* PixelPointer(int x, int y)
		{
			return _rasterBuffer.GetPixelPointer(y) + x * PixelWidth;
		}

		//--------------------------------------------------------------------
		unsafe public void MakePixel(byte* p, IColorType c)
		{
			p[_orderR] = (byte)c.R_Byte;
			p[_orderG] = (byte)c.G_Byte;
			p[_orderB] = (byte)c.B_Byte;
		}

		//--------------------------------------------------------------------
		public RGBA_Bytes Pixel(int x, int y)
		{
			unsafe
			{
				byte* p = _rasterBuffer.GetPixelPointer(y);
				if (p != null)
				{
					p += x * 3;
					return new RGBA_Bytes(p[_orderR], p[_orderG], p[_orderB], 255);
				}
				return new RGBA_Bytes();
			}
		}

		//--------------------------------------------------------------------
		unsafe public void CopyPixel(int x, int y, byte* c)
		{
			byte* p = _rasterBuffer.GetPixelPointer(y) + x + x + x;
			((int*)p)[0] = ((int*)c)[0];
			((int*)p)[1] = ((int*)c)[1];
			((int*)p)[2] = ((int*)c)[2];
			((int*)p)[3] = ((int*)c)[3];
		}

		//--------------------------------------------------------------------
		public void BlendPixel(int x, int y, RGBA_Bytes c, byte cover)
		{
			/*
			cob_type::CopyOrBlendPixel(
				(value_type*)_rasterBuffer->RowPointer(x, y, 1)  + x + x + x, 
				c.r, c.g, c.b, c.a, 
				cover);*/
		}

		//--------------------------------------------------------------------
		public unsafe void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c)
		{
			byte* p = _rasterBuffer.GetPixelPointer(y) + x + x + x;
			byte cr = c.m_R;
			byte cg = c.m_G;
			byte cb = c.m_B;
			do
			{
				p[_orderR] = cr;
				p[_orderG] = cg;
				p[_orderB] = cb;
				p += 3;
			}
			while (--len != 0);
		}

		//--------------------------------------------------------------------
		public unsafe void CopyVerticalLine(int x, int y, uint len, RGBA_Bytes c)
		{
			int scanWidth = _rasterBuffer.StrideInBytes();
			byte* p = _rasterBuffer.GetPixelPointer(y) + x + x + x;
			byte cr = c.m_R;
			byte cg = c.m_G;
			byte cb = c.m_B;
			do
			{
				p[_orderR] = cr;
				p[_orderG] = cg;
				p[_orderB] = cb;
				p = &p[scanWidth];
			}
			while (--len != 0);
		}


		//--------------------------------------------------------------------
		public void BlendHorizontalLine(int x1, int y, int x2, RGBA_Bytes c, byte cover)
		{
			if (c.m_A != 0)
			{
				unsafe
				{
					int len = x2 - x1 + 1;
					byte* p = (byte*)_rasterBuffer.GetPixelPointer(y) + x1 * 3;
					uint alpha = (uint)(((int)(c.A_Byte) * (cover + 1)) >> 8);
					if (alpha == BaseMask)
					{
						byte cr = c.m_R;
						byte cg = c.m_G;
						byte cb = c.m_B;
						do
						{
							p[_orderR] = cr;
							p[_orderG] = cg;
							p[_orderB] = cb;
							p += 3;
						}
						while (--len != 0);
					}
					else
					{
						if (cover == 255)
						{
							do
							{
								_blender.BlendPixel(p, c.m_R, c.m_G, c.m_B, alpha);
								p += 3;
							}
							while (--len != 0);
						}
						else
						{
							do
							{
#if USE_BLENDER
								m_Blender.blend_pix(p, c.m_R, c.m_G, c.m_B, alpha);
#else
								unchecked
								{
									uint b = p[0];
									uint g = p[1];
									uint r = p[2];
									uint a = p[3];
									p[0] = (byte)(((c.m_B - b) * alpha + (b << RGBA_Bytes.BaseShift)) >> RGBA_Bytes.BaseShift);
									p[1] = (byte)(((c.m_G - g) * alpha + (g << RGBA_Bytes.BaseShift)) >> RGBA_Bytes.BaseShift);
									p[2] = (byte)(((c.m_R - r) * alpha + (r << RGBA_Bytes.BaseShift)) >> RGBA_Bytes.BaseShift);
									p[3] = (byte)((alpha + a) - ((alpha * a + BaseMask) >> RGBA_Bytes.BaseShift));
								}
#endif
								p += 3;
							}
							while (--len != 0);
						}
					}
				}
			}
		}

		//--------------------------------------------------------------------
		public void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover)
		{
			int scanWidth = _rasterBuffer.StrideInBytes();
			if (c.m_A != 0)
			{
				unsafe
				{
					int len = y2 - y1 + 1;
					byte* p = _rasterBuffer.GetPixelPointer(y1) + x + x + x;
					uint alpha = (uint)((c.m_A * (cover + 1)) >> 8);
					if (alpha == BaseMask)
					{
						byte cr = c.m_R;
						byte cg = c.m_G;
						byte cb = c.m_B;
						do
						{
							p[_orderR] = cr;
							p[_orderG] = cg;
							p[_orderB] = cb;
							p = &p[scanWidth];
						}
						while (--len != 0);
					}
					else
					{
						if (cover == 255)
						{
							do
							{
								_blender.BlendPixel(p, c.m_R, c.m_G, c.m_B, alpha);
								p = &p[scanWidth];
							}
							while (--len != 0);
						}
						else
						{
							do
							{
								_blender.BlendPixel(p, c.m_R, c.m_G, c.m_B, alpha);
								p = &p[scanWidth];
							}
							while (--len != 0);
						}
					}
				}
			}
		}

		//--------------------------------------------------------------------
		unsafe public void BlendSolidHorizontalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
		{
			if (c.m_A != 0)
			{
				unchecked
				{
					byte* p = _rasterBuffer.GetPixelPointer(y) + x + x + x;
					do
					{
						uint alpha = (c.m_A * ((uint)(*covers) + 1)) >> 8;
						if (alpha == BaseMask)
						{
							p[_orderR] = c.m_R;
							p[_orderG] = c.m_G;
							p[_orderB] = c.m_B;
						}
						else
						{
#if false
							m_Blender.blend_pix(p, c.m_R, c.m_G, c.m_B, alpha);
#else // testing performance.  This is not noticibly faster.
							unchecked
							{
								uint r = p[2];
								uint g = p[1];
								uint b = p[0];
								p[0] = (byte)(((c.m_B - b) * alpha + (b << RGBA_Bytes.BaseShift)) >> RGBA_Bytes.BaseShift);
								p[1] = (byte)(((c.m_G - g) * alpha + (g << RGBA_Bytes.BaseShift)) >> RGBA_Bytes.BaseShift);
								p[2] = (byte)(((c.m_R - r) * alpha + (r << RGBA_Bytes.BaseShift)) >> RGBA_Bytes.BaseShift);
							}
#endif
						}
						p += 3;
						++covers;
					}
					while (--len != 0);
				}
			}
		}

		//--------------------------------------------------------------------
		unsafe public void BlendSolidVerticalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
		{
			if (c.m_A == 0) return;

			int scanWidth = _rasterBuffer.StrideInBytes();
			unchecked
			{
				byte* p = _rasterBuffer.GetPixelPointer(y) + x + x + x;
				do
				{
					uint alpha = (c.m_A * ((uint)(*covers) + 1)) >> 8;
					if (alpha == BaseMask)
					{
						p[_orderR] = c.m_R;
						p[_orderG] = c.m_G;
						p[_orderB] = c.m_B;
					}
					else
					{
						_blender.BlendPixel(p, c.m_R, c.m_G, c.m_B, alpha);
					}
					p = &p[scanWidth];
					++covers;
				}
				while (--len != 0);
			}
		}

		//--------------------------------------------------------------------
		unsafe public void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
		{
			byte* p = _rasterBuffer.GetPixelPointer(y) + x + x + x;
			do
			{
				p[_orderR] = colors[0].m_R;
				p[_orderG] = colors[0].m_G;
				p[_orderB] = colors[0].m_B;
				++colors;
				p += 3;
			}
			while (--len != 0);
		}

		//--------------------------------------------------------------------
		public unsafe void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
		{
			int scanWidth = _rasterBuffer.StrideInBytes();
			byte* p = _rasterBuffer.GetPixelPointer(y) + x + x + x;
			do
			{
				p[_orderR] = colors[0].m_R;
				p[_orderG] = colors[0].m_G;
				p[_orderB] = colors[0].m_B;
				p = &p[scanWidth];
				++colors;
			}
			while (--len != 0);
		}

		//--------------------------------------------------------------------
		unsafe public void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
		{
			byte* p = _rasterBuffer.GetPixelPointer(y) + x + x + x;
			if (covers != null)
			{
				do
				{
					CopyOrBlendBGRWrapper.CopyOrBlendPixel(_blender, p,
												colors->m_R,
												colors->m_G,
												colors->m_B,
												colors->m_A,
												*covers++);
					p += 3;
					++colors;
				}
				while (--len != 0);
			}
			else
			{
				if (cover == 255)
				{
					do
					{
						CopyOrBlendBGRWrapper.CopyOrBlendPixel(_blender, p,
													colors->R_Byte,
													colors->G_Byte,
													colors->B_Byte,
													colors->A_Byte);
						p += 3;
						++colors;
					}
					while (--len != 0);
				}
				else
				{
					do
					{
						CopyOrBlendBGRWrapper.CopyOrBlendPixel(_blender, p,
													colors->R_Byte,
													colors->G_Byte,
													colors->B_Byte,
													colors->A_Byte,
													cover);
						p += 3;
						++colors;
					}
					while (--len != 0);
				}
			}
		}

		public unsafe void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
		{
			byte* p = _rasterBuffer.GetPixelPointer(y) + x + x + x;
			int scanWidth = _rasterBuffer.StrideInBytes();
			if (covers != null)
			{
				do
				{
					CopyOrBlendBGRWrapper.CopyOrBlendPixel(_blender, p, colors->m_R, colors->m_G, colors->m_B, colors->m_A, *covers++);
					p = &p[scanWidth];
					++colors;
				}
				while (--len != 0);
			}
			else
			{
				if (cover == 255)
				{
					do
					{
						CopyOrBlendBGRWrapper.CopyOrBlendPixel(_blender, p, colors->R_Byte, colors->G_Byte, colors->B_Byte, colors->A_Byte); p = &p[scanWidth];
						++colors;
					}
					while (--len != 0);
				}
				else
				{
					do
					{
						CopyOrBlendBGRWrapper.CopyOrBlendPixel(_blender, p, colors->R_Byte, colors->G_Byte, colors->B_Byte, colors->A_Byte, cover);
						p = &p[scanWidth];
						++colors;
					}
					while (--len != 0);
				}
			}
		}

		public void CopyFrom(RasterBuffer sourceBuffer, int xdst, int ydst, int xsrc, int ysrc, uint len)
		{
			unsafe
			{
				Basics.memmove(&_rasterBuffer.GetPixelPointer(ydst)[xdst * 3],
							   &sourceBuffer.GetPixelPointer(ysrc)[xsrc * 3],
							   (int)len * 3);
			}
		}
	}
}

