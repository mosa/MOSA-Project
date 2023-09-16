// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Numerics;

namespace Mosa.Compiler.Common;

public static class BitTwiddling
{
	public static bool IsPowerOfTwo(ulong n) => n != 0 && (n & (n - 1)) == 0;

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

	public static int CountTrailingZeros(ulong value) => BitOperations.TrailingZeroCount(value);

	public static int CountLeadingZeros(ulong value) => BitOperations.LeadingZeroCount(value);

	public static int CountLeadingZeros(uint value) => BitOperations.LeadingZeroCount(value);

	public static int GetHighestSetBitPosition(ulong value) => 64 - BitOperations.LeadingZeroCount(value);

	public static int GetHighestSetBitPosition(uint value) => 32 - BitOperations.LeadingZeroCount(value);

	public static ulong GetBitsOver(ulong value)
	{
		var count = BitOperations.LeadingZeroCount(value);
		if (count == 0) return 0;

		return ~((1UL << (64 - count + 1)) - 1UL);
	}

	public static int CountBits(ulong value) => BitOperations.PopCount(value);

	public static int CountBits(uint value) => BitOperations.PopCount(value);
}
