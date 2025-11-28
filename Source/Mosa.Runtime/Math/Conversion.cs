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
		const short ExponentBias = 1023;
		const ulong ExponentMask = 0x7FF0_0000_0000_0000;
		const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(RawExponent - ExponentBias);
		ulong Sign = rawValue >> 63;

		if (rawValue == RawNaN || Exponent < 0)
		{
			return 0L;
		}

		if (Exponent > 63)
		{
			return Sign == 0uL ? long.MaxValue : long.MinValue;
		}

		ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

		int shift = Exponent - MantissaBits;
		var mantissa = RawMantissa | (1uL << MantissaBits);
		long result = (long)(shift <= 0 ? mantissa >> -shift : mantissa << shift);
		return Sign == 0uL ? result : -result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int R8ToI4(double value)
	{
		var rawValue = (ulong)System.BitConverter.DoubleToInt64Bits(value);

		const byte MantissaBits = 52;
		const short ExponentBias = 1023;
		const ulong ExponentMask = 0x7FF0_0000_0000_0000;
		const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(RawExponent - ExponentBias);
		ulong Sign = rawValue >> 63;

		if (rawValue == RawNaN || Exponent < 0)
		{
			return 0;
		}

		if (Exponent > 31)
		{
			return Sign == 0uL ? int.MaxValue : int.MinValue;
		}

		ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

		int shift = Exponent - MantissaBits;
		var mantissa = RawMantissa | (1uL << MantissaBits);
		int result = (int)(shift <= 0 ? mantissa >> -shift : mantissa << shift);
		return Sign == 0uL ? result : -result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static long R8ToI8(double value)
	{
		var rawValue = (ulong)System.BitConverter.DoubleToInt64Bits(value);

		const byte MantissaBits = 52;
		const short ExponentBias = 1023;
		const ulong ExponentMask = 0x7FF0_0000_0000_0000;
		const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(RawExponent - ExponentBias);
		ulong Sign = rawValue >> 63;

		if (rawValue == RawNaN || Exponent < 0)
		{
			return 0L;
		}

		if (Exponent > 63)
		{
			return Sign == 0uL ? long.MaxValue : long.MinValue;
		}

		ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

		int shift = Exponent - MantissaBits;
		var mantissa = RawMantissa | (1uL << MantissaBits);
		long result = (long)(shift <= 0 ? mantissa >> -shift : mantissa << shift);
		return Sign == 0uL ? result : -result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint R4ToU4(float value)
	{
		var rawValue = (uint)System.BitConverter.SingleToInt32Bits(value);

		const byte MantissaBits = 23;
		const short ExponentBias = 127;
		const uint ExponentMask = 0x7F80_0000;
		const uint RawNaN = 0xFFC0_0000; // Same as float.NaN

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(RawExponent - ExponentBias);
		uint Sign = rawValue >> 31;

		if (Sign == 1u || Exponent < 0 || rawValue == RawNaN)
		{
			return 0;
		}

		if (Exponent > 31)
		{
			return uint.MaxValue;
		}

		uint RawMantissa = rawValue & 0x007F_FFFF;

		int shift = Exponent - MantissaBits;
		var mantissa = RawMantissa | (1u << MantissaBits);
		int result = unchecked((int)(shift <= 0 ? mantissa >> -shift : mantissa << shift));
		return unchecked((uint)(Sign == 0u ? result : -result));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ulong R4ToU8(float value)
	{
		var rawValue = (ulong)System.BitConverter.DoubleToInt64Bits(value);

		const byte MantissaBits = 52;
		const short ExponentBias = 1023;
		const ulong ExponentMask = 0x7FF0_0000_0000_0000;
		const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(RawExponent - ExponentBias);
		ulong Sign = rawValue >> 63;

		if (Sign == 1uL || Exponent < 0 || rawValue == RawNaN)
		{
			return 0L;
		}

		if (Exponent > 63)
		{
			return ulong.MaxValue;
		}

		ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

		int shift = Exponent - MantissaBits;
		var mantissa = RawMantissa | (1uL << MantissaBits);
		long result = unchecked((long)(shift <= 0 ? mantissa >> -shift : mantissa << shift));
		return unchecked((ulong)(Sign == 0uL ? result : -result));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint R8ToU4(double value)
	{
		var rawValue = (ulong)System.BitConverter.DoubleToInt64Bits(value);

		const byte MantissaBits = 52;
		const short ExponentBias = 1023;
		const ulong ExponentMask = 0x7FF0_0000_0000_0000;
		const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(RawExponent - ExponentBias);
		ulong Sign = rawValue >> 63;

		if (Sign == 1uL || Exponent < 0 || rawValue == RawNaN)
		{
			return 0u;
		}

		if (Exponent > 31)
		{
			return uint.MaxValue;
		}

		ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

		int shift = Exponent - MantissaBits;
		var mantissa = RawMantissa | (1uL << MantissaBits);
		int result = unchecked((int)(shift <= 0 ? mantissa >> -shift : mantissa << shift));
		return unchecked((uint)(Sign == 0uL ? result : -result));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ulong R8ToU8(double value)
	{
		var rawValue = (ulong)System.BitConverter.DoubleToInt64Bits(value);

		const byte MantissaBits = 52;
		const short ExponentBias = 1023;
		const ulong ExponentMask = 0x7FF0_0000_0000_0000;
		const ulong RawNaN = 0xFFF8_0000_0000_0000; // Same as double.NaN

		short RawExponent = (short)((rawValue & ExponentMask) >> MantissaBits);
		short Exponent = (short)(RawExponent - ExponentBias);
		ulong Sign = rawValue >> 63;

		if (Sign == 1uL || Exponent < 0 || rawValue == RawNaN)
		{
			return 0L;
		}

		if (Exponent > 63)
		{
			return ulong.MaxValue;
		}

		ulong RawMantissa = rawValue & 0x000F_FFFF_FFFF_FFFF;

		int shift = Exponent - MantissaBits;
		var mantissa = RawMantissa | (1uL << MantissaBits);
		long result = unchecked((long)(shift <= 0 ? mantissa >> -shift : mantissa << shift));
		return unchecked((ulong)(Sign == 0uL ? result : -result));
	}
}
