// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common
{
	public static class BitTwiddling
	{
		public static bool IsPowerOfTwo(ulong n)
		{
			return (n & (n - 1)) == 0;
		}

		public static int GetPowerOfTwo(ulong n)
		{
			int bits = 0;
			while (n != 0)
			{
				bits++;
				n >>= 1;
			}

			return bits - 1;
		}

		public static int GetHighestBitSet(ulong value)
		{
			int r = 0;
			while (value != 0)
			{
				value >>= 1;
				r++;
			}
			return r;
		}

		public static ulong GetClearBitsOver(ulong value)
		{
			var highest = GetHighestBitSet(value);

			if (highest == 32)
				return 0;

			return ~((1uL << (highest + 1)) - 1uL);
		}
	}
}
