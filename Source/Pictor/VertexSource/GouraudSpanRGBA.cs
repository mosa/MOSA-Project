/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
using System;

namespace Pictor.VertexSource
{

	//=======================================================span_gouraud_rgba
	public sealed class span_gouraud_rgba : span_gouraud, ISpanGenerator
	{
		bool m_swap;
		int m_y2;
		rgba_calc m_rgba1;
		rgba_calc m_rgba2;
		rgba_calc m_rgba3;

		public enum ESubpixelScale
		{
			subpixel_shift = 4,
			subpixel_scale = 1 << subpixel_shift
		};

		//--------------------------------------------------------------------
		public struct rgba_calc
		{
			public void Init(span_gouraud.coord_type c1, span_gouraud.coord_type c2)
			{
				m_x1 = c1.x - 0.5;
				m_y1 = c1.y - 0.5;
				m_dx = c2.x - c1.x;
				double dy = c2.y - c1.y;
				m_1dy = (dy < 1e-5) ? 1e5 : 1.0 / dy;
				m_r1 = (int)c1.color.m_R;
				m_g1 = (int)c1.color.m_G;
				m_b1 = (int)c1.color.m_B;
				m_a1 = (int)c1.color.m_A;
				m_dr = (int)c2.color.m_R - m_r1;
				m_dg = (int)c2.color.m_G - m_g1;
				m_db = (int)c2.color.m_B - m_b1;
				m_da = (int)c2.color.m_A - m_a1;
			}

			public void Calculate(double y)
			{
				double k = (y - m_y1) * m_1dy;
				if (k < 0.0) k = 0.0;
				if (k > 1.0) k = 1.0;
				m_r = m_r1 + Basics.Round(m_dr * k);
				m_g = m_g1 + Basics.Round(m_dg * k);
				m_b = m_b1 + Basics.Round(m_db * k);
				m_a = m_a1 + Basics.Round(m_da * k);
				m_x = Basics.Round((m_x1 + m_dx * k) * (double)ESubpixelScale.subpixel_scale);
			}

			public double m_x1;
			public double m_y1;
			public double m_dx;
			public double m_1dy;
			public int m_r1;
			public int m_g1;
			public int m_b1;
			public int m_a1;
			public int m_dr;
			public int m_dg;
			public int m_db;
			public int m_da;
			public int m_r;
			public int m_g;
			public int m_b;
			public int m_a;
			public int m_x;
		};

		//--------------------------------------------------------------------
		public span_gouraud_rgba() { }
		public span_gouraud_rgba(RGBA_Bytes c1,
						  RGBA_Bytes c2,
						  RGBA_Bytes c3,
						  double x1, double y1,
						  double x2, double y2,
						  double x3, double y3)
			: this(c1, c2, c3, x1, y1, x2, y2, x3, y3, 0)
		{ }

		public span_gouraud_rgba(RGBA_Bytes c1,
						  RGBA_Bytes c2,
						  RGBA_Bytes c3,
						  double x1, double y1,
						  double x2, double y2,
						  double x3, double y3,
						  double d)
			: base(c1, c2, c3, x1, y1, x2, y2, x3, y3, d)
		{ }

		//--------------------------------------------------------------------
		public void Prepare()
		{
			unsafe
			{
				coord_type[] coord = new coord_type[3];
				base.arrange_vertices(coord);

				m_y2 = (int)(coord[1].y);

				m_swap = PictorMath.CrossProduct(coord[0].x, coord[0].y,
									   coord[2].x, coord[2].y,
									   coord[1].x, coord[1].y) < 0.0;

				m_rgba1.Init(coord[0], coord[2]);
				m_rgba2.Init(coord[0], coord[1]);
				m_rgba3.Init(coord[1], coord[2]);
			}
		}

		//--------------------------------------------------------------------
		unsafe public void Generate(RGBA_Bytes* span, int x, int y, uint len)
		{
			m_rgba1.Calculate(y);//(m_rgba1.m_1dy > 2) ? m_rgba1.m_y1 : y);
			rgba_calc pc1 = m_rgba1;
			rgba_calc pc2 = m_rgba2;

			if (y <= m_y2)
			{
				// Bottom part of the triangle (first subtriangle)
				//-------------------------
				m_rgba2.Calculate(y + m_rgba2.m_1dy);
			}
			else
			{
				// Upper part (second subtriangle)
				m_rgba3.Calculate(y - m_rgba3.m_1dy);
				//-------------------------
				pc2 = m_rgba3;
			}

			if (m_swap)
			{
				// It means that the triangle is oriented clockwise, 
				// so that we need to swap the controlling structures
				//-------------------------
				rgba_calc t = pc2;
				pc2 = pc1;
				pc1 = t;
			}

			// Get the horizontal length with subpixel accuracy
			// and protect it from division by zero
			//-------------------------
			int nlen = Math.Abs(pc2.m_x - pc1.m_x);
			if (nlen <= 0) nlen = 1;

			DdaLineInterpolator r = new DdaLineInterpolator(pc1.m_r, pc2.m_r, (uint)nlen, 14);
			DdaLineInterpolator g = new DdaLineInterpolator(pc1.m_g, pc2.m_g, (uint)nlen, 14);
			DdaLineInterpolator b = new DdaLineInterpolator(pc1.m_b, pc2.m_b, (uint)nlen, 14);
			DdaLineInterpolator a = new DdaLineInterpolator(pc1.m_a, pc2.m_a, (uint)nlen, 14);

			// Calculate the starting point of the Gradient with subpixel 
			// accuracy and correct (roll back) the interpolators.
			// This operation will also Clip the beginning of the Span
			// if necessary.
			//-------------------------
			int start = pc1.m_x - (x << (int)ESubpixelScale.subpixel_shift);
			r.Prev(start);
			g.Prev(start);
			b.Prev(start);
			a.Prev(start);
			nlen += start;

			int vr, vg, vb, va;
			uint lim = 255;

			// Beginning part of the Span. Since we rolled back the 
			// interpolators, the Color values may have overflowed.
			// So that, we render the beginning part with checking 
			// for overflow. It lasts until "Start" is positive;
			// typically it's 1-2 pixels, but may be more in some cases.
			//-------------------------
			while (len != 0 && start > 0)
			{
				vr = r.y();
				vg = g.y();
				vb = b.y();
				va = a.y();
				if (vr < 0) vr = 0; if (vr > lim) vr = (int)lim;
				if (vg < 0) vg = 0; if (vg > lim) vg = (int)lim;
				if (vb < 0) vb = 0; if (vb > lim) vb = (int)lim;
				if (va < 0) va = 0; if (va > lim) va = (int)lim;
				span[0].m_R = (byte)vr;
				span[0].m_G = (byte)vg;
				span[0].m_B = (byte)vb;
				span[0].m_A = (byte)va;
				r.Next((int)ESubpixelScale.subpixel_scale);
				g.Next((int)ESubpixelScale.subpixel_scale);
				b.Next((int)ESubpixelScale.subpixel_scale);
				a.Next((int)ESubpixelScale.subpixel_scale);
				nlen -= (int)ESubpixelScale.subpixel_scale;
				start -= (int)ESubpixelScale.subpixel_scale;
				++span;
				--len;
			}

			// Middle part, no checking for overflow.
			// Actual spans can be longer than the calculated length
			// because of anti-aliasing, thus, the interpolators can 
			// overflow. But while "nlen" is positive we are safe.
			//-------------------------
			while (len != 0 && nlen > 0)
			{
				span[0].m_R = (byte)r.y();
				span[0].m_G = (byte)g.y();
				span[0].m_B = (byte)b.y();
				span[0].m_A = (byte)a.y();
				r.Next((int)ESubpixelScale.subpixel_scale);
				g.Next((int)ESubpixelScale.subpixel_scale);
				b.Next((int)ESubpixelScale.subpixel_scale);
				a.Next((int)ESubpixelScale.subpixel_scale);
				nlen -= (int)ESubpixelScale.subpixel_scale;
				++span;
				--len;
			}

			// Ending part; checking for overflow.
			// Typically it's 1-2 pixels, but may be more in some cases.
			//-------------------------
			while (len != 0)
			{
				vr = r.y();
				vg = g.y();
				vb = b.y();
				va = a.y();
				if (vr < 0) vr = 0; if (vr > lim) vr = (int)lim;
				if (vg < 0) vg = 0; if (vg > lim) vg = (int)lim;
				if (vb < 0) vb = 0; if (vb > lim) vb = (int)lim;
				if (va < 0) va = 0; if (va > lim) va = (int)lim;
				span[0].m_R = (byte)vr;
				span[0].m_G = (byte)vg;
				span[0].m_B = (byte)vb;
				span[0].m_A = (byte)va;
				r.Next((int)ESubpixelScale.subpixel_scale);
				g.Next((int)ESubpixelScale.subpixel_scale);
				b.Next((int)ESubpixelScale.subpixel_scale);
				a.Next((int)ESubpixelScale.subpixel_scale);
				++span;
				--len;
			}
		}
	};
}
