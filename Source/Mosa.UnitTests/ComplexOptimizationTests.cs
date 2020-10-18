// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests
{
	public static class ComplexOptimizationTests
	{
		[MosaUnitTest]
		public static int OptimizationTest12()
		{
			int a = 32;
			int b = 10;
			int c = 10;
			int d = 1;
			int e = 0;

			int z = (a * b) + ((c * d) + (c * d) * e) + e;

			return z;
		}

		[MosaUnitTest(0, 1, 2, 3)]
		public static int OptimizationTest14(int a1, int b1, int c1, int d1)
		{
			var a = a1;
			var b = b1;
			var c = c1;
			var d = d1;

			int z = (a * b) + (c * d) + (c * d) + (a * b);

			return z;
		}

		[MosaUnitTest(1, true)]
		public static int OptimizationTest14(int j, bool b1)
		{
			int i = j;
			int a = 4 * i;
			int d;

			if (b1)
			{
				d = 4 * i;
			}
			else
			{
				d = 0;
			}

			return d;
		}

		[MosaUnitTest]
		public static int ConditionalConstantPropagation1()
		{
			int i = 1;
			int j = 1;
			int k = 0;

			while (k < 100)
			{
				if (j < 20)
				{
					j = i;
					k = k + 1;
				}
				else
				{
					j = k;
					k = k + 2;
				}
			}
			return j;
		}

		[MosaUnitTest]
		public static int ConditionalConstantPropagation2()
		{
			int a = 3;
			int b = 7;
			int c = 20;

			int z;

			int g = a + b;

			if (g < c)
				z = 9;
			else
				z = 12;

			return z;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool ConditionalConstantPropagation3(int a, int b)
		{
			int x, y, z;

			if (a > 3)
			{
				x = 4;
				y = 1;
				z = b & 0xFF00;
			}
			else
			{
				x = 9;
				y = 2;
				z = b << 8;
			}

			int p = (x * y) * 4;
			int q = z & 0xF;

			return p >= 16 && q == 0;
		}

		[MosaUnitTest(0)]
		[MosaUnitTest(10)]
		[MosaUnitTest(25)]
		[MosaUnitTest(50)]
		[MosaUnitTest(100)]
		public static int OptimizationTest20(int a)
		{
			if (a > 50)
			{
				if (a < 25)
					return 2;
				else
					return 1;
			}

			return 0;
		}

		[MosaUnitTest((byte)0)]
		[MosaUnitTest((byte)8)]
		[MosaUnitTest((byte)16)]
		[MosaUnitTest((byte)19)]
		[MosaUnitTest((byte)23)]
		[MosaUnitTest((byte)111)]
		[MosaUnitTest((byte)127)]
		[MosaUnitTest((byte)128)]
		[MosaUnitTest((byte)254)]
		[MosaUnitTest((byte)255)]
		public static bool OptimizationTest21(byte a)
		{
			short b = a;
			b <<= 4;
			b |= 3;
			return b != 0;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool OptimizationTest22(uint a)
		{
			return a % 2 == 0;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool OptimizationTest23(uint a)
		{
			return a + 10 > a;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool OptimizationTest24(uint a)
		{
			return a + 10 <= a;
		}

		[MosaUnitTest(Series = "I4")]
		public static int OptimizationTest25(int a)
		{
			return a % 2 != 0 ? 4 : 2;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint OptimizationTest26(uint a)
		{
			return a % 2 != 0 ? 4u : 2u;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool OptimizationTest27(uint a)
		{
			return a / 12 == 15;
		}

		[MosaUnitTest(Series = "I4")]
		public static bool OptimizationTest28(int a)
		{
			return a / 12 == 15;
		}

		[MosaUnitTest(Series = "I4")]
		public static bool OptimizationTest29(int a)
		{
			return a / -12 == -15;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint OptimizationTest30(uint a)
		{
			return (a | 0x0F) >> 8;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint OptimizationTest31(uint a)
		{
			return (a | 0x01) >> 3;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint OptimizationTest32(uint a)
		{
			return (a ^ 0b11) >> 3;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint OptimizationTest33(uint a)
		{
			return (a | 0xF0000000) << 8;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint OptimizationTest34(uint a)
		{
			return (a ^ 0xF0000000) << 8;
		}
	}
}
