/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using Pictor.VertexSource;
using filling_rule_e = Pictor.Basics.FillingRule;
using status = Pictor.AntiAliasedScanlineRasterizer.Status;
using poly_subpixel_scale_e = Pictor.Basics.PolySubpixelScale;

namespace Pictor
{
	//==================================================AntiAliasedRasterizerScanline
	// Polygon rasterizer that is used to render filled polygons with 
	// high-quality Anti-Aliasing. Internally, by default, the class uses 
	// integer Coordinates in format 24.8, i.e. 24 bits for integer part 
	// and 8 bits for fractional - see Shift. This class can be 
	// used in the following  way:
	//
	// 1. FillingRule(FillingRule ft) - optional.
	//
	// 2. Gamma() - optional.
	//
	// 3. Reset()
	//
	// 4. MoveTo(x, y) / LineTo(x, y) - make the polygon. One can create 
	//    more than one contour, but each contour must consist of At least 3
	//    vertices, i.e. MoveTo(x1, y1); LineTo(x2, y2); LineTo(x3, y3);
	//    is the absolute minimum of vertices that define a triangle.
	//    The algorithm does not check either the number of vertices nor
	//    coincidence of their Coordinates, but in the worst case it just 
	//    won't draw anything.
	//    The orger of the vertices (clockwise or counterclockwise) 
	//    is important when using the non-zero filling rule (NonZero).
	//    In this case the Vertex order of all the contours must be the same
	//    if you want your intersecting polygons to be without "holes".
	//    You actually can use different vertices order. If the contours do not 
	//    intersect each other the order is not important anyway. If they do, 
	//    contours with the same Vertex order will be rendered without "holes" 
	//    while the intersecting contours with different orders will have "holes".
	//
	// FillingRule() and Gamma() can be called anytime before "sweeping".
	//------------------------------------------------------------------------
	public interface IRasterizer
	{
		int MinX();
		int MinY();
		int MaxX();
		int MaxY();

		IGammaFunction Gamma
		{
			set;
		}

		bool SweepScanline(IScanline sl);
		void Reset();
		void AddPath(IVertexSource vs);
		void AddPath(IVertexSource vs, uint path_id);
		bool RewindScanlines();
	}

	public sealed class AntiAliasedScanlineRasterizer : IRasterizer
	{
		private AntiAliasedRasterizerCells m_outline;
		private IVectorClipper m_VectorClipper;
		private int[] m_gamma = new int[(int)EAntiAliasedScale.aa_scale];
		private Basics.FillingRule m_filling_rule;
		private bool m_auto_close;
		private int m_start_x;
		private int m_start_y;
		private Status m_status;
		private int m_scan_y;

		public enum Status
		{
			Initial,
			MoveTo,
			LineTo,
			Closed
		};

		public enum EAntiAliasedScale
		{
			aa_shift = 8,
			aa_scale = 1 << aa_shift,
			aa_mask = aa_scale - 1,
			aa_scale2 = aa_scale * 2,
			aa_mask2 = aa_scale2 - 1
		};

		public AntiAliasedScanlineRasterizer()
			: this(new VectorClipper_DoClip())
		{
		}

		//--------------------------------------------------------------------
		public AntiAliasedScanlineRasterizer(IVectorClipper rasterizer_sl_clip)
		{
			m_outline = new AntiAliasedRasterizerCells();
			m_VectorClipper = rasterizer_sl_clip;
			m_filling_rule = filling_rule_e.NonZero;
			m_auto_close = true;
			m_start_x = 0;
			m_start_y = 0;
			m_status = Status.Initial;

			int i;
			for (i = 0; i < (int)EAntiAliasedScale.aa_scale; i++) m_gamma[i] = i;
		}

		/*
		//--------------------------------------------------------------------
		public AntiAliasedRasterizerScanline(IClipper rasterizer_sl_clip, IGammaFunction gamma_function)
		{
			m_outline = new AntiAliasedRasterizerCells();
			m_clipper = rasterizer_sl_clip;
			m_filling_rule = FillingRule.NonZero;
			m_auto_close = true;
			m_start_x = 0;
			m_start_y = 0;
			m_status = EStatus.Initial;

			Gamma(gamma_function);
		}*/

		//--------------------------------------------------------------------
		public void Reset()
		{
			m_outline.Reset();
			m_status = Status.Initial;
		}

		public void ResetClipping()
		{
			Reset();
			m_VectorClipper.ResetClipping();
		}

		public void SetVectorClipBox(double x1, double y1, double x2, double y2)
		{
			Reset();
			m_VectorClipper.ClipBox(m_VectorClipper.UpScale(x1), m_VectorClipper.UpScale(y1),
							   m_VectorClipper.UpScale(x2), m_VectorClipper.UpScale(y2));
		}

		public Basics.FillingRule FillingRule
		{
			set { m_filling_rule = value; }
		}

		public bool AutoClose
		{
			set { m_auto_close = value; }
		}

		//--------------------------------------------------------------------
		public IGammaFunction Gamma
		{
			set
			{
				for (int i = 0; i < (int)EAntiAliasedScale.aa_scale; i++)
				{
					m_gamma[i] = (int)Basics.UnsignedRound(value.GetGamma((double)(i) / (int)EAntiAliasedScale.aa_mask) * (int)EAntiAliasedScale.aa_mask);
				}
			}
		}

		/*
		//--------------------------------------------------------------------
		public uint apply_gamma(uint cover) 
		{ 
			return (uint)m_gamma[cover];
		}
		 */

		//--------------------------------------------------------------------
		void MoveTo(int x, int y)
		{
			if (m_outline.IsSorted) Reset();
			if (m_auto_close) ClosePolygon();
			m_VectorClipper.MoveTo(m_start_x = m_VectorClipper.DownScale(x),
							  m_start_y = m_VectorClipper.DownScale(y));
			m_status = Status.MoveTo;
		}

		//------------------------------------------------------------------------
		void LineTo(int x, int y)
		{
			m_VectorClipper.LineTo(m_outline,
							  m_VectorClipper.DownScale(x),
							  m_VectorClipper.DownScale(y));
			m_status = Status.LineTo;
		}

		//------------------------------------------------------------------------
		public void MoveToD(double x, double y)
		{
			if (m_outline.IsSorted) Reset();
			if (m_auto_close) ClosePolygon();
			m_VectorClipper.MoveTo(m_start_x = m_VectorClipper.UpScale(x),
							  m_start_y = m_VectorClipper.UpScale(y));
			m_status = Status.MoveTo;
		}

		//------------------------------------------------------------------------
		public void LineToD(double x, double y)
		{
			m_VectorClipper.LineTo(m_outline,
							  m_VectorClipper.UpScale(x),
							  m_VectorClipper.UpScale(y));
			m_status = Status.LineTo;
			//DebugFile.Print("x=" + x.ToString() + " y=" + y.ToString() + "\n");
		}

		public void ClosePolygon()
		{
			if (m_status == Status.LineTo)
			{
				m_VectorClipper.LineTo(m_outline, m_start_x, m_start_y);
				m_status = Status.Closed;
			}
		}

		void AddVertex(double x, double y, uint PathAndFlags)
		{
			if (Path.IsMoveTo(PathAndFlags))
			{
				MoveToD(x, y);
			}
			else
			{
				if (Path.IsVertex(PathAndFlags))
				{
					LineToD(x, y);
				}
				else
				{
					if (Path.IsClose(PathAndFlags))
					{
						ClosePolygon();
					}
				}
			}
		}
		//------------------------------------------------------------------------
		void Edge(int x1, int y1, int x2, int y2)
		{
			if (m_outline.IsSorted) Reset();
			m_VectorClipper.MoveTo(m_VectorClipper.DownScale(x1), m_VectorClipper.DownScale(y1));
			m_VectorClipper.LineTo(m_outline,
							  m_VectorClipper.DownScale(x2),
							  m_VectorClipper.DownScale(y2));
			m_status = Status.MoveTo;
		}

		//------------------------------------------------------------------------
		void EdgeD(double x1, double y1, double x2, double y2)
		{
			if (m_outline.IsSorted) Reset();
			m_VectorClipper.MoveTo(m_VectorClipper.UpScale(x1), m_VectorClipper.UpScale(y1));
			m_VectorClipper.LineTo(m_outline,
							  m_VectorClipper.UpScale(x2),
							  m_VectorClipper.UpScale(y2));
			m_status = Status.MoveTo;
		}

		//-------------------------------------------------------------------
		public void AddPath(IVertexSource vs)
		{
			AddPath(vs, 0);
		}

		public void AddPath(IVertexSource vs, uint path_id)
		{
			double x = 0;
			double y = 0;

			uint PathAndFlags;
			vs.Rewind(path_id);
			if (m_outline.IsSorted)
			{
				Reset();
			}

			while (!Path.IsStop(PathAndFlags = vs.Vertex(out x, out y)))
			{
				AddVertex(x, y, PathAndFlags);
			}

			//DebugFile.Print("Test");
		}

		//--------------------------------------------------------------------
		public int MinX() { return m_outline.MinX(); }
		public int MinY() { return m_outline.MinY(); }
		public int MaxX() { return m_outline.MaxX(); }
		public int MaxY() { return m_outline.MaxY(); }

		//--------------------------------------------------------------------
		void Sort()
		{
			if (m_auto_close) ClosePolygon();
			m_outline.SortCells();
		}

		//------------------------------------------------------------------------
		public bool RewindScanlines()
		{
			if (m_auto_close) ClosePolygon();
			m_outline.SortCells();
			if (m_outline.TotalCells == 0)
			{
				return false;
			}
			m_scan_y = m_outline.MinY();
			return true;
		}

		//------------------------------------------------------------------------
		bool NavigateScanline(int y)
		{
			if (m_auto_close) ClosePolygon();
			m_outline.SortCells();
			if (m_outline.TotalCells == 0 ||
			   y < m_outline.MinY() ||
			   y > m_outline.MaxY())
			{
				return false;
			}
			m_scan_y = y;
			return true;
		}

		//--------------------------------------------------------------------
		public uint CalculateAlpha(int area)
		{
			int cover = area >> ((int)poly_subpixel_scale_e.Shift * 2 + 1 - (int)EAntiAliasedScale.aa_shift);

			if (cover < 0) cover = -cover;
			if (m_filling_rule == filling_rule_e.EvenOdd)
			{
				cover &= (int)EAntiAliasedScale.aa_mask2;
				if (cover > (int)EAntiAliasedScale.aa_scale)
				{
					cover = (int)EAntiAliasedScale.aa_scale2 - cover;
				}
			}
			if (cover > (int)EAntiAliasedScale.aa_mask) cover = (int)EAntiAliasedScale.aa_mask;
			unsafe
			{
				return (uint)m_gamma[cover];
			}
		}

#if use_timers
        static CNamedTimer SweepSacanLine = new CNamedTimer("SweepSacanLine");
#endif
		//--------------------------------------------------------------------
		public bool SweepScanline(IScanline sl)
		{
#if use_timers
            SweepSacanLine.Start();
#endif
			for (; ; )
			{
				if (m_scan_y > m_outline.MaxY())
				{
#if use_timers
                    SweepSacanLine.Stop();
#endif
					return false;
				}

				sl.ResetSpans();
				uint scan_y_uint = 0; // it is going to Get initialize to 0 anyway so make it Clear.
				if (m_scan_y > 0)
				{
					scan_y_uint = (uint)m_scan_y;
				}
				uint num_cells = m_outline.ScanlineNumCells(scan_y_uint);
				AntiAliasingCell[] cells;
				uint Offset;
				m_outline.ScanlineCells(scan_y_uint, out cells, out Offset);
				int cover = 0;

				while (num_cells != 0)
				{
					AntiAliasingCell cur_cell = cells[Offset];
					int x = cur_cell.x;
					int area = cur_cell.area;
					uint alpha;

					cover += cur_cell.cover;

					//accumulate all cells with the same X
					while (--num_cells != 0)
					{
						Offset++;
						cur_cell = cells[Offset];
						if (cur_cell.x != x)
						{
							break;
						}

						area += cur_cell.area;
						cover += cur_cell.cover;
					}

					if (area != 0)
					{
						alpha = CalculateAlpha((cover << ((int)poly_subpixel_scale_e.Shift + 1)) - area);
						if (alpha != 0)
						{
							sl.AddCell(x, alpha);
						}
						x++;
					}

					if ((num_cells != 0) && (cur_cell.x > x))
					{
						alpha = CalculateAlpha(cover << ((int)poly_subpixel_scale_e.Shift + 1));
						if (alpha != 0)
						{
							sl.AddSpan(x, (cur_cell.x - x), alpha);
						}
					}
				}

				if (sl.NumberOfSpans != 0) break;
				++m_scan_y;
			}

			sl.Finalize(m_scan_y);
			++m_scan_y;
#if use_timers
            SweepSacanLine.Stop();
#endif
			return true;
		}

		//--------------------------------------------------------------------
		bool HitTest(int tx, int ty)
		{
			if (!NavigateScanline(ty)) return false;
			//ScanlineHitTest sl(tx);
			//SweepScanline(sl);
			//return sl.hit();
			return true;
		}
	};
}
