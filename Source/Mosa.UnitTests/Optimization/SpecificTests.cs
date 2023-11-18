// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Optimization;

public static class SpecificTests
{
	[MosaUnitTest(Series = "I4I4")]
	public static uint Xor32Xor32(uint x, uint y)
	{
		return x ^ x ^ y;
	}

	[MosaUnitTest(Series = "I8I8")]
	public static ulong Xor64Xor64(ulong x, ulong y)
	{
		return x ^ x ^ y;
	}

	[MosaUnitTest(Series = "I4")]
	public static uint IsolateAndFlipLeastSignificantBit32(uint x)
	{
		return ((x << 31) >> 31) + 1;
	}

	[MosaUnitTest(Series = "I8")]
	public static ulong IsolateAndFlipLeastSignificantBit64(ulong x)
	{
		return ((x << 63) >> 63) + 1;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static uint Or32Xor32(uint x, uint y)
	{
		return (x ^ y) | x;
	}

	[MosaUnitTest(Series = "I8I8")]
	public static ulong Or64Xor64(ulong x, ulong y)
	{
		return (x ^ y) | x;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static int Or32And32And32ByI1(int x, int y)
	{
		return (byte)x | (byte)y;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static int Or32And32And32ByI2(int x, int y)
	{
		return (short)x | (short)y;
	}

	[MosaUnitTest(Series = "I8I8")]
	public static long Or64And64And64ByI4(long x, long y)
	{
		return (int)x | (int)y;
	}

	[MosaUnitTest(Series = "I4")]
	public static int NegTest32(int i) => ~i + 1;

	[MosaUnitTest(Series = "I8")]
	public static long NegTest64(long i) => ~i + 1;

	[MosaUnitTest(Series = "I4")]
	public static bool BitSet(int x) => (x & 1) == 1;

	[MosaUnitTest(Series = "I4")]
	public static int ResetLowestSetBit(int x) => x & (x - 1);

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftLeftU8By1(ulong x) => x << 1;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftLeftU8By2(ulong x) => x << 2;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftLeftU8By31(ulong x) => x << 31;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftLeftU8By32(ulong x) => x << 32;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftLeftU8By33(ulong x) => x << 33;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftLeftU8By45(ulong x) => x << 45;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftLeftU8By63(ulong x) => x << 63;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftLeftU8By64(ulong x) => x << 64;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftRightU8By1(ulong x) => x >> 1;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftRightU8By2(ulong x) => x >> 2;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftRightU8By31(ulong x) => x >> 31;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftRightU8By32(ulong x) => x >> 32;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftRightU8By33(ulong x) => x >> 33;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftRightU8By45(ulong x) => x >> 45;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftRightU8By63(ulong x) => x >> 63;

	[MosaUnitTest(Series = "U8")]
	public static ulong ShiftRightU8By64(ulong x) => x >> 64;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftLeftI8By1(long x) => x << 1;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftLeftI8By2(long x) => x << 2;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftLeftI8By31(long x) => x << 31;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftLeftI8By32(long x) => x << 32;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftLeftI8By33(long x) => x << 33;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftLeftI8By45(long x) => x << 45;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftLeftI8By63(long x) => x << 63;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftLeftI8By64(long x) => x << 64;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftRightI8By1(long x) => x << 1;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftRightI8By2(long x) => x >> 2;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftRightI8By31(long x) => x >> 31;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftRightI8By32(long x) => x >> 32;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftRightI8By33(long x) => x >> 33;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftRightI8By45(long x) => x >> 45;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftRightI8By63(long x) => x >> 63;

	[MosaUnitTest(Series = "I8")]
	public static long ShiftRightI8By64(long x) => x >> 64;

	[MosaUnitTest(Series = "U4")]
	public static bool CompareLessAddU4(uint v)
	{
		var a = v & 0xFFFF;
		var c1 = 10;
		var v2 = 100;

		return a + c1 < v2;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool CompareLessAddI4(int v)
	{
		var a = v & 0xFFFF;
		var c1 = 10;
		var v2 = 100;

		return a + c1 < v2;
	}

	[MosaUnitTest(Series = "U8")]
	public static bool CompareLessAddU8(ulong v)
	{
		var a = v & 0xFFFF;
		var c1 = 10ul;
		var v2 = 100ul;

		return a + c1 < v2;
	}

	[MosaUnitTest(Series = "I8")]
	public static bool CompareLessAddI8(long v)
	{
		var a = v & 0xFFFF;
		var c1 = 10L;
		var v2 = 100L;

		return a + c1 < v2;
	}

	[MosaUnitTest(Series = "U4")]
	public static bool CompareEqualAddU4(uint v)
	{
		var a = v & 0xFFFF;
		var c1 = 10;
		var v2 = 100;

		return a + c1 == v2;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool CompareEqualAddI4(int v)
	{
		var a = v & 0xFFFF;
		var c1 = 10;
		var v2 = 100;

		return a + c1 == v2;
	}

	[MosaUnitTest(Series = "U8")]
	public static bool CompareEqualAddU8(ulong v)
	{
		var a = v & 0xFFFF;
		var c1 = 10ul;
		var v2 = 100ul;

		return a + c1 == v2;
	}

	[MosaUnitTest(Series = "I8")]
	public static bool CompareEqualAddI8(long v)
	{
		var a = v & 0xFFFF;
		var c1 = 10L;
		var v2 = 100L;

		return a + c1 == v2;
	}

	[MosaUnitTest(Series = "U4")]
	public static uint MulBy3(uint a)
	{
		return a * 3;
	}

	[MosaUnitTest(Series = "U4")]
	public static uint MulBy5(uint a)
	{
		return a * 5;
	}

	[MosaUnitTest(Series = "U4")]
	public static uint MulBy9(uint a)
	{
		return a * 9;
	}
}
