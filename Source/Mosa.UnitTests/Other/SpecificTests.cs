// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long R8ToI8(double value)
		{
			var rawValue = (ulong)System.BitConverter.DoubleToInt64Bits(value);

			const int MantissaBits = 52;
			const int ExponentBits = 11;
			const int ExponentBias = 1023;
			const ulong ExponentMask = 0x7FF0_0000_0000_0000;
			const ulong SignMask = 0x8000_0000_0000_0000;
			const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN
			const ulong RawPositiveInfinity = 0x7FF0_0000_0000_0000;
			const ulong RawNegativeInfinity = RawPositiveInfinity ^ SignMask;

			ushort RawExponent = (ushort)((rawValue & ExponentMask) >> MantissaBits);
			short Exponent = (short)(RawExponent - ExponentBias);

			if (rawValue == RawPositiveInfinity || rawValue == RawNegativeInfinity || rawValue == RawNaN)
			{
				return long.MinValue;
			}

			if (Exponent < 0)
			{
				return 0;
			}

			ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

			int shift = MantissaBits - Exponent;
			var mantissa = (long)(RawMantissa | (1uL << MantissaBits));
			long result = shift < 0 ? mantissa << -shift : mantissa >> shift;
			return (rawValue & SignMask) == 0 ? result : -result;
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
