// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common;

public static class IntegerTwiddling
{
	public static bool IsAddUnsignedCarry(ulong a, ulong b)
	{
		return b > 0 && a > ulong.MaxValue - b;
	}

	public static bool IsAddUnsignedCarry(uint a, uint b)
	{
		return b > 0 && a > uint.MaxValue - b;
	}

	public static bool IsAddSignedOverflow(int a, int b)
	{
		if (a > 0 && b > 0)
			if (b > int.MaxValue - a)
				return true;

		if (a < 0 && b < 0)
			if (b < int.MinValue - a)
				return true;

		return false;
	}

	public static bool IsAddSignedOverflow(long a, long b)
	{
		if (a > 0 && b > 0)
			if (b > long.MaxValue - a)
				return true;

		if (a < 0 && b < 0)
			if (b < long.MinValue - a)
				return true;

		return false;
	}

	public static bool IsAddUnsignedCarry(uint a, uint b, bool carry)
	{
		if (IsAddUnsignedCarry(a, b))
			return true;

		if (carry & a + b == uint.MaxValue)
			return true;

		return false;
	}

	public static bool IsSubSignedOverflow(int a, int b)
	{
		if (b < 0 && a < int.MinValue - b)
			return true;

		if (b > 0 && a > int.MaxValue - b)
			return true;

		return false;
	}

	public static bool IsSubSignedOverflow(long a, long b)
	{
		if (b < 0 && a < long.MinValue - b)
			return true;

		if (b > 0 && a > long.MaxValue - b)
			return true;

		return false;
	}

	public static bool IsSubUnsignedCarry(uint a, uint b)
	{
		return b > a;
	}

	public static bool IsSubUnsignedCarry(ulong a, ulong b)
	{
		return b > a;
	}

	public static bool IsMultiplyUnsignedCarry(uint a, uint b)
	{
		var r = a * (ulong)b;

		return r > uint.MaxValue;
	}

	public static bool IsMultiplySignedOverflow(int a, int b)
	{
		var z = a * b;
		return (b < 0 && a == int.MinValue) | (b != 0 && z / b != a);
	}

	public static bool IsMultiplyUnsignedCarry(ulong a, ulong b)
	{
		if (a == 0 | b == 0)
			return false;

		return ulong.MaxValue / a < b;
	}

	public static bool IsMultiplySignedOverflow(long a, long b)
	{
		var z = a * b;
		return (b < 0 && a == long.MinValue) | (b != 0 && z / b != a);
	}

	public static bool HasSignBitSet(int a)
	{
		return a < 0;
	}

	public static bool HasSignBitSet(uint a)
	{
		return HasSignBitSet((int)a);
	}

	public static bool HasSignBitSet(long a)
	{
		return a < 0;
	}

	public static bool HasSignBitSet(ulong a)
	{
		return HasSignBitSet((long)a);
	}
}
