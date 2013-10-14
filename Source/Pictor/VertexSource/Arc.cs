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
	//=====================================================================Arc
	//
	// See Implementation agg_arc.cpp
	//
	public class Arc
	{
		private double m_OriginX;
		private double m_OriginY;

		private double m_RadiusX;
		private double m_RadiusY;

		private double m_StartAngle;
		private double m_EndAngle;
		private double m_Scale;
		private EDirection m_Direction;

		private double m_CurrentFlatenAngle;
		private double m_FlatenDeltaAngle;

		private bool m_IsInitialized;
		private uint m_NextPathCommand;

		public enum EDirection
		{
			ClockWise,
			CounterClockWise,
		}

		public Arc()
		{
			m_Scale = 1.0;
			m_IsInitialized = false;
		}

		public Arc(double OriginX, double OriginY,
			 double RadiusX, double RadiusY,
			 double Angle1, double Angle2)
			: this(OriginX, OriginY, RadiusX, RadiusY, Angle1, Angle2, EDirection.CounterClockWise)
		{
		}

		public Arc(double OriginX, double OriginY,
			 double RadiusX, double RadiusY,
			 double Angle1, double Angle2,
			 EDirection Direction)
		{
			m_OriginX = OriginX;
			m_OriginY = OriginY;
			m_RadiusX = RadiusX;
			m_RadiusY = RadiusY;
			m_Scale = 1.0;
			Normalize(Angle1, Angle2, Direction);
		}

		public void Init(double OriginX, double OriginY,
				  double RadiusX, double RadiusY,
				  double Angle1, double Angle2)
		{
			Init(OriginX, OriginY, RadiusX, RadiusY, Angle1, Angle2, EDirection.CounterClockWise);
		}

		public void Init(double OriginX, double OriginY,
				   double RadiusX, double RadiusY,
				   double Angle1, double Angle2,
				   EDirection Direction)
		{
			m_OriginX = OriginX;
			m_OriginY = OriginY;
			m_RadiusX = RadiusX;
			m_RadiusY = RadiusY;
			Normalize(Angle1, Angle2, Direction);
		}

		public double ApproximationScale
		{
			get { return m_Scale; }
			set
			{
				m_Scale = value;
				if (m_IsInitialized)
				{
					Normalize(m_StartAngle, m_EndAngle, m_Direction);
				}
			}
		}

		public void Rewind(uint unused)
		{
			m_NextPathCommand = (uint)Path.EPathCommands.MoveTo;
			m_CurrentFlatenAngle = m_StartAngle;
		}

		public uint Vertex(out double x, out double y)
		{
			x = 0;
			y = 0;

			if (Path.IsStop(m_NextPathCommand))
			{
				return (uint)Path.EPathCommands.Stop;
			}

			if ((m_CurrentFlatenAngle < m_EndAngle - m_FlatenDeltaAngle / 4) != ((int)EDirection.CounterClockWise == 1))
			{
				x = m_OriginX + Math.Cos(m_EndAngle) * m_RadiusX;
				y = m_OriginY + Math.Sin(m_EndAngle) * m_RadiusY;
				m_NextPathCommand = (uint)Path.EPathCommands.Stop;

				return (uint)Path.EPathCommands.LineTo;
			}

			x = m_OriginX + Math.Cos(m_CurrentFlatenAngle) * m_RadiusX;
			y = m_OriginY + Math.Sin(m_CurrentFlatenAngle) * m_RadiusY;

			m_CurrentFlatenAngle += m_FlatenDeltaAngle;

			uint CurrentPathCommand = m_NextPathCommand;
			m_NextPathCommand = (uint)Path.EPathCommands.LineTo;
			return CurrentPathCommand;
		}

		private void Normalize(double Angle1, double Angle2, EDirection Direction)
		{
			double ra = (Math.Abs(m_RadiusX) + Math.Abs(m_RadiusY)) / 2;
			m_FlatenDeltaAngle = Math.Acos(ra / (ra + 0.125 / m_Scale)) * 2;
			if (Direction == EDirection.CounterClockWise)
			{
				while (Angle2 < Angle1)
				{
					Angle2 += Math.PI * 2.0;
				}
			}
			else
			{
				while (Angle1 < Angle2)
				{
					Angle1 += Math.PI * 2.0;
				}
				m_FlatenDeltaAngle = -m_FlatenDeltaAngle;
			}
			m_Direction = Direction;
			m_StartAngle = Angle1;
			m_EndAngle = Angle2;
			m_IsInitialized = true;
		}
	};
}