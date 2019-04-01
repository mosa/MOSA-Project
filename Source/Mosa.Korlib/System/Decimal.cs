// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Implements the Decimal data type. The Decimal data type can
	// represent values ranging from -79,228,162,514,264,337,593,543,950,335 to
	// 79,228,162,514,264,337,593,543,950,335 with 28 significant digits. The
	// Decimal data type is ideally suited to financial calculations that
	// require a large number of significant digits and no round-off errors.
	//
	// The finite set of values of type Decimal are of the form m
	// / 10e, where m is an integer such that
	// -296 <; m <; 296, and e is an integer
	// between 0 and 28 inclusive.
	//
	// Contrary to the float and double data types, decimal
	// fractional numbers such as 0.1 can be represented exactly in the
	// Decimal representation. In the float and double
	// representations, such numbers are often infinite fractions, making those
	// representations more prone to round-off errors.
	//
	// The Decimal class implements widening conversions from the
	// ubyte, char, short, int, and long types
	// to Decimal. These widening conversions never loose any information
	// and never throw exceptions. The Decimal class also implements
	// narrowing conversions from Decimal to ubyte, char,
	// short, int, and long. These narrowing conversions round
	// the Decimal value towards zero to the nearest integer, and then
	// converts that integer to the destination type. An OverflowException
	// is thrown if the result is not within the range of the destination type.
	//
	// The Decimal class provides a widening conversion from
	// Currency to Decimal. This widening conversion never loses any
	// information and never throws exceptions. The Currency class provides
	// a narrowing conversion from Decimal to Currency. This
	// narrowing conversion rounds the Decimal to four decimals and then
	// converts that number to a Currency. An OverflowException
	// is thrown if the result is not within the range of the Currency type.
	//
	// The Decimal class provides narrowing conversions to and from the
	// float and double types. A conversion from Decimal to
	// float or double may loose precision, but will not loose
	// information about the overall magnitude of the numeric value, and will never
	// throw an exception. A conversion from float or double to
	// Decimal throws an OverflowException if the value is not within
	// the range of the Decimal type.
	[StructLayout(LayoutKind.Sequential)]
	[Serializable]
	public /*readonly*/ partial struct Decimal
	{
		// Sign mask for the flags field. A value of zero in this bit indicates a
		// positive Decimal value, and a value of one in this bit indicates a
		// negative Decimal value.
		//
		// Look at OleAut's DECIMAL_NEG constant to check for negative values
		// in native code.
		private const int SignMask = unchecked((int)0x80000000);

		// Scale mask for the flags field. This byte in the flags field contains
		// the power of 10 to divide the Decimal value by. The scale byte must
		// contain a value between 0 and 28 inclusive.
		private const int ScaleMask = 0x00FF0000;

		// Number of bits scale is shifted by.
		private const int ScaleShift = 16;

		// Constant representing the Decimal value 0.
		public const decimal Zero = 0m;

		// Constant representing the Decimal value 1.
		public const decimal One = 1m;

		// Constant representing the Decimal value -1.
		public const decimal MinusOne = -1m;

		// Constant representing the largest possible Decimal value. The value of
		// this constant is 79,228,162,514,264,337,593,543,950,335.
		public const decimal MaxValue = 79228162514264337593543950335m;

		// Constant representing the smallest possible Decimal value. The value of
		// this constant is -79,228,162,514,264,337,593,543,950,335.
		public const decimal MinValue = -79228162514264337593543950335m;

		// The lo, mid, hi, and flags fields contain the representation of the
		// Decimal value. The lo, mid, and hi fields contain the 96-bit integer
		// part of the Decimal. Bits 0-15 (the lower word) of the flags field are
		// unused and must be zero; bits 16-23 contain must contain a value between
		// 0 and 28, indicating the power of 10 to divide the 96-bit integer part
		// by to produce the Decimal value; bits 24-30 are unused and must be zero;
		// and finally bit 31 indicates the sign of the Decimal value, 0 meaning
		// positive and 1 meaning negative.
		//
		// NOTE: Do not change the order in which these fields are declared. The
		// native methods in this class rely on this particular order.
		// Do not rename (binary serialization).
		private readonly int flags;

		private readonly int hi;
		private readonly int lo;
		private readonly int mid;

		// Constructs a Decimal from an integer value.
		//
		public Decimal(int value)
		{
			if (value >= 0)
			{
				flags = 0;
			}
			else
			{
				flags = SignMask;
				value = -value;
			}
			lo = value;
			mid = 0;
			hi = 0;
		}

		// Constructs a Decimal from an unsigned integer value.
		//
		public Decimal(uint value)
		{
			flags = 0;
			lo = (int)value;
			mid = 0;
			hi = 0;
		}

		// Constructs a Decimal from a long value.
		//
		public Decimal(long value)
		{
			if (value >= 0)
			{
				flags = 0;
			}
			else
			{
				flags = SignMask;
				value = -value;
			}
			lo = (int)value;
			mid = (int)(value >> 32);
			hi = 0;
		}

		// Constructs a Decimal from an unsigned long value.
		//
		public Decimal(ulong value)
		{
			flags = 0;
			lo = (int)value;
			mid = (int)(value >> 32);
			hi = 0;
		}

		//
		// Decimal <==> Currency conversion.
		//
		// A Currency represents a positive or negative decimal value with 4 digits past the decimal point. The actual Int64 representation used by these methods
		// is the currency value multiplied by 10,000. For example, a currency value of $12.99 would be represented by the Int64 value 129,900.
		//
		public static decimal FromOACurrency(long cy)
		{
			ulong absoluteCy; // has to be ulong to accommodate the case where cy == long.MinValue.
			bool isNegative = false;
			if (cy < 0)
			{
				isNegative = true;
				absoluteCy = (ulong)(-cy);
			}
			else
			{
				absoluteCy = (ulong)cy;
			}

			// In most cases, FromOACurrency() produces a Decimal with Scale set to 4. Unless, that is, some of the trailing digits past the decimal point are zero,
			// in which case, for compatibility with .Net, we reduce the Scale by the number of zeros. While the result is still numerically equivalent, the scale does
			// affect the ToString() value. In particular, it prevents a converted currency value of $12.95 from printing uglily as "12.9500".
			int scale = 4;
			if (absoluteCy != 0)  // For compatibility, a currency of 0 emits the Decimal "0.0000" (scale set to 4).
			{
				while (scale != 0 && ((absoluteCy % 10) == 0))
				{
					scale--;
					absoluteCy /= 10;
				}
			}

			return new decimal((int)absoluteCy, (int)(absoluteCy >> 32), 0, isNegative, (byte)scale);
		}

		private static bool IsValid(int flags) => (flags & ~(SignMask | ScaleMask)) == 0 && ((uint)(flags & ScaleMask) <= (28 << ScaleShift));

		// Constructs a Decimal from an integer array containing a binary
		// representation. The bits argument must be a non-null integer
		// array with four elements. bits[0], bits[1], and
		// bits[2] contain the low, middle, and high 32 bits of the 96-bit
		// integer part of the Decimal. bits[3] contains the scale factor
		// and sign of the Decimal: bits 0-15 (the lower word) are unused and must
		// be zero; bits 16-23 must contain a value between 0 and 28, indicating
		// the power of 10 to divide the 96-bit integer part by to produce the
		// Decimal value; bits 24-30 are unused and must be zero; and finally bit
		// 31 indicates the sign of the Decimal value, 0 meaning positive and 1
		// meaning negative.
		//
		// Note that there are several possible binary representations for the
		// same numeric value. For example, the value 1 can be represented as {1,
		// 0, 0, 0} (integer value 1 with a scale factor of 0) and equally well as
		// {1000, 0, 0, 0x30000} (integer value 1000 with a scale factor of 3).
		// The possible binary representations of a particular value are all
		// equally valid, and all are numerically equivalent.
		//
		public Decimal(int[] bits)
		{
			if (bits == null)
				throw new ArgumentNullException(nameof(bits));
			if (bits.Length == 4)
			{
				int f = bits[3];
				if (IsValid(f))
				{
					lo = bits[0];
					mid = bits[1];
					hi = bits[2];
					flags = f;
					return;
				}
			}
			throw new ArgumentException("SR.Arg_DecBitCtor");
		}

		// Constructs a Decimal from its constituent parts.
		//
		public Decimal(int lo, int mid, int hi, bool isNegative, byte scale)
		{
			if (scale > 28)
				throw new ArgumentOutOfRangeException(nameof(scale), "SR.ArgumentOutOfRange_DecimalScale");
			this.lo = lo;
			this.mid = mid;
			this.hi = hi;
			flags = ((int)scale) << 16;
			if (isNegative)
				flags |= SignMask;
		}

		// Constructs a Decimal from its constituent parts.
		private Decimal(int lo, int mid, int hi, int flags)
		{
			if (IsValid(flags))
			{
				this.lo = lo;
				this.mid = mid;
				this.hi = hi;
				this.flags = flags;
				return;
			}
			throw new ArgumentException("SR.Arg_DecBitCtor");
		}

		// Returns a binary representation of a Decimal. The return value is an
		// integer array with four elements. Elements 0, 1, and 2 contain the low,
		// middle, and high 32 bits of the 96-bit integer part of the Decimal.
		// Element 3 contains the scale factor and sign of the Decimal: bits 0-15
		// (the lower word) are unused; bits 16-23 contain a value between 0 and
		// 28, indicating the power of 10 to divide the 96-bit integer part by to
		// produce the Decimal value; bits 24-30 are unused; and finally bit 31
		// indicates the sign of the Decimal value, 0 meaning positive and 1
		// meaning negative.
		//
		public static int[] GetBits(decimal d)
		{
			return new int[] { d.lo, d.mid, d.hi, d.flags };
		}
	}
}
