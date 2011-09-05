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
	//------------------------------------------------------------MathStroke
	public class MathStroke
	{
		double m_width;
		double m_width_abs;
		double m_width_eps;
		int m_width_sign;
		double m_miter_limit;
		double m_inner_miter_limit;
		double m_approx_scale;
		ELineCap m_line_cap;
		ELineJoin m_line_join;
		EInnerJoin m_inner_join;

		//-------------------------------------------------------------ELineCap
		public enum ELineCap
		{
			butt_cap,
			square_cap,
			round_cap
		};

		//------------------------------------------------------------ELineJoin
		public enum ELineJoin
		{
			miter_join = 0,
			miter_join_revert = 1,
			round_join = 2,
			bevel_join = 3,
			miter_join_round = 4
		};


		//-----------------------------------------------------------EInnerJoin
		public enum EInnerJoin
		{
			inner_bevel,
			inner_miter,
			inner_jag,
			inner_round
		};

		public MathStroke()
		{
			m_width = 0.5;
			m_width_abs = 0.5;
			m_width_eps = 0.5 / 1024.0;
			m_width_sign = 1;
			m_miter_limit = 4.0;
			m_inner_miter_limit = 1.01;
			m_approx_scale = 1.0;
			m_line_cap = ELineCap.butt_cap;
			m_line_join = ELineJoin.miter_join;
			m_inner_join = EInnerJoin.inner_miter;
		}

		public void LineCap(ELineCap lc) { m_line_cap = lc; }
		public void LineJoin(ELineJoin lj) { m_line_join = lj; }
		public void InnerJoin(EInnerJoin ij) { m_inner_join = ij; }

		public ELineCap LineCap() { return m_line_cap; }
		public ELineJoin LineJoin() { return m_line_join; }
		public EInnerJoin InnerJoin() { return m_inner_join; }

		public double MiterLimitTheta
		{
			set { m_miter_limit = 1.0 / Math.Sin(value * 0.5); }
		}

		public double Width
		{
			get { return m_width * 2.0; }
			set
			{
				m_width = value * 0.5;
				if (m_width < 0)
				{
					m_width_abs = -m_width;
					m_width_sign = -1;
				}
				else
				{
					m_width_abs = m_width;
					m_width_sign = 1;
				}
				m_width_eps = m_width / 1024.0;
			}
		}
		public double MiterLimit
		{
			get { return m_miter_limit; }
			set { m_miter_limit = value; }
		}
		public double InnerMiterLimit
		{
			get { return m_inner_miter_limit; }
			set { m_inner_miter_limit = value; }
		}
		public double ApproximationScale
		{
			get { return m_approx_scale; }
			set { m_approx_scale = value; }
		}

		public void CaluclateCap(IVertexDest vc, VertexDistance v0, VertexDistance v1, double len)
		{
			vc.RemoveAll();

			double dx1 = (v1.y - v0.y) / len;
			double dy1 = (v1.x - v0.x) / len;
			double dx2 = 0;
			double dy2 = 0;

			dx1 *= m_width;
			dy1 *= m_width;

			if (m_line_cap != ELineCap.round_cap)
			{
				if (m_line_cap == ELineCap.square_cap)
				{
					dx2 = dy1 * m_width_sign;
					dy2 = dx1 * m_width_sign;
				}
				AddVertex(vc, v0.x - dx1 - dx2, v0.y + dy1 - dy2);
				AddVertex(vc, v0.x + dx1 - dx2, v0.y - dy1 - dy2);
			}
			else
			{
				double da = Math.Acos(m_width_abs / (m_width_abs + 0.125 / m_approx_scale)) * 2;
				double a1;
				int i;
				int n = (int)(Math.PI / da);

				da = Math.PI / (n + 1);
				AddVertex(vc, v0.x - dx1, v0.y + dy1);
				if (m_width_sign > 0)
				{
					a1 = Math.Atan2(dy1, -dx1);
					a1 += da;
					for (i = 0; i < n; i++)
					{
						AddVertex(vc, v0.x + Math.Cos(a1) * m_width,
									   v0.y + Math.Sin(a1) * m_width);
						a1 += da;
					}
				}
				else
				{
					a1 = Math.Atan2(-dy1, dx1);
					a1 -= da;
					for (i = 0; i < n; i++)
					{
						AddVertex(vc, v0.x + Math.Cos(a1) * m_width,
									   v0.y + Math.Sin(a1) * m_width);
						a1 -= da;
					}
				}
				AddVertex(vc, v0.x + dx1, v0.y - dy1);
			}
		}

		public void CalculateJoin(IVertexDest vc, VertexDistance v0,
										VertexDistance v1,
										VertexDistance v2,
										double len1,
										double len2)
		{
			double dx1 = m_width * (v1.y - v0.y) / len1;
			double dy1 = m_width * (v1.x - v0.x) / len1;
			double dx2 = m_width * (v2.y - v1.y) / len2;
			double dy2 = m_width * (v2.x - v1.x) / len2;

			vc.RemoveAll();

			double cp = PictorMath.CrossProduct(v0.x, v0.y, v1.x, v1.y, v2.x, v2.y);
			if (cp != 0 && (cp > 0) == (m_width > 0))
			{
				// Inner join
				//---------------
				double limit = ((len1 < len2) ? len1 : len2) / m_width_abs;
				if (limit < m_inner_miter_limit)
				{
					limit = m_inner_miter_limit;
				}

				switch (m_inner_join)
				{
					default: // inner_bevel
						AddVertex(vc, v1.x + dx1, v1.y - dy1);
						AddVertex(vc, v1.x + dx2, v1.y - dy2);
						break;

					case EInnerJoin.inner_miter:
						CalculateMiter(vc,
								   v0, v1, v2, dx1, dy1, dx2, dy2,
								   ELineJoin.miter_join_revert,
								   limit, 0);
						break;

					case EInnerJoin.inner_jag:
					case EInnerJoin.inner_round:
						cp = (dx1 - dx2) * (dx1 - dx2) + (dy1 - dy2) * (dy1 - dy2);
						if (cp < len1 * len1 && cp < len2 * len2)
						{
							CalculateMiter(vc,
									   v0, v1, v2, dx1, dy1, dx2, dy2,
									   ELineJoin.miter_join_revert,
									   limit, 0);
						}
						else
						{
							if (m_inner_join == EInnerJoin.inner_jag)
							{
								AddVertex(vc, v1.x + dx1, v1.y - dy1);
								AddVertex(vc, v1.x, v1.y);
								AddVertex(vc, v1.x + dx2, v1.y - dy2);
							}
							else
							{
								AddVertex(vc, v1.x + dx1, v1.y - dy1);
								AddVertex(vc, v1.x, v1.y);
								CalculateArc(vc, v1.x, v1.y, dx2, -dy2, dx1, -dy1);
								AddVertex(vc, v1.x, v1.y);
								AddVertex(vc, v1.x + dx2, v1.y - dy2);
							}
						}
						break;
				}
			}
			else
			{
				// Outer join
				//---------------

				// Calculate the distance between v1 and 
				// the central point of the bevel Line segment
				//---------------
				double dx = (dx1 + dx2) / 2;
				double dy = (dy1 + dy2) / 2;
				double dbevel = Math.Sqrt(dx * dx + dy * dy);

				if (m_line_join == ELineJoin.round_join || m_line_join == ELineJoin.bevel_join)
				{
					// This is an optimization that reduces the number of points 
					// in cases of almost collinear segments. If there's no
					// visible difference between bevel and miter joins we'd rather
					// use miter join because it adds only one point instead of two. 
					//
					// Here we Calculate the middle point between the bevel points 
					// and then, the distance between v1 and this middle point. 
					// At outer joins this distance always less than stroke Width, 
					// because it's actually the Height of an isosceles triangle of
					// v1 and its two bevel points. If the difference between this
					// Width and this Value is small (no visible bevel) we can 
					// Add just one point. 
					//
					// The constant in the expression makes the result approximately 
					// the same as in round joins and caps. You can safely comment 
					// out this entire "if".
					//-------------------
					if (m_approx_scale * (m_width_abs - dbevel) < m_width_eps)
					{
						if (PictorMath.CalculateIntersection(v0.x + dx1, v0.y - dy1,
											 v1.x + dx1, v1.y - dy1,
											 v1.x + dx2, v1.y - dy2,
											 v2.x + dx2, v2.y - dy2,
											 out dx, out dy))
						{
							AddVertex(vc, dx, dy);
						}
						else
						{
							AddVertex(vc, v1.x + dx1, v1.y - dy1);
						}
						return;
					}
				}

				switch (m_line_join)
				{
					case ELineJoin.miter_join:
					case ELineJoin.miter_join_revert:
					case ELineJoin.miter_join_round:
						CalculateMiter(vc,
								   v0, v1, v2, dx1, dy1, dx2, dy2,
								   m_line_join,
								   m_miter_limit,
								   dbevel);
						break;

					case ELineJoin.round_join:
						CalculateArc(vc, v1.x, v1.y, dx1, -dy1, dx2, -dy2);
						break;

					default: // Bevel join
						AddVertex(vc, v1.x + dx1, v1.y - dy1);
						AddVertex(vc, v1.x + dx2, v1.y - dy2);
						break;
				}
			}
		}

		private void AddVertex(IVertexDest vc, double x, double y)
		{
			vc.Add(new PointD(x, y));
		}

		void CalculateArc(IVertexDest vc,
					  double x, double y,
					  double dx1, double dy1,
					  double dx2, double dy2)
		{
			double a1 = Math.Atan2(dy1 * m_width_sign, dx1 * m_width_sign);
			double a2 = Math.Atan2(dy2 * m_width_sign, dx2 * m_width_sign);
			double da = a1 - a2;
			int i, n;

			da = Math.Acos(m_width_abs / (m_width_abs + 0.125 / m_approx_scale)) * 2;

			AddVertex(vc, x + dx1, y + dy1);
			if (m_width_sign > 0)
			{
				if (a1 > a2) a2 += 2 * Math.PI;
				n = (int)((a2 - a1) / da);
				da = (a2 - a1) / (n + 1);
				a1 += da;
				for (i = 0; i < n; i++)
				{
					AddVertex(vc, x + Math.Cos(a1) * m_width, y + Math.Sin(a1) * m_width);
					a1 += da;
				}
			}
			else
			{
				if (a1 < a2) a2 -= 2 * Math.PI;
				n = (int)((a1 - a2) / da);
				da = (a1 - a2) / (n + 1);
				a1 -= da;
				for (i = 0; i < n; i++)
				{
					AddVertex(vc, x + Math.Cos(a1) * m_width, y + Math.Sin(a1) * m_width);
					a1 -= da;
				}
			}
			AddVertex(vc, x + dx2, y + dy2);
		}

		void CalculateMiter(IVertexDest vc,
						VertexDistance v0,
						VertexDistance v1,
						VertexDistance v2,
						double dx1, double dy1,
						double dx2, double dy2,
						ELineJoin lj,
						double mlimit,
						double dbevel)
		{
			double xi = v1.x;
			double yi = v1.y;
			double di = 1;
			double lim = m_width_abs * mlimit;
			bool miter_limit_exceeded = true; // Assume the worst
			bool intersection_failed = true; // Assume the worst

			if (PictorMath.CalculateIntersection(v0.x + dx1, v0.y - dy1,
								 v1.x + dx1, v1.y - dy1,
								 v1.x + dx2, v1.y - dy2,
								 v2.x + dx2, v2.y - dy2,
								 out xi, out yi))
			{
				// Calculation of the intersection succeeded
				//---------------------
				di = PictorMath.CalculateDistance(v1.x, v1.y, xi, yi);
				if (di <= lim)
				{
					// Inside the miter limit
					//---------------------
					AddVertex(vc, xi, yi);
					miter_limit_exceeded = false;
				}
				intersection_failed = false;
			}
			else
			{
				// Calculation of the intersection failed, most probably
				// the three points lie one straight Line. 
				// First check if v0 and v2 lie on the opposite sides of vector: 
				// (v1.x, v1.y) -> (v1.x+dx1, v1.y-dy1), that is, the perpendicular
				// to the Line determined by vertices v0 and v1.
				// This condition determines whether the next Line segments continues
				// the previous one or goes back.
				//----------------
				double x2 = v1.x + dx1;
				double y2 = v1.y - dy1;
				if ((PictorMath.CrossProduct(v0.x, v0.y, v1.x, v1.y, x2, y2) < 0.0) ==
				   (PictorMath.CrossProduct(v1.x, v1.y, v2.x, v2.y, x2, y2) < 0.0))
				{
					// This case means that the next segment continues 
					// the previous one (straight Line)
					//-----------------
					AddVertex(vc, v1.x + dx1, v1.y - dy1);
					miter_limit_exceeded = false;
				}
			}

			if (miter_limit_exceeded)
			{
				// Miter limit exceeded
				//------------------------
				switch (lj)
				{
					case ELineJoin.miter_join_revert:
						// For the compatibility with SVG, PDF, etc, 
						// we use a simple bevel join instead of
						// "smart" bevel
						//-------------------
						AddVertex(vc, v1.x + dx1, v1.y - dy1);
						AddVertex(vc, v1.x + dx2, v1.y - dy2);
						break;

					case ELineJoin.miter_join_round:
						CalculateArc(vc, v1.x, v1.y, dx1, -dy1, dx2, -dy2);
						break;

					default:
						// If no miter-revert, Calculate new dx1, dy1, dx2, dy2
						//----------------
						if (intersection_failed)
						{
							mlimit *= m_width_sign;
							AddVertex(vc, v1.x + dx1 + dy1 * mlimit,
										   v1.y - dy1 + dx1 * mlimit);
							AddVertex(vc, v1.x + dx2 - dy2 * mlimit,
										   v1.y - dy2 - dx2 * mlimit);
						}
						else
						{
							double x1 = v1.x + dx1;
							double y1 = v1.y - dy1;
							double x2 = v1.x + dx2;
							double y2 = v1.y - dy2;
							di = (lim - dbevel) / (di - dbevel);
							AddVertex(vc, x1 + (xi - x1) * di,
										   y1 + (yi - y1) * di);
							AddVertex(vc, x2 + (xi - x2) * di,
										   y2 + (yi - y2) * di);
						}
						break;
				}
			}
		}
	};
}