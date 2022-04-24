// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.Runtime.Math
{
	internal static class Conversion
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long R4ToI8(float value)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong R4ToU8(float value)
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
				return unchecked((ulong)long.MinValue);
			}

			if (Exponent < 0)
			{
				return 0;
			}

			ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

			int shift = MantissaBits - Exponent;
			var mantissa = (long)(RawMantissa | (1uL << MantissaBits));
			long result = shift < 0 ? mantissa << -shift : mantissa >> shift;
			return unchecked((ulong)((rawValue & SignMask) == 0 ? result : -result));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong R8ToU8(double value)
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
				return unchecked((ulong)long.MinValue);
			}

			if (Exponent < 0)
			{
				return 0;
			}

			ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

			int shift = MantissaBits - Exponent;
			var mantissa = (long)(RawMantissa | (1uL << MantissaBits));
			long result = shift < 0 ? mantissa << -shift : mantissa >> shift;
			return unchecked((ulong)((rawValue & SignMask) == 0 ? result : -result));
		}
	}
}
