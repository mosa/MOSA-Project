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
	public interface IImageFilterFunction
	{
		double Radius
		{
			get;
		}
		double CalculateWeight(double x);
	};

	//-----------------------------------------------------ImageFilterLookUpTable
	public class ImageFilterLookUpTable
	{
		double m_radius;
		uint m_diameter;
		int m_start;
		ArrayPOD<short> m_weight_array;

		public enum EImageFilterScale
		{
			Shift = 14,                      //----Shift
			Scale = 1 << Shift, //----Scale 
			Mask = Scale - 1   //----Mask 
		};

		public enum EImageSubpixelScale
		{
			Shift = 8,                         //----Shift
			Scale = 1 << Shift, //----Scale 
			Mask = Scale - 1   //----Mask 
		};

		public void Calculate(IImageFilterFunction filter)
		{
			Calculate(filter, true);
		}

		public void Calculate(IImageFilterFunction filter, bool normalization)
		{
			double r = filter.Radius;
			ReallocateLookupTable(r);
			uint i;
			uint pivot = diameter() << ((int)EImageSubpixelScale.Shift - 1);
			for (i = 0; i < pivot; i++)
			{
				double x = (double)i / (double)EImageSubpixelScale.Scale;
				double y = filter.CalculateWeight(x);
				m_weight_array.Array[pivot + i] =
				m_weight_array.Array[pivot - i] = (short)Basics.Round(y * (int)EImageFilterScale.Scale);
			}
			uint end = (diameter() << (int)EImageSubpixelScale.Shift) - 1;
			m_weight_array.Array[0] = m_weight_array.Array[end];
			if (normalization)
			{
				Normalize();
			}
		}

		public ImageFilterLookUpTable()
		{
			m_weight_array = new ArrayPOD<short>(256);
			m_radius = (0);
			m_diameter = (0);
			m_start = (0);
		}

		public ImageFilterLookUpTable(IImageFilterFunction filter)
			: this(filter, true)
		{

		}
		public ImageFilterLookUpTable(IImageFilterFunction filter, bool normalization)
		{
			m_weight_array = new ArrayPOD<short>(256);
			Calculate(filter, normalization);
		}

		public double radius() { return m_radius; }
		public uint diameter() { return m_diameter; }
		public int start() { return m_start; }
		public unsafe short[] weight_array() { return m_weight_array.Array; }

		//--------------------------------------------------------------------
		// This function normalizes integer values and corrects the rounding 
		// errors. It doesn't do anything with the source floating point values
		// (m_weight_array_dbl), it corrects only integers according to the rule 
		// of 1.0 which means that any sum of Pixel weights must be equal to 1.0.
		// So, the filter function must produce a graph of the proper shape.
		//--------------------------------------------------------------------
		public void Normalize()
		{
			uint i;
			int flip = 1;

			for (i = 0; i < (int)EImageSubpixelScale.Scale; i++)
			{
				for (; ; )
				{
					int sum = 0;
					uint j;
					for (j = 0; j < m_diameter; j++)
					{
						sum += m_weight_array.Array[j * (int)EImageSubpixelScale.Scale + i];
					}

					if (sum == (int)EImageFilterScale.Scale) break;

					double k = (double)((int)EImageFilterScale.Scale) / (double)(sum);
					sum = 0;
					for (j = 0; j < m_diameter; j++)
					{
						sum += m_weight_array.Array[j * (int)EImageSubpixelScale.Scale + i] =
							(short)Basics.Round(m_weight_array.Array[j * (int)EImageSubpixelScale.Scale + i] * k);
					}

					sum -= (int)EImageFilterScale.Scale;
					int inc = (sum > 0) ? -1 : 1;

					for (j = 0; j < m_diameter && sum != 0; j++)
					{
						flip ^= 1;
						uint idx = flip != 0 ? m_diameter / 2 + j / 2 : m_diameter / 2 - j / 2;
						int v = m_weight_array.Array[idx * (int)EImageSubpixelScale.Scale + i];
						if (v < (int)EImageFilterScale.Scale)
						{
							m_weight_array.Array[idx * (int)EImageSubpixelScale.Scale + i] += (short)inc;
							sum += inc;
						}
					}
				}
			}

			uint pivot = m_diameter << ((int)EImageSubpixelScale.Shift - 1);

			for (i = 0; i < pivot; i++)
			{
				m_weight_array.Array[pivot + i] = m_weight_array.Array[pivot - i];
			}
			uint end = (diameter() << (int)EImageSubpixelScale.Shift) - 1;
			m_weight_array.Array[0] = m_weight_array.Array[end];
		}

		private void ReallocateLookupTable(double radius)
		{
			m_radius = radius;
			m_diameter = Basics.UnsignedCeiling(radius) * 2;
			m_start = -(int)(m_diameter / 2 - 1);
			int size = (int)m_diameter << (int)EImageSubpixelScale.Shift;
			if (size > m_weight_array.Size)
			{
				m_weight_array.Resize(size);
			}
		}
	};

	/*

	//--------------------------------------------------------image_filter
	public class image_filter : ImageFilterLookUpTable
	{
		public image_filter()
		{
			Calculate(m_filter_function);
		}
	
		private IImageFilter m_filter_function;
	};
	 */


	//-----------------------------------------------BilinearImageFilter
	public struct BilinearImageFilter : IImageFilterFunction
	{
		public double Radius { get { return 1.0; } }
		public double CalculateWeight(double x)
		{
			return 1.0 - x;
		}
	};

	//-----------------------------------------------HanningImageFilter
	public struct HanningImageFilter : IImageFilterFunction
	{
		public double Radius { get { return 1.0; } }
		public double CalculateWeight(double x)
		{
			return 0.5 + 0.5 * Math.Cos(Math.PI * x);
		}
	};

	//-----------------------------------------------HammingImageFilter
	public struct HammingImageFilter : IImageFilterFunction
	{
		public double Radius { get { return 1.0; } }
		public double CalculateWeight(double x)
		{
			return 0.54 + 0.46 * Math.Cos(Math.PI * x);
		}
	};

	//-----------------------------------------------HermiteImageFilter
	public struct HermiteImageFilter : IImageFilterFunction
	{
		public double Radius { get { return 1.0; } }
		public double CalculateWeight(double x)
		{
			return (2.0 * x - 3.0) * x * x + 1.0;
		}
	};

	//------------------------------------------------QuadricImageFilter
	public struct QuadricImageFilter : IImageFilterFunction
	{
		public double Radius { get { return 1.5; } }
		public double CalculateWeight(double x)
		{
			double t;
			if (x < 0.5) return 0.75 - x * x;
			if (x < 1.5) { t = x - 1.5; return 0.5 * t * t; }
			return 0.0;
		}
	};

	//------------------------------------------------BicubicImageFilter
	public class BicubicImageFilter : IImageFilterFunction
	{
		private static double pow3(double x)
		{
			return (x <= 0.0) ? 0.0 : x * x * x;
		}

		public double Radius { get { return 2.0; } }
		public double CalculateWeight(double x)
		{
			return
				(1.0 / 6.0) *
				(pow3(x + 2) - 4 * pow3(x + 1) + 6 * pow3(x) - 4 * pow3(x - 1));
		}
	};

	//-------------------------------------------------KaiserImageFilter
	public class KaiserImageFilter : IImageFilterFunction
	{
		private double a;
		private double i0a;
		private double epsilon;

		public KaiserImageFilter()
			: this(6.33)
		{

		}
		public KaiserImageFilter(double b)
		{
			a = (b);
			epsilon = (1e-12);
			i0a = 1.0 / bessel_i0(b);
		}

		public double Radius { get { return 1.0; } }
		public double CalculateWeight(double x)
		{
			return bessel_i0(a * Math.Sqrt(1.0 - x * x)) * i0a;
		}

		private double bessel_i0(double x)
		{
			int i;
			double sum, y, t;

			sum = 1.0;
			y = x * x / 4.0;
			t = y;

			for (i = 2; t > epsilon; i++)
			{
				sum += t;
				t *= (double)y / (i * i);
			}
			return sum;
		}
	};

	//----------------------------------------------CatromImageFilter
	public struct CatromImageFilter : IImageFilterFunction
	{
		public double Radius { get { return 2.0; } }
		public double CalculateWeight(double x)
		{
			if (x < 1.0) return 0.5 * (2.0 + x * x * (-5.0 + x * 3.0));
			if (x < 2.0) return 0.5 * (4.0 + x * (-8.0 + x * (5.0 - x)));
			return 0.0;
		}
	};

	//---------------------------------------------MitchellImageFilter
	public class MitchellImageFilter : IImageFilterFunction
	{
		private double p0, p2, p3;
		private double q0, q1, q2, q3;

		public MitchellImageFilter()
			: this(1.0 / 3.0, 1.0 / 3.0)
		{

		}

		public MitchellImageFilter(double b, double c)
		{
			p0 = ((6.0 - 2.0 * b) / 6.0);
			p2 = ((-18.0 + 12.0 * b + 6.0 * c) / 6.0);
			p3 = ((12.0 - 9.0 * b - 6.0 * c) / 6.0);
			q0 = ((8.0 * b + 24.0 * c) / 6.0);
			q1 = ((-12.0 * b - 48.0 * c) / 6.0);
			q2 = ((6.0 * b + 30.0 * c) / 6.0);
			q3 = ((-b - 6.0 * c) / 6.0);
		}

		public double Radius { get { return 2.0; } }
		public double CalculateWeight(double x)
		{
			if (x < 1.0) return p0 + x * x * (p2 + x * p3);
			if (x < 2.0) return q0 + x * (q1 + x * (q2 + x * q3));
			return 0.0;
		}
	};


	//----------------------------------------------Spline16ImageFilter
	public struct Spline16ImageFilter : IImageFilterFunction
	{
		public double Radius { get { return 2.0; } }
		public double CalculateWeight(double x)
		{
			if (x < 1.0)
			{
				return ((x - 9.0 / 5.0) * x - 1.0 / 5.0) * x + 1.0;
			}
			return ((-1.0 / 3.0 * (x - 1) + 4.0 / 5.0) * (x - 1) - 7.0 / 15.0) * (x - 1);
		}
	};


	//---------------------------------------------Spline36ImageFilter
	public struct Spline36ImageFilter : IImageFilterFunction
	{
		public double Radius { get { return 3.0; } }
		public double CalculateWeight(double x)
		{
			if (x < 1.0)
			{
				return ((13.0 / 11.0 * x - 453.0 / 209.0) * x - 3.0 / 209.0) * x + 1.0;
			}
			if (x < 2.0)
			{
				return ((-6.0 / 11.0 * (x - 1) + 270.0 / 209.0) * (x - 1) - 156.0 / 209.0) * (x - 1);
			}
			return ((1.0 / 11.0 * (x - 2) - 45.0 / 209.0) * (x - 2) + 26.0 / 209.0) * (x - 2);
		}
	};


	//----------------------------------------------GaussianImageFilter
	public struct GaussianImageFilter : IImageFilterFunction
	{
		public double Radius { get { return 2.0; } }
		public double CalculateWeight(double x)
		{
			return Math.Exp(-2.0 * x * x) * Math.Sqrt(2.0 / Math.PI);
		}
	};


	//------------------------------------------------BesselImageFilter
	public struct BesselImageFilter : IImageFilterFunction
	{
		public double Radius { get { return 3.2383; } }
		public double CalculateWeight(double x)
		{
			return (x == 0.0) ? Math.PI / 4.0 : PictorMath.Bessel(Math.PI * x, 1) / (2.0 * x);
		}
	};


	//-------------------------------------------------SincImageFilter
	public class SincImageFilter : IImageFilterFunction
	{
		public SincImageFilter(double r)
		{
			m_radius = (r < 2.0 ? 2.0 : r);
		}
		public double Radius { get { return m_radius; } }
		public double CalculateWeight(double x)
		{
			if (x == 0.0) return 1.0;
			x *= Math.PI;
			return Math.Sin(x) / x;
		}

		private double m_radius;
	};


	//-----------------------------------------------LanczosImageFilter
	public class LanczosImageFilter : IImageFilterFunction
	{
		public LanczosImageFilter(double r)
		{
			m_radius = (r < 2.0 ? 2.0 : r);
		}
		public double Radius { get { return m_radius; } }
		public double CalculateWeight(double x)
		{
			if (x == 0.0) return 1.0;
			if (x > m_radius) return 0.0;
			x *= Math.PI;
			double xr = x / m_radius;
			return (Math.Sin(x) / x) * (Math.Sin(xr) / xr);
		}
		private double m_radius;
	};

	//----------------------------------------------BlackmanImageFilter
	public class BlackmanImageFilter : IImageFilterFunction
	{
		public BlackmanImageFilter(double r)
		{
			m_radius = (r < 2.0 ? 2.0 : r);
		}

		public double Radius { get { return m_radius; } }

		public double CalculateWeight(double x)
		{
			if (x == 0.0)
			{
				return 1.0;
			}

			if (x > m_radius)
			{
				return 0.0;
			}

			x *= Math.PI;
			double xr = x / m_radius;
			return (Math.Sin(x) / x) * (0.42 + 0.5 * Math.Cos(xr) + 0.08 * Math.Cos(2 * xr));
		}

		private double m_radius;
	};
}