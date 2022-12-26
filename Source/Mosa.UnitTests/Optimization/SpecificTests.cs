// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Optimization
{
	public static class SpecificTests
	{
		[MosaUnitTest(Series = "I4I4")]
		public static uint Xor32Xor32(uint x, uint y)
		{
			return x ^ (x ^ y);
		}

		[MosaUnitTest(Series = "I8I8")]
		public static ulong Xor64Xor64(ulong x, ulong y)
		{
			return x ^ (x ^ y);
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
	}
}
