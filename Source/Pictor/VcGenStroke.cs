/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Pictor.VertexSource
{
	internal class point_d_vector : VectorPOD<PointD>, IVertexDest
	{
		public override void RemoveLast()
		{
			base.RemoveLast();
		}

		public override void Add(PointD v)
		{
			base.Add(v);
		}

		new public void RemoveAll()
		{
			base.RemoveAll();
		}
	};

	//============================================================VcGenStroke
	internal class VcGenStroke : IGenerator
	{
		private MathStroke m_stroker;

		private vertex_sequence m_src_vertices;
		private point_d_vector m_out_vertices;

		private double m_shorten;
		private uint m_closed;
		private status_e m_status;
		private status_e m_prev_status;

		private uint m_src_vertex;
		private uint m_out_vertex;

		private enum status_e
		{
			initial,
			ready,
			cap1,
			cap2,
			outline1,
			close_first,
			outline2,
			out_vertices,
			end_poly1,
			end_poly2,
			stop
		};

		public VcGenStroke()
		{
			m_stroker = new MathStroke();
			m_src_vertices = new vertex_sequence();
			m_out_vertices = new point_d_vector();
			m_status = status_e.initial;
		}

		public void LineCap(MathStroke.ELineCap lc)
		{
			m_stroker.LineCap(lc);
		}

		public void LineJoin(MathStroke.ELineJoin lj)
		{
			m_stroker.LineJoin(lj);
		}

		public void InnerJoin(MathStroke.EInnerJoin ij)
		{
			m_stroker.InnerJoin(ij);
		}

		public MathStroke.ELineCap LineCap()
		{
			return m_stroker.LineCap();
		}

		public MathStroke.ELineJoin LineJoin()
		{
			return m_stroker.LineJoin();
		}

		public MathStroke.EInnerJoin InnerJoin()
		{
			return m_stroker.InnerJoin();
		}

		public void Width(double w)
		{
			m_stroker.Width = w;
		}

		public void MiterLimit(double ml)
		{
			m_stroker.MiterLimit = ml;
		}

		public void MiterLimitTheta(double t)
		{
			m_stroker.MiterLimitTheta = t;
		}

		public void InnerMiterLimit(double ml)
		{
			m_stroker.InnerMiterLimit = ml;
		}

		public void ApproximationScale(double approx_scale)
		{
			m_stroker.ApproximationScale = approx_scale;
		}

		public double Width()
		{
			return m_stroker.Width;
		}

		public double MiterLimit()
		{
			return m_stroker.MiterLimit;
		}

		public double InnerMiterLimit()
		{
			return m_stroker.InnerMiterLimit;
		}

		public double ApproximationScale()
		{
			return m_stroker.ApproximationScale;
		}

		public void Shorten(double s)
		{
			m_shorten = s;
		}

		public double Shorten()
		{
			return m_shorten;
		}

		// Vertex Generator Interface
		public void RemoveAll()
		{
			m_src_vertices.RemoveAll();
			m_closed = 0;
			m_status = status_e.initial;
		}

		public void AddVertex(double x, double y, uint cmd)
		{
			m_status = status_e.initial;
			if (Path.IsMoveTo(cmd))
			{
				m_src_vertices.modify_last(new VertexDistance(x, y));
			}
			else
			{
				if (Path.IsVertex(cmd))
				{
					m_src_vertices.Add(new VertexDistance(x, y));
				}
				else
				{
					m_closed = (uint)Path.GetCloseFlag(cmd);
				}
			}
		}

		// Vertex Source Interface
		public void Rewind(uint idx)
		{
			if (m_status == status_e.initial)
			{
				m_src_vertices.close(m_closed != 0);
				Path.ShortenPath(m_src_vertices, m_shorten, m_closed);
				if (m_src_vertices.Size() < 3) m_closed = 0;
			}
			m_status = status_e.ready;
			m_src_vertex = 0;
			m_out_vertex = 0;
		}

		public uint Vertex(ref double x, ref double y)
		{
			uint cmd = (uint)Path.EPathCommands.LineTo;
			while (!Path.IsStop(cmd))
			{
				switch (m_status)
				{
					case status_e.initial:
						Rewind(0);
						goto case status_e.ready;

					case status_e.ready:
						if (m_src_vertices.Size() < 2 + (m_closed != 0 ? 1 : 0))
						{
							cmd = (uint)Path.EPathCommands.Stop;
							break;
						}
						m_status = (m_closed != 0) ? VcGenStroke.status_e.outline1 : VcGenStroke.status_e.cap1;
						cmd = (uint)Path.EPathCommands.MoveTo;
						m_src_vertex = 0;
						m_out_vertex = 0;
						break;

					case status_e.cap1:
						m_stroker.CaluclateCap(m_out_vertices, m_src_vertices[0], m_src_vertices[1],
							m_src_vertices[0].dist);
						m_src_vertex = 1;
						m_prev_status = VcGenStroke.status_e.outline1;
						m_status = VcGenStroke.status_e.out_vertices;
						m_out_vertex = 0;
						break;

					case status_e.cap2:
						m_stroker.CaluclateCap(m_out_vertices,
							m_src_vertices[m_src_vertices.Size() - 1],
							m_src_vertices[m_src_vertices.Size() - 2],
							m_src_vertices[m_src_vertices.Size() - 2].dist);
						m_prev_status = VcGenStroke.status_e.outline2;
						m_status = VcGenStroke.status_e.out_vertices;
						m_out_vertex = 0;
						break;

					case status_e.outline1:
						if (m_closed != 0)
						{
							if (m_src_vertex >= m_src_vertices.Size())
							{
								m_prev_status = VcGenStroke.status_e.close_first;
								m_status = VcGenStroke.status_e.end_poly1;
								break;
							}
						}
						else
						{
							if (m_src_vertex >= m_src_vertices.Size() - 1)
							{
								m_status = VcGenStroke.status_e.cap2;
								break;
							}
						}
						m_stroker.CalculateJoin(m_out_vertices,
							m_src_vertices.prev(m_src_vertex),
							m_src_vertices.curr(m_src_vertex),
							m_src_vertices.next(m_src_vertex),
							m_src_vertices.prev(m_src_vertex).dist,
							m_src_vertices.curr(m_src_vertex).dist);
						++m_src_vertex;
						m_prev_status = m_status;
						m_status = VcGenStroke.status_e.out_vertices;
						m_out_vertex = 0;
						break;

					case status_e.close_first:
						m_status = VcGenStroke.status_e.outline2;
						cmd = (uint)Path.EPathCommands.MoveTo;
						goto case status_e.outline2;

					case status_e.outline2:
						if (m_src_vertex <= (m_closed == 0 ? 1 : 0))
						{
							m_status = VcGenStroke.status_e.end_poly2;
							m_prev_status = VcGenStroke.status_e.stop;
							break;
						}

						--m_src_vertex;
						m_stroker.CalculateJoin(m_out_vertices,
							m_src_vertices.next(m_src_vertex),
							m_src_vertices.curr(m_src_vertex),
							m_src_vertices.prev(m_src_vertex),
							m_src_vertices.curr(m_src_vertex).dist,
							m_src_vertices.prev(m_src_vertex).dist);

						m_prev_status = m_status;
						m_status = VcGenStroke.status_e.out_vertices;
						m_out_vertex = 0;
						break;

					case status_e.out_vertices:
						if (m_out_vertex >= m_out_vertices.Size())
						{
							m_status = m_prev_status;
						}
						else
						{
							PointD c = m_out_vertices[(int)m_out_vertex++];
							x = c.x;
							y = c.y;
							return cmd;
						}
						break;

					case status_e.end_poly1:
						m_status = m_prev_status;
						return (uint)Path.EPathCommands.EndPoly
							| (uint)Path.EPathFlags.Close
							| (uint)Path.EPathFlags.CounterClockwise;

					case status_e.end_poly2:
						m_status = m_prev_status;
						return (uint)Path.EPathCommands.EndPoly
							| (uint)Path.EPathFlags.Close
							| (uint)Path.EPathFlags.Clockwise;

					case status_e.stop:
						cmd = (uint)Path.EPathCommands.Stop;
						break;
				}
			}
			return cmd;
		}
	};
}