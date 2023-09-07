// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Numerics;

namespace Mosa.Compiler.Common;

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

	public static int CountTrailingZeros(ulong value)
	{
		return BitOperations.TrailingZeroCount(value);
	}

	public static int CountLeadingZeros(ulong value)
	{
		return BitOperations.LeadingZeroCount(value);
	}

	public static int CountLeadingZeros(uint value)
	{
		return BitOperations.LeadingZeroCount(value);
	}

	public static int GetHighestSetBitPosition(ulong value)
	{
		return 64 - BitOperations.LeadingZeroCount(value);
	}

	public static int GetHighestSetBitPosition(uint value)
	{
		return 32 - BitOperations.LeadingZeroCount(value);
	}

	public static ulong GetBitsOver(ulong value)
	{
		var count = BitOperations.LeadingZeroCount(value);

		if (count == 0)
			return 0;

		return ~((1uL << (64 - count)) - 1uL);
	}

	public static int CountSetBits(ulong value)
	{
		return BitOperations.PopCount(value);
	}

	public static int CountSetBits(uint value)
	{
		return BitOperations.PopCount(value);
	}
}
