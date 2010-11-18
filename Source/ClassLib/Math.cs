/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.ClassLib
{
	/// <summary>
	/// 
	/// </summary>
	public static class Math
	{
		/// <summary>
		/// Returns the lowest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static uint Min(uint a, uint b)
		{
			if (a < b) return a; else return b;
		}

		/// <summary>
		/// Returns the highest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static uint Max(uint a, uint b)
		{
			if (a >= b) return a; else return b;
		}

		/// <summary>
		/// Returns the lowest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static int Min(int a, int b)
		{
			if (a < b) return a; else return b;
		}

		/// <summary>
		/// Returns the highest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static int Max(int a, int b)
		{
			if (a >= b) return a; else return b;
		}

		/// <summary>
		/// Returns the lowest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static byte Min(byte a, byte b)
		{
			if (a < b) return a; else return b;
		}

		/// <summary>
		/// Returns the highest value
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B</param>
		/// <returns></returns>
		public static byte Max(byte a, byte b)
		{
			if (a >= b) return a; else return b;
		}

		/*The following are based on John Walkers trig functions for FBENCH.*/
		static double pic = 3.1415926535897932;
		static double pi = pic, twopi = pic * 2.0, piover4 = pic / 4.0, fouroverpi = 4.0 / pic, piover2 = pic / 2.0;
		static double[] atanc = new double[] { 0.0, 0.4636476090008061165, 0.7853981633974483094, 0.98279372324732906714, 1.1071487177940905022, 1.1902899496825317322, 1.2490457723982544262, 1.2924966677897852673, 1.3258176636680324644 };
		
		public static double Pi { get { return pic; } }
		
		public static double[] AtanC { get { return atanc; } }
		
		public static double Fabs (double x)
		{
			return ((x < 0.0) ? -x : x);
		}

		public static double Cot (double x)
		{
			return (1.0 / Tan (x));
		}

		public static double Aint (double x)
		{
			float l;

			l = (float)x;
			
			if ((-0.5) != 0 && l < 0)
				l++;
			
			x = (double)l;
			
			return x;
		}

		public static double Sin (double x)
		{
			bool sign;
			double y, r, z;
			
			x = (((sign = (x < 0.0)) != false) ? -x : x);
			
			if (x > twopi)
				x -= (Aint (x / twopi) * twopi);
			
			if (x > pi) {
				x -= pi;
				sign = !sign;
			}
			
			if (x > piover2)
				x = pi - x;
			
			if (x < piover4) {
				y = x * fouroverpi;
				z = y * y;
				r = y * (((((((-0.202253129293E-13 * z + 0.69481520350522E-11) * z - 0.17572474176170806E-8) * z + 0.313361688917325348E-6) * z - 0.365762041821464001E-4) * z + 0.249039457019271628E-2) * z - 0.0807455121882807815) * z + 0.785398163397448310);
			} else {
				y = (piover2 - x) * fouroverpi;
				z = y * y;
				r = ((((((-0.38577620372E-12 * z + 0.11500497024263E-9) * z - 0.2461136382637005E-7) * z + 0.359086044588581953E-5) * z - 0.325991886926687550E-3) * z + 0.0158543442438154109) * z - 0.308425137534042452) * z + 1.0;
			}
			return sign ? -r : r;
		}

		public static double Cos (double x)
		{
			x = (x < 0.0) ? -x : x;
			
			if (x > twopi)
				x = x - (Aint (x / twopi) * twopi);
			
			return Sin (x + piover2);
		}

		public static double Tan (double x)
		{
			return Sin (x) / Cos (x);
		}

		public static double Sqrt (double x)
		{
			double c, cl, y;
			int n;
			
			if (x == 0.0)
				return 0.0;
			
			if (x < 0.0) {
				return 1;
			}
			
			y = (0.154116 + 1.893872 * x) / (1.0 + 1.047988 * x);
			
			c = (y - x / y) / 2.0;
			cl = 0.0;
			for (n = 50; c != cl; n--) {
				y = y - c;
				cl = c;
				c = (y - x / y) / 2.0;
			}
			return y;
		}

		public static double Atan (double x)
		{
			bool sign;
			int l, y;
			double a, b, z;
			
			x = (((sign = (x < 0.0)) != false) ? -x : x);
			l = 0;
			
			if (x >= 4.0) {
				l = -1;
				x = 1.0 / x;
				y = 0;
				goto atl;
			} else {
				if (x < 0.25) {
					y = 0;
					goto atl;
				}
			}
			
			y = (int)Aint (x / 0.5);
			z = y * 0.5;
			x = (x - z) / (x * z + 1);
			atl:
			
			z = x * x;
			b = ((((893025.0 * z + 49116375.0) * z + 425675250.0) * z + 1277025750.0) * z + 1550674125.0) * z + 654729075.0;
			a = (((13852575.0 * z + 216602100.0) * z + 891080190.0) * z + 1332431100.0) * z + 654729075.0;
			a = (a / b) * x + atanc[y];
			
			if (l != 0)
				a = piover2 - a;
			
			return sign ? -a : a;
		}


		public static double Atan2 (double y, double x)
		{
			double temp;
			
			if (x == 0.0) {
				if (y == 0.0)
					return 0.0; 
				else if (y > 0)
					return piover2;
				else
					return -piover2;
			}
			
			temp = Atan (y / x);
			
			if (x < 0.0) {
				if (y >= 0.0)
					temp += pic;
				else
					temp -= pic;
			}
			
			return temp;
		}

		public static double Asin (double x)
		{
			if (Fabs (x) > 1.0) {
				return 1;
			}
			
			return Atan2 (x, Sqrt (1 - x * x));
		}
	}
}
