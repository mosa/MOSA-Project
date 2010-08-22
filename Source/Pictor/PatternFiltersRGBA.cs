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
	public interface IPatternFilter
	{
		int Dilation();
		unsafe void PixelHighResolution(RasterBuffer buf, RGBA_Bytes* p, int x, int y);
	}

	//=======================================================pattern_filter_nn
	//template<class ColorT> 
	/*
	struct pattern_filter_nn
	{
		typedef ColorT RGBA_Bytes;
		static uint Dilation() { return 0; }

		static void pixel_low_res(RGBA_Bytes** buf, 
											 RGBA_Bytes* p, int x, int y)
		{
			*p = buf[y][x];
		}

		static void PixelHighResolution(RGBA_Bytes** buf, 
											  RGBA_Bytes* p, int x, int y)
		{
			*p = buf[y >> line_subpixel_shift]
					[x >> line_subpixel_shift];
		}
	};
	 */

	//===========================================pattern_filter_bilinear_rgba
	public struct pattern_filter_bilinear_RGBA_Bytes : IPatternFilter
	{
		public int Dilation() { return 1; }

		public unsafe void pixel_low_res(RGBA_Bytes** buf, RGBA_Bytes* p, int x, int y)
		{
			*p = buf[y][x];
		}

		public unsafe void PixelHighResolution(RasterBuffer buf, RGBA_Bytes* p, int x, int y)
		{
			int r, g, b, a;
			r = g = b = a = LineAABasics.line_subpixel_scale * LineAABasics.line_subpixel_scale / 2;

			int weight;
			int x_lr = x >> LineAABasics.line_subpixel_shift;
			int y_lr = y >> LineAABasics.line_subpixel_shift;

			x &= LineAABasics.line_subpixel_mask;
			y &= LineAABasics.line_subpixel_mask;
			RGBA_Bytes* ptr = (RGBA_Bytes*)buf.GetPixelPointer(x_lr, y_lr);

			weight = (LineAABasics.line_subpixel_scale - x) *
					 (LineAABasics.line_subpixel_scale - y);
			r += weight * ptr->m_R;
			g += weight * ptr->m_G;
			b += weight * ptr->m_B;
			a += weight * ptr->m_A;

			++ptr;

			weight = x * (LineAABasics.line_subpixel_scale - y);
			r += weight * ptr->m_R;
			g += weight * ptr->m_G;
			b += weight * ptr->m_B;
			a += weight * ptr->m_A;

			ptr = (RGBA_Bytes*)buf.GetPixelPointer(x_lr, y_lr + 1);

			weight = (LineAABasics.line_subpixel_scale - x) * y;
			r += weight * ptr->m_R;
			g += weight * ptr->m_G;
			b += weight * ptr->m_B;
			a += weight * ptr->m_A;

			++ptr;

			weight = x * y;
			r += weight * ptr->m_R;
			g += weight * ptr->m_G;
			b += weight * ptr->m_B;
			a += weight * ptr->m_A;

			p->m_R = (byte)(r >> LineAABasics.line_subpixel_shift * 2);
			p->m_G = (byte)(g >> LineAABasics.line_subpixel_shift * 2);
			p->m_B = (byte)(b >> LineAABasics.line_subpixel_shift * 2);
			p->m_A = (byte)(a >> LineAABasics.line_subpixel_shift * 2);
		}
	};
}
