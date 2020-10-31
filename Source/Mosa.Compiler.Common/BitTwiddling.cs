// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common
{
	public static class BitTwiddling
	{
		public static bool IsPowerOfTwo(ulong n)
		{
			return (n & (n - 1)) == 0;
		}

		public static uint GetPowerOfTwo(ulong n)
		{
			uint bits = 0;
			while (n != 0)
			{
				bits++;
				n >>= 1;
			}

			return bits - 1;
		}

		public static int GetHighestSetBit(ulong value)
		{
			int r = 0;
			while (value != 0)
			{
				value >>= 1;
				r++;
			}
			return r;
		}

		public static int GetLowestSetBit(ulong value)
		{
			int r = 0;
			while (value != 0)
			{
				value <<= 1;
				r++;
			}
			return r;
		}

		public static ulong GetClearBitsOver(ulong value)
		{
			var highest = GetHighestSetBit(value);

			if (highest == 32)
				return 0;

			return ~((1uL << (highest + 1)) - 1uL);
		}
	}
}
