// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class CompilerTests
	{
		public static int even_mod(int n)
		{
			return n % 2;
		}

		public static uint even_unsigned(uint n)
		{
			return n % 2;
		}

		public static int even_bit(int n)
		{
			return n & 1;
		}

		public static bool even_bool(uint n)
		{
			return (n % 2) != 0;
		}

		public static bool aligned4_bool(uint n)
		{
			return (n % 4) != 0;
		}

		public static uint aligned4_unsigned(uint n)
		{
			return n % 4;
		}
	}
}
