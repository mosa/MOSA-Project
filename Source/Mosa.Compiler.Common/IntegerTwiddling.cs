// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common;

public static class IntegerTwiddling
{
	public static bool IsAddUnsignedCarry(ulong a, ulong b) => b > 0 && a > ulong.MaxValue - b;

	public static bool IsAddUnsignedCarry(uint a, uint b) => b > 0 && a > uint.MaxValue - b;

	public static bool IsAddSignedOverflow(int a, int b) => a switch
	{
		> 0 when b > 0 && b > int.MaxValue - a => true,
		< 0 when b < 0 && b < int.MinValue - a => true,
		_ => false
	};

	public static bool IsAddSignedOverflow(long a, long b) => a switch
	{
		> 0 when b > 0 && b > long.MaxValue - a => true,
		< 0 when b < 0 && b < long.MinValue - a => true,
		_ => false
	};

	public static bool IsAddUnsignedCarry(uint a, uint b, bool carry)
	{
		if (IsAddUnsignedCarry(a, b))
			return true;

		return carry & a + b == uint.MaxValue;
	}

	public static bool IsSubSignedOverflow(int a, int b) => b switch
	{
		< 0 when a < int.MinValue - b => true,
		> 0 when a > int.MaxValue - b => true,
		_ => false
	};

	public static bool IsSubSignedOverflow(long a, long b) => b switch
	{
		< 0 when a < long.MinValue - b => true,
		> 0 when a > long.MaxValue - b => true,
		_ => false
	};

	public static bool IsSubUnsignedCarry(uint a, uint b) => b > a;

	public static bool IsSubUnsignedCarry(ulong a, ulong b) => b > a;

	public static bool IsMultiplyUnsignedCarry(uint a, uint b) => a * (ulong)b > uint.MaxValue;

	public static bool IsMultiplySignedOverflow(int a, int b) => (b < 0 && a == int.MinValue) || (b != 0 && a * b / b != a);

	public static bool IsMultiplyUnsignedCarry(ulong a, ulong b)
	{
		if (a == 0 || b == 0) return false;
		return ulong.MaxValue / a < b;
	}

	public static bool IsMultiplySignedOverflow(long a, long b) => (b < 0 && a == long.MinValue) || (b != 0 && a * b / b != a);

	public static bool HasSignBitSet(int a) => a < 0;

	public static bool HasSignBitSet(long a) => a < 0;
}
