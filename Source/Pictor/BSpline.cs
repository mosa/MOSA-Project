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
	//----------------------------------------------------------------BSpline
	// A very simple class of Bi-cubic Spline Interpolation.
	// First call Init(num, x[], y[]) where num - number of source points, 
	// x, y - arrays of X and Y values respectively. Here Y must be a function 
	// of X. It means that all the X-Coordinates must be arranged in the ascending
	// order. 
	// Then call Get(x) that calculates a Value Y for the respective X. 
	// The class supports extrapolation, i.e. you can call Get(x) where x is
	// outside the given with Init() X-range. Extrapolation is a simple linear 
	// function.
	//------------------------------------------------------------------------
	public sealed class BSpline
	{
		private int m_max;
		private int m_num;
		private int m_xOffset;
		private int m_yOffset;
		private ArrayPOD<double> m_am = new ArrayPOD<double>(16);
		private int m_last_idx;

		//------------------------------------------------------------------------
		public BSpline()
		{
			m_max = (0);
			m_num = (0);
			m_xOffset = (0);
			m_yOffset = (0);
			m_last_idx = -1;
		}

		//------------------------------------------------------------------------
		public BSpline(int num)
		{
			m_max = (0);
			m_num = (0);
			m_xOffset = (0);
			m_yOffset = (0);
			m_last_idx = -1;

			Init(num);
		}

		//------------------------------------------------------------------------
		public BSpline(int num, double[] x, double[] y)
		{
			m_max = (0);
			m_num = (0);
			m_xOffset = (0);
			m_yOffset = (0);
			m_last_idx = -1;
			Init(num, x, y);
		}


		//------------------------------------------------------------------------
		public void Init(int max)
		{
			if (max > 2 && max > m_max)
			{
				m_am.Resize(max * 3);
				m_max = max;
				m_xOffset = m_max;
				m_yOffset = m_max * 2;
			}
			m_num = 0;
			m_last_idx = -1;
		}


		//------------------------------------------------------------------------
		public void AddPoint(double x, double y)
		{
			if (m_num < m_max)
			{
				m_am[m_xOffset + m_num] = x;
				m_am[m_yOffset + m_num] = y;
				++m_num;
			}
		}


		//------------------------------------------------------------------------
		public void Prepare()
		{
			if (m_num > 2)
			{
				int i, k;
				int r;
				int s;
				double h, p, d, f, e;

				for (k = 0; k < m_num; k++)
				{
					m_am[k] = 0.0;
				}

				int n1 = 3 * m_num;

				ArrayPOD<double> al = new ArrayPOD<double>(n1);

				for (k = 0; k < n1; k++)
				{
					al[k] = 0.0;
				}

				r = m_num;
				s = m_num * 2;

				n1 = m_num - 1;
				d = m_am[m_xOffset + 1] - m_am[m_xOffset + 0];
				e = (m_am[m_yOffset + 1] - m_am[m_yOffset + 0]) / d;

				for (k = 1; k < n1; k++)
				{
					h = d;
					d = m_am[m_xOffset + k + 1] - m_am[m_xOffset + k];
					f = e;
					e = (m_am[m_yOffset + k + 1] - m_am[m_yOffset + k]) / d;
					al[k] = d / (d + h);
					al[r + k] = 1.0 - al[k];
					al[s + k] = 6.0 * (e - f) / (h + d);
				}

				for (k = 1; k < n1; k++)
				{
					p = 1.0 / (al[r + k] * al[k - 1] + 2.0);
					al[k] *= -p;
					al[s + k] = (al[s + k] - al[r + k] * al[s + k - 1]) * p;
				}

				m_am[n1] = 0.0;
				al[n1 - 1] = al[s + n1 - 1];
				m_am[n1 - 1] = al[n1 - 1];

				for (k = n1 - 2, i = 0; i < m_num - 2; i++, k--)
				{
					al[k] = al[k] * al[k + 1] + al[s + k];
					m_am[k] = al[k];
				}
			}
			m_last_idx = -1;
		}



		//------------------------------------------------------------------------
		public void Init(int num, double[] x, double[] y)
		{
			if (num > 2)
			{
				Init(num);
				int i;
				for (i = 0; i < num; i++)
				{
					AddPoint(x[i], y[i]);
				}
				Prepare();
			}
			m_last_idx = -1;
		}


		//------------------------------------------------------------------------
		void BSearch(int n, int xOffset, double x0, out int i)
		{
			int j = n - 1;
			int k;

			for (i = 0; (j - i) > 1; )
			{
				k = (i + j) >> 1;
				if (x0 < m_am[xOffset + k]) j = k;
				else i = k;
			}
		}



		//------------------------------------------------------------------------
		double Interpolation(double x, int i)
		{
			int j = i + 1;
			double d = m_am[m_xOffset + i] - m_am[m_xOffset + j];
			double h = x - m_am[m_xOffset + j];
			double r = m_am[m_xOffset + i] - x;
			double p = d * d / 6.0;
			return (m_am[j] * r * r * r + m_am[i] * h * h * h) / 6.0 / d +
				   ((m_am[m_yOffset + j] - m_am[j] * p) * r + (m_am[m_yOffset + i] - m_am[i] * p) * h) / d;
		}


		//------------------------------------------------------------------------
		double ExtrapolationLeft(double x)
		{
			double d = m_am[m_xOffset + 1] - m_am[m_xOffset + 0];
			return (-d * m_am[1] / 6 + (m_am[m_yOffset + 1] - m_am[m_yOffset + 0]) / d) *
				   (x - m_am[m_xOffset + 0]) +
				   m_am[m_yOffset + 0];
		}

		//------------------------------------------------------------------------
		double ExtrapolationRight(double x)
		{
			double d = m_am[m_xOffset + m_num - 1] - m_am[m_xOffset + m_num - 2];
			return (d * m_am[m_num - 2] / 6 + (m_am[m_yOffset + m_num - 1] - m_am[m_yOffset + m_num - 2]) / d) *
				   (x - m_am[m_xOffset + m_num - 1]) +
				   m_am[m_yOffset + m_num - 1];
		}

		//------------------------------------------------------------------------
		public double Get(double x)
		{
			if (m_num > 2)
			{
				int i;

				// Extrapolation on the left
				if (x < m_am[m_xOffset + 0]) return ExtrapolationLeft(x);

				// Extrapolation on the right
				if (x >= m_am[m_xOffset + m_num - 1]) return ExtrapolationRight(x);

				// Interpolation
				BSearch(m_num, m_xOffset, x, out i);
				return Interpolation(x, i);
			}
			return 0.0;
		}


		//------------------------------------------------------------------------
		public double GetStateful(double x)
		{
			if (m_num > 2)
			{
				// Extrapolation on the left
				if (x < m_am[m_xOffset + 0]) return ExtrapolationLeft(x);

				// Extrapolation on the right
				if (x >= m_am[m_xOffset + m_num - 1]) return ExtrapolationRight(x);

				if (m_last_idx >= 0)
				{
					// Check if x is not in current range
					if (x < m_am[m_xOffset + m_last_idx] || x > m_am[m_xOffset + m_last_idx + 1])
					{
						// Check if x between next points (most probably)
						if (m_last_idx < m_num - 2 &&
						   x >= m_am[m_xOffset + m_last_idx + 1] &&
						   x <= m_am[m_xOffset + m_last_idx + 2])
						{
							++m_last_idx;
						}
						else
							if (m_last_idx > 0 &&
							   x >= m_am[m_xOffset + m_last_idx - 1] &&
							   x <= m_am[m_xOffset + m_last_idx])
							{
								// x is between pevious points
								--m_last_idx;
							}
							else
							{
								// Else perform full search
								BSearch(m_num, m_xOffset, x, out m_last_idx);
							}
					}
					return Interpolation(x, m_last_idx);
				}
				else
				{
					// Interpolation
					BSearch(m_num, m_xOffset, x, out m_last_idx);
					return Interpolation(x, m_last_idx);
				}
			}
			return 0.0;
		}
	};
}
