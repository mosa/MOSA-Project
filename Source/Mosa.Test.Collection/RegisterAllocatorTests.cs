/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Test.Collection
{
	public static class RegisterAllocatorTests
	{
		public static int Pressure8()
		{
			int a = 10;
			int b = 20;
			int c = 30;
			int d = 40;
			int e = 50;
			int f = 60;
			int g = 70;
			int h = 80;
			int j = 90;
			int k = 100;

			if (a >> 2 == 5)
				k++;
			else
				j++;

			int z = ((((((a >> b) + c + (d * e) - f) * g) - h) * j) * k) >> 2;

			return z;
		}

		public static int Pressure7(int a, int b, int c, int d, int e, int f, int g)
		{
			int h = 70;
			int j = 90;
			int k = 100;

			if (a >> 2 == 5)
				k++;
			else
				j++;

			int z = (((((((a >> b) + c + (d * e) - f) * g) - h) * j) * k) >> 2) + a + b + c + d + e + (f * g);

			return z;
		}

		public static int Pressure7B(int a, int b, int c, int d, int e, int f, int g)
		{
			int h = 70;
			int j = 90;
			int k = 100;

			if (a >> 2 == 5)
			{
				k++; a++; c--;
			}
			else
			{
				j++; c++; d--;
			}

			int z = (((((((a >> b) + c * (d * e) - f) * g) - h) * j) * k) >> 2) + a % b + c * d + e % (f * g + 1);

			if (z % 2 == c)
			{
				z--;
				z = z + d * e;
			}
			else
			{
				z = z * a / (e + 1);
			}

			return z;
		}

		public static int Pressure7C(int a, int b, int c, int d, int e, int f, int g)
		{
			int z = a + b + c + d + e + f + g;
			int q = 0;

			for (int i = 1; i < 20; i++)
			{
				z = z + a * b * c * d * e * f * g + i * q;
				q = q + z - 1;
			}

			return z * 10;
		}
	}
}