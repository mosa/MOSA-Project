/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
using System;
using Pictor;

namespace Pictor.PixelFormat
{
	//-----------------------------------------------------------ClippingPixelFormtProxy
	public class FormatClippingProxy : PixelFormatProxy
	{
		private RectI m_clip_box;

		public const byte cover_full = 255;

		//--------------------------------------------------------------------
		public FormatClippingProxy(IPixelFormat ren)
			: base(ren)
		{
			m_clip_box = new RectI(0, 0, (int)ren.Width - 1, (int)ren.Height - 1);
		}

		public override void Attach(IPixelFormat ren)
		{
			base.Attach(ren);
			m_clip_box = new RectI(0, 0, (int)ren.Width - 1, (int)ren.Height - 1);
		}

		//--------------------------------------------------------------------
		//public IPixelFormat ren() { return m_ren; }

		//--------------------------------------------------------------------
		public bool SetClippingBox(int x1, int y1, int x2, int y2)
		{
			RectI cb = new RectI(x1, y1, x2, y2);
			cb.Normalize();
			if (cb.Clip(new RectI(0, 0, (int)Width - 1, (int)Height - 1)))
			{
				m_clip_box = cb;
				return true;
			}
			m_clip_box.x1 = 1;
			m_clip_box.y1 = 1;
			m_clip_box.x2 = 0;
			m_clip_box.y2 = 0;
			return false;
		}

		//--------------------------------------------------------------------
		public void ResetClipping(bool visibility)
		{
			if (visibility)
			{
				m_clip_box.x1 = 0;
				m_clip_box.y1 = 0;
				m_clip_box.x2 = (int)Width - 1;
				m_clip_box.y2 = (int)Height - 1;
			}
			else
			{
				m_clip_box.x1 = 1;
				m_clip_box.y1 = 1;
				m_clip_box.x2 = 0;
				m_clip_box.y2 = 0;
			}
		}

		//--------------------------------------------------------------------
		public void ClipBoxNaked(int x1, int y1, int x2, int y2)
		{
			m_clip_box.x1 = x1;
			m_clip_box.y1 = y1;
			m_clip_box.x2 = x2;
			m_clip_box.y2 = y2;
		}

		//--------------------------------------------------------------------
		public bool IsInBox(int x, int y)
		{
			return x >= m_clip_box.x1 && y >= m_clip_box.y1 &&
				   x <= m_clip_box.x2 && y <= m_clip_box.y2;
		}

		//--------------------------------------------------------------------
		public RectI ClipBox
		{
			get { return m_clip_box; }
		}
		int MinX
		{
			get { return m_clip_box.x1; }
		}
		int MinY
		{
			get { return m_clip_box.y1; }
		}
		int MaxX
		{
			get { return m_clip_box.x2; }
		}
		int MaxY
		{
			get { return m_clip_box.y2; }
		}

		//--------------------------------------------------------------------
		public RectI BoundingClipBox
		{
			get { return m_clip_box; }
		}
		public int BoundingMinX
		{
			get { return m_clip_box.x1; }
		}
		public int BoundingMinY
		{
			get { return m_clip_box.y1; }
		}
		public int BoundingMaxX
		{
			get { return m_clip_box.x2; }
		}
		public int BoundingMaxY
		{
			get { return m_clip_box.y2; }
		}

		//--------------------------------------------------------------------
		public void Clear(IColorType in_c)
		{
			uint y;
			RGBA_Bytes c = new RGBA_Bytes(in_c.R_Byte, in_c.G_Byte, in_c.B_Byte, in_c.A_Byte);
			if (Width != 0)
			{
				for (y = 0; y < Height; y++)
				{
					base.CopyHorizontalLine(0, (int)y, Width, c);
				}
			}
		}


		//--------------------------------------------------------------------
		public override unsafe void CopyPixel(int x, int y, byte* c)
		{
			if (IsInBox(x, y))
			{
				base.CopyPixel(x, y, c);
			}
		}

		//--------------------------------------------------------------------
		public override void BlendPixel(int x, int y, RGBA_Bytes c, byte cover)
		{
			if (IsInBox(x, y))
			{
				base.BlendPixel(x, y, c.GetAsRGBA_Bytes(), cover);
			}
		}

		//--------------------------------------------------------------------
		public override RGBA_Bytes Pixel(int x, int y)
		{
			return IsInBox(x, y) ? base.Pixel(x, y) : new RGBA_Bytes();
		}

		//--------------------------------------------------------------------
		public override void CopyHorizontalLine(int x1, int y, uint x2, RGBA_Bytes c)
		{
			if (x1 > x2) { int t = (int)x2; x2 = (uint)x1; x1 = t; }
			if (y > MaxY) return;
			if (y < MinY) return;
			if (x1 > MaxX) return;
			if (x2 < MinX) return;

			if (x1 < MinX) x1 = MinX;
			if (x2 > MaxX) x2 = (uint)MaxX;

			base.CopyHorizontalLine(x1, y, (uint)(x2 - x1 + 1), c);
		}

		//--------------------------------------------------------------------
		public override void CopyVerticalLine(int x, int y1, uint y2, RGBA_Bytes c)
		{
			if (y1 > y2) { int t = (int)y2; y2 = (uint)y1; y1 = t; }
			if (x > MaxX) return;
			if (x < MinX) return;
			if (y1 > MaxY) return;
			if (y2 < MinY) return;

			if (y1 < MinY) y1 = MinY;
			if (y2 > MaxY) y2 = (uint)MaxY;

			base.CopyVerticalLine(x, y1, (uint)(y2 - y1 + 1), c);
		}

		//--------------------------------------------------------------------
		public override void BlendHorizontalLine(int x1, int y, int x2, RGBA_Bytes c, byte cover)
		{
			if (x1 > x2)
			{
				int t = (int)x2;
				x2 = x1;
				x1 = t;
			}
			if (y > MaxY)
				return;
			if (y < MinY)
				return;
			if (x1 > MaxX)
				return;
			if (x2 < MinX)
				return;

			if (x1 < MinX)
				x1 = MinX;
			if (x2 > MaxX)
				x2 = MaxX;

			base.BlendHorizontalLine(x1, y, x2, c, cover);
		}

		//--------------------------------------------------------------------
		public override void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover)
		{
			if (y1 > y2) { int t = y2; y2 = y1; y1 = t; }
			if (x > MaxX) return;
			if (x < MinX) return;
			if (y1 > MaxY) return;
			if (y2 < MinY) return;

			if (y1 < MinY) y1 = MinY;
			if (y2 > MaxY) y2 = MaxY;

			base.BlendVerticalLine(x, y1, y2, c, cover);
		}

		/*
		//--------------------------------------------------------------------
		public void copy_bar(int x1, int y1, int x2, int y2, IColorType c)
		{
			RectI rc(x1, y1, x2, y2);
			rc.Normalize();
			if(rc.Clip(ClipBox()))
			{
				int y;
				for(y = rc.y1; y <= rc.y2; y++)
				{
					m_ren->CopyHorizontalLine(rc.x1, y, uint(rc.x2 - rc.x1 + 1), c);
				}
			}
		}

		//--------------------------------------------------------------------
		public void blend_bar(int x1, int y1, int x2, int y2, 
					   IColorType c, byte cover)
		{
			RectI rc(x1, y1, x2, y2);
			rc.Normalize();
			if(rc.Clip(ClipBox()))
			{
				int y;
				for(y = rc.y1; y <= rc.y2; y++)
				{
					m_ren->BlendHorizontalLine(rc.x1,
									   y,
									   uint(rc.x2 - rc.x1 + 1), 
									   c, 
									   cover);
				}
			}
		}
		 */

		//--------------------------------------------------------------------
		public override unsafe void BlendSolidHorizontalSpan(int x, int y, uint in_len, RGBA_Bytes c, byte* covers)
		{
			int len = (int)in_len;
			if (y > MaxY) return;
			if (y < MinY) return;

			if (x < MinX)
			{
				len -= MinX - x;
				if (len <= 0) return;
				covers += MinX - x;
				x = MinX;
			}
			if (x + len > MaxX)
			{
				len = MaxX - x + 1;
				if (len <= 0) return;
			}
			base.BlendSolidHorizontalSpan(x, y, (uint)len, c, covers);
		}

		//--------------------------------------------------------------------
		public override unsafe void BlendSolidVerticalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
		{
			if (x > MaxX) return;
			if (x < MinX) return;

			if (y < MinY)
			{
				len -= (uint)(MinY - y);
				if (len <= 0) return;
				covers += MinY - y;
				y = MinY;
			}
			if (y + len > MaxY)
			{
				len = (uint)(MaxY - y + 1);
				if (len <= 0) return;
			}
			base.BlendSolidVerticalSpan(x, y, len, c, covers);
		}


		//--------------------------------------------------------------------
		public override unsafe void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
		{
			if (y > MaxY) return;
			if (y < MinY) return;

			if (x < MinX)
			{
				int d = MinX - x;
				len -= (uint)d;
				if (len <= 0) return;
				colors += d;
				x = MinX;
			}
			if (x + len > MaxX)
			{
				len = (uint)(MaxX - x + 1);
				if (len <= 0) return;
			}
			base.CopyHorizontalColorSpan(x, y, len, colors);
		}


		//--------------------------------------------------------------------
		public override unsafe void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
		{
			if (x > MaxX) return;
			if (x < MinX) return;

			if (y < MinY)
			{
				int d = MinY - y;
				len -= (uint)d;
				if (len <= 0) return;
				colors += d;
				y = MinY;
			}
			if (y + len > MaxY)
			{
				len = (uint)(MaxY - y + 1);
				if (len <= 0) return;
			}
			base.CopyVerticalColorSpan(x, y, len, colors);
		}

		//--------------------------------------------------------------------
		public unsafe void BlendHorizontalColorSpan(int x, int y, uint len,
							   RGBA_Bytes* colors, byte* covers)
		{
			throw new System.NotImplementedException();
			//BlendHorizontalColorSpan(x, y, len, Colors, covers, CoverFull);
		}

		public override unsafe void BlendHorizontalColorSpan(int x, int y, uint in_len, RGBA_Bytes* colors, byte* covers, byte cover)
		{
			int len = (int)in_len;
			if (y > MaxY)
				return;
			if (y < MinY)
				return;

			if (x < MinX)
			{
				int d = MinX - x;
				len -= d;
				if (len <= 0) return;
				if (covers != null) covers += d;
				colors += d;
				x = MinX;
			}
			if (x + len - 1 > MaxX)
			{
				len = MaxX - x + 1;
				if (len <= 0) return;
			}

			base.BlendHorizontalColorSpan(x, y, (uint)len, colors, covers, cover);
		}

		public void CopyFrom(RasterBuffer src)
		{
			CopyFrom(src, new RectI(0, 0, (int)src.Width(), (int)src.Height()), 0, 0);
		}

		public override void CopyFrom(RasterBuffer from, int xdst, int ydst, int xsrc, int ysrc, uint len)
		{
			throw new System.NotImplementedException();
		}

		public override unsafe void MakePixel(byte* p, IColorType c)
		{
			throw new System.NotImplementedException();
		}

		public void CopyFrom(RasterBuffer src,
					   RectI rect_src_ptr,
					   int dx,
					   int dy)
		{
			RectI rsrc = new RectI(rect_src_ptr.x1, rect_src_ptr.y1, rect_src_ptr.x2 + 1, rect_src_ptr.y2 + 1);

			// Version with xdst, ydst (absolute positioning)
			//RectI rdst(xdst, ydst, xdst + rsrc.x2 - rsrc.x1, ydst + rsrc.y2 - rsrc.y1);

			// Version with dx, dy (relative positioning)
			RectI rdst = new RectI(rsrc.x1 + dx, rsrc.y1 + dy, rsrc.x2 + dx, rsrc.y2 + dy);

			RectI rc = ClipRectangleArea(ref rdst, ref rsrc, (int)src.Width(), (int)src.Height());

			if (rc.x2 > 0)
			{
				int incy = 1;
				if (rdst.y1 > rsrc.y1)
				{
					rsrc.y1 += rc.y2 - 1;
					rdst.y1 += rc.y2 - 1;
					incy = -1;
				}
				while (rc.y2 > 0)
				{
					base.CopyFrom(src,
									 rdst.x1, rdst.y1,
									 rsrc.x1, rsrc.y1,
									 (uint)rc.x2);
					rdst.y1 += incy;
					rsrc.y1 += incy;
					--rc.y2;
				}
			}
		}

		public RectI ClipRectangleArea(ref RectI dst, ref RectI src, int wsrc, int hsrc)
		{
			RectI rc = new RectI(0, 0, 0, 0);
			RectI cb = ClipBox;
			++cb.x2;
			++cb.y2;

			if (src.x1 < 0)
			{
				dst.x1 -= src.x1;
				src.x1 = 0;
			}
			if (src.y1 < 0)
			{
				dst.y1 -= src.y1;
				src.y1 = 0;
			}

			if (src.x2 > wsrc) src.x2 = wsrc;
			if (src.y2 > hsrc) src.y2 = hsrc;

			if (dst.x1 < cb.x1)
			{
				src.x1 += cb.x1 - dst.x1;
				dst.x1 = cb.x1;
			}
			if (dst.y1 < cb.y1)
			{
				src.y1 += cb.y1 - dst.y1;
				dst.y1 = cb.y1;
			}

			if (dst.x2 > cb.x2) dst.x2 = cb.x2;
			if (dst.y2 > cb.y2) dst.y2 = cb.y2;

			rc.x2 = dst.x2 - dst.x1;
			rc.y2 = dst.y2 - dst.y1;

			if (rc.x2 > src.x2 - src.x1) rc.x2 = src.x2 - src.x1;
			if (rc.y2 > src.y2 - src.y1) rc.y2 = src.y2 - src.y1;
			return rc;
		}

		//--------------------------------------------------------------------
		public override unsafe void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
		{
			if (x > MaxX) return;
			if (x < MinX) return;

			if (y < MinY)
			{
				int d = MinY - y;
				len -= (uint)d;
				if (len <= 0) return;
				if (covers != null) covers += d;
				colors += d;
				y = MinY;
			}
			if (y + len > MaxY)
			{
				len = (uint)(MaxY - y + 1);
				if (len <= 0) return;
			}
			base.BlendVerticalColorSpan(x, y, len, colors, covers, cover);
		}

		/*
		//--------------------------------------------------------------------
		//template<class SrcPixelFormatRenderer>
		public void blend_from(rendering_buffer src)
		{
			blend_from(src, 0, 0, 0, Pictor::CoverFull)

		}

		public void blend_from(rendering_buffer src, 
						ref RectI rect_src_ptr, 
						int dx, 
						int dy,
						byte cover)
		{
			RectI rsrc(0, 0, src.Width(), src.Height());
			if(rect_src_ptr)
			{
				rsrc.x1 = rect_src_ptr->x1; 
				rsrc.y1 = rect_src_ptr->y1;
				rsrc.x2 = rect_src_ptr->x2 + 1;
				rsrc.y2 = rect_src_ptr->y2 + 1;
			}

			// Version with xdst, ydst (absolute positioning)
			//RectI rdst(xdst, ydst, xdst + rsrc.x2 - rsrc.x1, ydst + rsrc.y2 - rsrc.y1);

			// Version with dx, dy (relative positioning)
			RectI rdst(rsrc.x1 + dx, rsrc.y1 + dy, rsrc.x2 + dx, rsrc.y2 + dy);
			RectI rc = ClipRectangleArea(rdst, rsrc, src.Width(), src.Height());

			if(rc.x2 > 0)
			{
				int incy = 1;
				if(rdst.y1 > rsrc.y1)
				{
					rsrc.y1 += rc.y2 - 1;
					rdst.y1 += rc.y2 - 1;
					incy = -1;
				}
				while(rc.y2 > 0)
				{
					typename SrcPixelFormatRenderer::row_data rw = src.row(rsrc.y1);
					if(rw.ptr)
					{
						int x1src = rsrc.x1;
						int x1dst = rdst.x1;
						int len   = rc.x2;
						if(rw.x1 > x1src)
						{
							x1dst += rw.x1 - x1src;
							len   -= rw.x1 - x1src;
							x1src  = rw.x1;
						}
						if(len > 0)
						{
							if(x1src + len-1 > rw.x2)
							{
								len -= x1src + len - rw.x2 - 1;
							}
							if(len > 0)
							{
								m_ren->blend_from(src,
												  x1dst, rdst.y1,
												  x1src, rsrc.y1,
												  len,
												  cover);
							}
						}
					}
					rdst.y1 += incy;
					rsrc.y1 += incy;
					--rc.y2;
				}
			}
		}

		//--------------------------------------------------------------------
		//template<class SrcPixelFormatRenderer>
		public void blend_from_color(rendering_buffer src, 
							  IColorType Color)
		{
			blend_from_color(src, Color, h0, 0, 0, Pictor::CoverFull)
		}
		public void blend_from_color(rendering_buffer src, 
							  IColorType Color,
							  ref RectI rect_src_ptr, 
							  int dx, 
							  int dy,
							  byte cover)
		{
			RectI rsrc(0, 0, src.Width(), src.Height());
			if(rect_src_ptr)
			{
				rsrc.x1 = rect_src_ptr->x1; 
				rsrc.y1 = rect_src_ptr->y1;
				rsrc.x2 = rect_src_ptr->x2 + 1;
				rsrc.y2 = rect_src_ptr->y2 + 1;
			}

			// Version with xdst, ydst (absolute positioning)
			//RectI rdst(xdst, ydst, xdst + rsrc.x2 - rsrc.x1, ydst + rsrc.y2 - rsrc.y1);

			// Version with dx, dy (relative positioning)
			RectI rdst(rsrc.x1 + dx, rsrc.y1 + dy, rsrc.x2 + dx, rsrc.y2 + dy);
			RectI rc = ClipRectangleArea(rdst, rsrc, src.Width(), src.Height());

			if(rc.x2 > 0)
			{
				int incy = 1;
				if(rdst.y1 > rsrc.y1)
				{
					rsrc.y1 += rc.y2 - 1;
					rdst.y1 += rc.y2 - 1;
					incy = -1;
				}
				while(rc.y2 > 0)
				{
					typename SrcPixelFormatRenderer::row_data rw = src.row(rsrc.y1);
					if(rw.ptr)
					{
						int x1src = rsrc.x1;
						int x1dst = rdst.x1;
						int len   = rc.x2;
						if(rw.x1 > x1src)
						{
							x1dst += rw.x1 - x1src;
							len   -= rw.x1 - x1src;
							x1src  = rw.x1;
						}
						if(len > 0)
						{
							if(x1src + len-1 > rw.x2)
							{
								len -= x1src + len - rw.x2 - 1;
							}
							if(len > 0)
							{
								m_ren->blend_from_color(src,
														Color,
														x1dst, rdst.y1,
														x1src, rsrc.y1,
														len,
														cover);
							}
						}
					}
					rdst.y1 += incy;
					rsrc.y1 += incy;
					--rc.y2;
				}
			}
		}

	/*
		//--------------------------------------------------------------------
		//template<class SrcPixelFormatRenderer>
		public void blend_from_lut(rendering_buffer src, IColorType color_lut)
		{
			blend_from_lut(rendering_buffer src, IColorType color_lut, 0, 0, 0, Pictor::CoverFull);
		}
		public void blend_from_lut(rendering_buffer src, 
							IColorType color_lut,
							ref RectI rect_src_ptr, 
							int dx, 
							int dy,
							byte cover)
		{
			RectI rsrc(0, 0, src.Width(), src.Height());
			if(rect_src_ptr)
			{
				rsrc.x1 = rect_src_ptr->x1; 
				rsrc.y1 = rect_src_ptr->y1;
				rsrc.x2 = rect_src_ptr->x2 + 1;
				rsrc.y2 = rect_src_ptr->y2 + 1;
			}

			// Version with xdst, ydst (absolute positioning)
			//RectI rdst(xdst, ydst, xdst + rsrc.x2 - rsrc.x1, ydst + rsrc.y2 - rsrc.y1);

			// Version with dx, dy (relative positioning)
			RectI rdst(rsrc.x1 + dx, rsrc.y1 + dy, rsrc.x2 + dx, rsrc.y2 + dy);
			RectI rc = ClipRectangleArea(rdst, rsrc, src.Width(), src.Height());

			if(rc.x2 > 0)
			{
				int incy = 1;
				if(rdst.y1 > rsrc.y1)
				{
					rsrc.y1 += rc.y2 - 1;
					rdst.y1 += rc.y2 - 1;
					incy = -1;
				}
				while(rc.y2 > 0)
				{
					typename SrcPixelFormatRenderer::row_data rw = src.row(rsrc.y1);
					if(rw.ptr)
					{
						int x1src = rsrc.x1;
						int x1dst = rdst.x1;
						int len   = rc.x2;
						if(rw.x1 > x1src)
						{
							x1dst += rw.x1 - x1src;
							len   -= rw.x1 - x1src;
							x1src  = rw.x1;
						}
						if(len > 0)
						{
							if(x1src + len-1 > rw.x2)
							{
								len -= x1src + len - rw.x2 - 1;
							}
							if(len > 0)
							{
								m_ren->blend_from_lut(src,
													  color_lut,
													  x1dst, rdst.y1,
													  x1src, rsrc.y1,
													  len,
													  cover);
							}
						}
					}
					rdst.y1 += incy;
					rsrc.y1 += incy;
					--rc.y2;
				}
			}
		}
	 */
	}
}
