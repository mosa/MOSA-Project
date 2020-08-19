// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests
{
	public static class Algebraic
	{
		[MosaUnitTest(Series = "I4I4")]
		public static int AAPlusBBPlus2AB(int a, int b)
		{
			return (a * a) + (b * b) + (2 * a * b);
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int AAPlusBBMinus2AB(int a, int b)
		{
			return (a * a) + (b * b) - (2 * a * b);
		}

		[MosaUnitTest(Series = "I4")]
		public static int PerfectSquareFormula1(int x)
		{
			return (x * x) + (10 * x) + 25;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int AAMinusBB(int a, int b)
		{
			return (a * a) - (b * b);
		}
	}
}
