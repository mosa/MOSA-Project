// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests
{
	public static class OptimizationTests
	{
		[MosaUnitTest]
		public static bool OptimizationTest1()
		{
			int a = 10;
			int b = 20;

			int c = a + b;

			return c == 30;
		}

		[MosaUnitTest]
		public static bool OptimizationTest2()
		{
			uint a = 10;
			uint b = 20;

			uint c = a + b;

			return c == 30;
		}

		[MosaUnitTest]
		public static bool OptimizationTest3()
		{
			byte a = 10;
			uint b = 20;

			uint c = a + b;

			return c == 30;
		}

		[MosaUnitTest]
		public static bool OptimizationTest4()
		{
			ulong a = 10;
			ulong b = 20;

			ulong c = a + b;

			return c == 30;
		}

		[MosaUnitTest]
		public static bool OptimizationTest5()
		{
			ulong a = ulong.MaxValue;
			ulong b = 20;

			ulong c = a + b;

			return c == unchecked(ulong.MaxValue + 20);
		}

		[MosaUnitTest]
		public static bool OptimizationTest6()
		{
			char a = (char)10;
			char b = (char)20;

			char c = (char)(a + b);

			return c == 30;
		}

		[MosaUnitTest]
		public static bool OptimizationTest7()
		{
			ulong a = 10;
			ulong b = 0;

			ulong c = a * b;

			return c == 0;
		}

		[MosaUnitTest]
		public static bool OptimizationTest8()
		{
			int a = 10;
			int b = 0;

			int c = a * b;

			return c == 0;
		}

		[MosaUnitTest]
		public static bool OptimizationTest9()
		{
			ulong a = 10;
			ulong b = 1;

			ulong c = a * b;

			return c == 10;
		}

		[MosaUnitTest]
		public static bool OptimizationTest10()
		{
			int a = 1;
			int b = 10;

			int c = a * b;

			return c == 10;
		}

		[MosaUnitTest]
		public static bool OptimizationTest11()
		{
			int a = 0;
			int b = 10;

			int c = a * b;

			return c == 0;
		}

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

		[MosaUnitTest(Series = "I4")]
		public static int OptimizationTest13(int q)
		{
			int a = 10;
			int b = 20;

			int c = a + b + q;

			return c;
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
	}
}
