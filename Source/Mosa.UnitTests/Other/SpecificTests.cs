// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Other
{
	public static class SpecificTests

	{
		[MosaUnitTest(Series = "I8")]
		public static long SwitchI8_v2(long a)
		{
			switch (a)
			{
				case 0:
					return 0;

				case -1:
					return -1;

				case 2:
					return 2;

				case long.MinValue:
					return long.MinValue;

				case long.MaxValue:
					return long.MaxValue;

				default:
					return 42;
			}
		}

		[MosaUnitTest(Series = "I4")]
		public static int IncBy1(int a)
		{
			return a + 1;
		}

		[MosaUnitTest(Series = "I4")]
		public static int DecBy1(int a)
		{
			return a - 1;
		}

		[MosaUnitTest((byte)0)]
		[MosaUnitTest((byte)1)]
		[MosaUnitTest((byte)2)]
		[MosaUnitTest((byte)3)]
		[MosaUnitTest((byte)4)]
		[MosaUnitTest((byte)8)]
		[MosaUnitTest((byte)9)]
		[MosaUnitTest((byte)16)]
		[MosaUnitTest((byte)31)]
		[MosaUnitTest((byte)32)]
		[MosaUnitTest((byte)33)]
		[MosaUnitTest((byte)63)]
		[MosaUnitTest((byte)64)]
		public static ulong ShiftRight(byte count)
		{
			return 0xFFFFFFFFFFFFFFFFU >> count;
		}

		[MosaUnitTest((byte)0)]
		[MosaUnitTest((byte)1)]
		[MosaUnitTest((byte)2)]
		[MosaUnitTest((byte)3)]
		[MosaUnitTest((byte)4)]
		[MosaUnitTest((byte)8)]
		[MosaUnitTest((byte)9)]
		[MosaUnitTest((byte)16)]
		[MosaUnitTest((byte)31)]
		[MosaUnitTest((byte)32)]
		[MosaUnitTest((byte)33)]
		[MosaUnitTest((byte)63)]
		[MosaUnitTest((byte)64)]
		public static ulong ShiftRight2(byte count)
		{
			return 0xAAAAAAAAAAAAAAAAu >> count;
		}

		[MosaUnitTest((byte)0)]
		[MosaUnitTest((byte)1)]
		[MosaUnitTest((byte)2)]
		[MosaUnitTest((byte)3)]
		[MosaUnitTest((byte)4)]
		[MosaUnitTest((byte)8)]
		[MosaUnitTest((byte)9)]
		[MosaUnitTest((byte)16)]
		[MosaUnitTest((byte)31)]
		[MosaUnitTest((byte)32)]
		[MosaUnitTest((byte)33)]
		[MosaUnitTest((byte)63)]
		[MosaUnitTest((byte)64)]
		public static ulong ShiftRightB(byte count)
		{
			return 0xFFFFFFFFFFFFFFFFU >> (64 - count);
		}

		[MosaUnitTest((byte)0)]
		[MosaUnitTest((byte)1)]
		[MosaUnitTest((byte)2)]
		[MosaUnitTest((byte)3)]
		[MosaUnitTest((byte)4)]
		[MosaUnitTest((byte)8)]
		[MosaUnitTest((byte)9)]
		[MosaUnitTest((byte)16)]
		[MosaUnitTest((byte)31)]
		[MosaUnitTest((byte)32)]
		[MosaUnitTest((byte)33)]
		[MosaUnitTest((byte)63)]
		[MosaUnitTest((byte)64)]
		public static ulong ShiftLeft(byte count)
		{
			return 0xFFFFFFFFFFFFFFFFU << count;
		}

		[MosaUnitTest((byte)0)]
		[MosaUnitTest((byte)1)]
		[MosaUnitTest((byte)2)]
		[MosaUnitTest((byte)3)]
		[MosaUnitTest((byte)4)]
		[MosaUnitTest((byte)8)]
		[MosaUnitTest((byte)9)]
		[MosaUnitTest((byte)16)]
		[MosaUnitTest((byte)31)]
		[MosaUnitTest((byte)32)]
		[MosaUnitTest((byte)33)]
		[MosaUnitTest((byte)63)]
		[MosaUnitTest((byte)64)]
		public static ulong ShiftLeft2(byte count)
		{
			return 0xAAAAAAAAAAAAAAAAu << count;
		}

		[MosaUnitTest((byte)0)]
		[MosaUnitTest((byte)1)]
		[MosaUnitTest((byte)2)]
		[MosaUnitTest((byte)3)]
		[MosaUnitTest((byte)4)]
		[MosaUnitTest((byte)8)]
		[MosaUnitTest((byte)9)]
		[MosaUnitTest((byte)16)]
		[MosaUnitTest((byte)31)]
		[MosaUnitTest((byte)32)]
		[MosaUnitTest((byte)33)]
		[MosaUnitTest((byte)63)]
		[MosaUnitTest((byte)64)]
		public static ulong ShiftLeftB(byte count)
		{
			return 0xFFFFFFFFFFFFFFFFU << (64 - count);
		}

		[MosaUnitTest]
		public static uint SetBits32()
		{
			uint self = 0x00004000;
			return self.SetBits(12, 52, 0x00000007, 12);
		}

		[MosaUnitTest]
		public static ulong SetBits64()
		{
			ulong self = 0x00004000;
			return self.SetBits(12, 52, 0x00000007, 12);
		}

		[MosaUnitTest((ulong)9)]
		public static long I8ShrBy52(long x)
		{
			return x >> 52;
		}

		[MosaUnitTest((ulong)9)]
		[MosaUnitTest(ulong.MaxValue)]
		public static ulong U8ShrBy52(ulong x)
		{
			return x >> 52;
		}

		[MosaUnitTest((ulong)9)]
		[MosaUnitTest(ulong.MaxValue)]
		public static ulong U8ShrBy30(ulong x)
		{
			return x >> 30;
		}

		[MosaUnitTest((ulong)9)]
		[MosaUnitTest(ulong.MaxValue)]
		public static ulong U8ShrBy31(ulong x)
		{
			return x >> 31;
		}

		[MosaUnitTest(9, 9)]
		[MosaUnitTest(int.MaxValue, 9)]
		[MosaUnitTest(int.MaxValue, int.MaxValue)]
		public static bool BranchingEqI4(int x, int y)
		{
			var result = false;
			if (x == y)
			{
				result = true;
			}
			return result;
		}

		[MosaUnitTest(9u, 9u)]
		[MosaUnitTest(uint.MaxValue, 9u)]
		[MosaUnitTest(uint.MaxValue, uint.MaxValue)]
		public static bool BranchingEqU4(uint x, uint y)
		{
			var result = false;
			if (x == y)
			{
				result = true;
			}
			return result;
		}

		[MosaUnitTest(9L, 9L)]
		[MosaUnitTest(long.MaxValue, 9L)]
		[MosaUnitTest(long.MaxValue, long.MaxValue)]
		public static bool BranchingEqI8(long x, long y)
		{
			var result = false;
			if (x == y)
			{
				result = true;
			}
			return result;
		}

		[MosaUnitTest(9uL, 9uL)]
		[MosaUnitTest(ulong.MaxValue, 9uL)]
		[MosaUnitTest(ulong.MaxValue, ulong.MaxValue)]
		public static bool BranchingEqU8(ulong x, ulong y)
		{
			var result = false;
			if (x == y)
			{
				result = true;
			}
			return result;
		}
	}

	public static class Extension
	{
		public static uint SetBits(this uint self, byte index, byte count, uint value, byte sourceIndex)
		{
			value = value >> sourceIndex;
			uint mask = 0xFFFFFFFFU >> (32 - count);
			uint bits = (value & mask) << index;
			return (self & ~(mask << index)) | bits;
		}

		public static ulong SetBits(this ulong self, byte index, byte count, ulong value, byte sourceIndex)
		{
			value = value >> sourceIndex;
			ulong mask = 0xFFFFFFFFFFFFFFFFU >> (64 - count);
			ulong bits = (value & mask) << index;
			return (self & ~(mask << index)) | bits;
		}
	}
}
