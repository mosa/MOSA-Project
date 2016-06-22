// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
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

		public static int Pressure9(int a, int b, int c, int d, int e, int f, int g)
		{
			int a1 = a;
			int b1 = b;
			int c1 = c;
			int d1 = d;
			int e1 = e;
			int f1 = f;
			int g1 = g;

			int z = a1 + b1 + c1 + d1 + e1 + f1 + g1;

			for (int i = 1; i < 20; i++)
			{
				z = z + a1 * b1 * c1 * d1 * e1 * f1 * g1 + i;
			}

			return z;
		}
	}
}
