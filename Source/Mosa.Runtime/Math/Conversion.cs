// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.Runtime.Math;

internal static class Conversion
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static long R4ToI8(float value)
	{
		var rawValue = (ulong)System.BitConverter.DoubleToInt64Bits(value);

		const byte MantissaBits = 52;
		const short ExponentBias = 1075;
		const ulong ExponentMask = 0x7FF0_0000_0000_0000;
		const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN
		const ulong RawPositiveInfinity = 0x7FF0_0000_0000_0000;
		const ulong RawNegativeInfinity = 0xFFF0_0000_0000_0000;

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(ExponentBias - RawExponent);
		ulong Sign = rawValue >> 63;

		if (rawValue is RawPositiveInfinity or RawNegativeInfinity or RawNaN || Exponent < -11)
		{
			return long.MinValue;
		}

		if (Exponent > 52)
		{
			return 0L;
		}

		ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

		int shift = Exponent;
		var mantissa = RawMantissa | (1uL << MantissaBits);
		long result = (long)(shift <= 0 ? mantissa << -shift : mantissa >> shift);
		return Sign == 0uL ? result : -result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static long R8ToI8(double value)
	{
		var rawValue = (ulong)System.BitConverter.DoubleToInt64Bits(value);

		const byte MantissaBits = 52;
		const short ExponentBias = 1075;
		const ulong ExponentMask = 0x7FF0_0000_0000_0000;
		const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN
		const ulong RawPositiveInfinity = 0x7FF0_0000_0000_0000;
		const ulong RawNegativeInfinity = 0xFFF0_0000_0000_0000;

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(ExponentBias - RawExponent);
		ulong Sign = rawValue >> 63;

		if (rawValue is RawPositiveInfinity or RawNegativeInfinity or RawNaN || Exponent < -11)
		{
			return long.MinValue;
		}

		if (Exponent > 52)
		{
			return 0L;
		}

		ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

		int shift = Exponent;
		var mantissa = RawMantissa | (1uL << MantissaBits);
		long result = (long)(shift <= 0 ? mantissa << -shift : mantissa >> shift);
		return Sign == 0uL ? result : -result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ulong R4ToU8(float value)
	{
		var rawValue = (ulong)System.BitConverter.DoubleToInt64Bits(value);

		const byte MantissaBits = 52;
		const short ExponentBias = 1075;
		const ulong ExponentMask = 0x7FF0_0000_0000_0000;
		const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN
		const ulong RawPositiveInfinity = 0x7FF0_0000_0000_0000;
		const ulong RawNegativeInfinity = 0xFFF0_0000_0000_0000;

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(ExponentBias - RawExponent);
		ulong Sign = rawValue >> 63;

		if (rawValue == RawNegativeInfinity)
		{
			return unchecked((ulong)long.MinValue);
		}

		if (rawValue is RawPositiveInfinity or RawNaN)
		{
			return ulong.MinValue;
		}

		if (Exponent < -11)
		{
			return Sign << 63;
		}

		if (Exponent > 52)
		{
			return 0L;
		}

		ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

		int shift = Exponent;
		var mantissa = RawMantissa | (1uL << MantissaBits);
		long result = unchecked((long)(shift <= 0 ? mantissa << -shift : mantissa >> shift));
		return unchecked((ulong)(Sign == 0uL ? result : -result));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ulong R8ToU8(double value)
	{
		var rawValue = (ulong)System.BitConverter.DoubleToInt64Bits(value);

		const byte MantissaBits = 52;
		const short ExponentBias = 1075;
		const ulong ExponentMask = 0x7FF0_0000_0000_0000;
		const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN
		const ulong RawPositiveInfinity = 0x7FF0_0000_0000_0000;
		const ulong RawNegativeInfinity = 0xFFF0_0000_0000_0000;

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(ExponentBias - RawExponent);
		ulong Sign = rawValue >> 63;

		if (rawValue == RawNegativeInfinity)
		{
			return unchecked((ulong)long.MinValue);
		}

		if (rawValue is RawPositiveInfinity or RawNaN)
		{
			return ulong.MinValue;
		}

		if (Exponent < -11)
		{
			return Sign << 63;
		}

		if (Exponent > 52)
		{
			return 0L;
		}

		ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

		int shift = Exponent;
		var mantissa = RawMantissa | (1uL << MantissaBits);
		long result = unchecked((long)(shift <= 0 ? mantissa << -shift : mantissa >> shift));
		return unchecked((ulong)(Sign == 0uL ? result : -result));
	}
}
