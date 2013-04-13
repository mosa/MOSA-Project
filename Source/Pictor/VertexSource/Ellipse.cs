/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using path_commands_e = Pictor.Path.EPathCommands;
using path_flags_e = Pictor.Path.EPathFlags;

namespace Pictor.VertexSource
{
	//----------------------------------------------------------------ellipse
	public class Ellipse : IVertexSource
	{
		public double m_x;
		public double m_y;
		public double m_rx;
		public double m_ry;
		private double m_scale;
		private uint m_num;
		private uint m_step;
		private bool m_cw;

		public Ellipse()
		{
			m_x = 0.0;
			m_y = 0.0;
			m_rx = 1.0;
			m_ry = 1.0;
			m_scale = 1.0;
			m_num = 4;
			m_step = 0;
			m_cw = false;
		}

		public Ellipse(double OriginX, double OriginY, double RadiusX, double RadiusY)
			: this(OriginX, OriginY, RadiusX, RadiusY, 0, false)
		{
		}

		public Ellipse(double OriginX, double OriginY, double RadiusX, double RadiusY, uint num_steps)
			: this(OriginX, OriginY, RadiusX, RadiusY, num_steps, false)
		{
		}

		public Ellipse(double OriginX, double OriginY, double RadiusX, double RadiusY,
				uint num_steps, bool cw)
		{
			m_x = OriginX;
			m_y = OriginY;
			m_rx = RadiusX;
			m_ry = RadiusY;
			m_scale = 1;
			m_num = num_steps;
			m_step = 0;
			m_cw = cw;
			if (m_num == 0) CalculateNumberOfSteps();
		}

		public void Init(double OriginX, double OriginY, double RadiusX, double RadiusY)
		{
			Init(OriginX, OriginY, RadiusX, RadiusY, 0, false);
		}

		public void Init(double OriginX, double OriginY, double RadiusX, double RadiusY, uint num_steps)
		{
			Init(OriginX, OriginY, RadiusX, RadiusY, num_steps, false);
		}

		public void Init(double OriginX, double OriginY, double RadiusX, double RadiusY,
				  uint num_steps, bool cw)
		{
			m_x = OriginX;
			m_y = OriginY;
			m_rx = RadiusX;
			m_ry = RadiusY;
			m_num = num_steps;
			m_step = 0;
			m_cw = cw;
			if (m_num == 0) CalculateNumberOfSteps();
		}

		public double ApproximationScale
		{
			get
			{
				return m_scale;
			}
			set
			{
				m_scale = value;
				CalculateNumberOfSteps();
			}
		}

		public void Rewind(uint path_id)
		{
			m_step = 0;
		}

		public uint Vertex(out double x, out double y)
		{
			x = 0;
			y = 0;
			if (m_step == m_num)
			{
				++m_step;
				return (int)path_commands_e.EndPoly | (int)path_flags_e.Close | (int)path_flags_e.CounterClockwise;
			}
			if (m_step > m_num) return (uint)path_commands_e.Stop;
			double angle = (double)(m_step) / (double)(m_num) * 2.0 * Math.PI;
			if (m_cw) angle = 2.0 * Math.PI - angle;
			x = m_x + Math.Cos(angle) * m_rx;
			y = m_y + Math.Sin(angle) * m_ry;
			m_step++;
			return ((m_step == 1) ? (uint)path_commands_e.MoveTo : (uint)path_commands_e.LineTo);
		}

		private void CalculateNumberOfSteps()
		{
			double ra = (Math.Abs(m_rx) + Math.Abs(m_ry)) / 2;
			double da = Math.Acos(ra / (ra + 0.125 / m_scale)) * 2;
			m_num = (uint)Math.Round(2 * Math.PI / da);
		}
	};
}

//#endif