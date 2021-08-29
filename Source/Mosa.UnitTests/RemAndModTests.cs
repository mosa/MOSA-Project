// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests
{
	public static class RemAndModTests
	{
		[MosaUnitTest(Series = "I4")]
		public static int even_mod(int n)
		{
			return n % 2;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint even_unsigned(uint n)
		{
			return n % 2;
		}

		[MosaUnitTest(Series = "I4")]
		public static int even_bit(int n)
		{
			return n & 1;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool even_bool(uint n)
		{
			return (n % 2) != 0;
		}

		[MosaUnitTest(Series = "U4")]
		public static bool aligned4_bool(uint n)
		{
			return (n % 4) != 0;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint aligned4_unsigned(uint n)
		{
			return n % 4;
		}
	}
}
