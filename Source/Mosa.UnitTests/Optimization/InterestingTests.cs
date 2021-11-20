// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Optimization
{
	public static class InterestingTests
	{
		[MosaUnitTest(Series = "I4")]
		public static bool IsOdd(int a)
		{
			return (a % 2) != 0;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool IsOdd(uint a)
		{
			return (a % 2) != 0;
		}

		[MosaUnitTest(Series = "I4")]
		public static bool IsOdd2(int a)
		{
			return (a % 2) != 1;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool IsOdd2(uint a)
		{
			return (a % 2) != 1;
		}

		[MosaUnitTest(Series = "I4")]
		public static bool IsEven(int a)
		{
			return (a % 2) == 0;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool IsEven(uint a)
		{
			return (a % 2) == 0;
		}
	}
}
