// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTests;

namespace Mosa.UnitTests.Optimization
{
	public static partial class InterestingTests
	{
		[MosaUnitTest(Series = "I4")]
		public static bool IsOddI4(int a)
		{
			return (a % 2) != 0;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool IsOddU4(uint a)
		{
			return (a % 2) != 0;
		}

		[MosaUnitTest(Series = "I4")]
		public static bool IsOdd2I4(int a)
		{
			return (a % 2) == 1;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool IsOdd2U4(uint a)
		{
			return (a % 2) == 1;
		}

		[MosaUnitTest(Series = "I4")]
		public static bool IsEvenI4(int a)
		{
			return (a % 2) == 0;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool IsEvenU4(uint a)
		{
			return (a % 2) == 0;
		}

		[MosaUnitTest(Series = "I4")]
		public static bool IsEven2I4(int a)
		{
			return (a % 2) != 1;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool ImproveDivideU4(uint a)
		{
			return a / 12 == 15;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool TooHighU4(uint a)
		{
			return a % 12 == 15;
		}
	}
}
