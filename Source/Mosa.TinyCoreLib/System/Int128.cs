using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

public readonly struct Int128 : IComparable, IComparable<Int128>, IEquatable<Int128>, IFormattable, IParsable<Int128>, ISpanFormattable, ISpanParsable<Int128>, IAdditionOperators<Int128, Int128, Int128>, IAdditiveIdentity<Int128, Int128>, IBinaryInteger<Int128>, IBinaryNumber<Int128>, IBitwiseOperators<Int128, Int128, Int128>, IComparisonOperators<Int128, Int128, bool>, IEqualityOperators<Int128, Int128, bool>, IDecrementOperators<Int128>, IDivisionOperators<Int128, Int128, Int128>, IIncrementOperators<Int128>, IModulusOperators<Int128, Int128, Int128>, IMultiplicativeIdentity<Int128, Int128>, IMultiplyOperators<Int128, Int128, Int128>, INumber<Int128>, INumberBase<Int128>, ISubtractionOperators<Int128, Int128, Int128>, IUnaryNegationOperators<Int128, Int128>, IUnaryPlusOperators<Int128, Int128>, IUtf8SpanFormattable, IUtf8SpanParsable<Int128>, IShiftOperators<Int128, int, Int128>, IMinMaxValue<Int128>, ISignedNumber<Int128>
{
	private readonly int _dummyPrimitive;

	public static Int128 MaxValue
	{
		get
		{
			throw null;
		}
	}

	public static Int128 MinValue
	{
		get
		{
			throw null;
		}
	}

	public static Int128 NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public static Int128 One
	{
		get
		{
			throw null;
		}
	}

	static Int128 IAdditiveIdentity<Int128, Int128>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static Int128 IBinaryNumber<Int128>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static Int128 IMultiplicativeIdentity<Int128, Int128>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<Int128>.Radix
	{
		get
		{
			throw null;
		}
	}

	public static Int128 Zero
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public Int128(ulong upper, ulong lower)
	{
		throw null;
	}

	public static Int128 Abs(Int128 value)
	{
		throw null;
	}

	public static Int128 Clamp(Int128 value, Int128 min, Int128 max)
	{
		throw null;
	}

	public int CompareTo(Int128 value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static Int128 CopySign(Int128 value, Int128 sign)
	{
		throw null;
	}

	public static Int128 CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static Int128 CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static Int128 CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (Int128 Quotient, Int128 Remainder) DivRem(Int128 left, Int128 right)
	{
		throw null;
	}

	public bool Equals(Int128 other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool IsEvenInteger(Int128 value)
	{
		throw null;
	}

	public static bool IsNegative(Int128 value)
	{
		throw null;
	}

	public static bool IsOddInteger(Int128 value)
	{
		throw null;
	}

	public static bool IsPositive(Int128 value)
	{
		throw null;
	}

	public static bool IsPow2(Int128 value)
	{
		throw null;
	}

	public static Int128 LeadingZeroCount(Int128 value)
	{
		throw null;
	}

	public static Int128 Log2(Int128 value)
	{
		throw null;
	}

	public static Int128 Max(Int128 x, Int128 y)
	{
		throw null;
	}

	public static Int128 MaxMagnitude(Int128 x, Int128 y)
	{
		throw null;
	}

	public static Int128 Min(Int128 x, Int128 y)
	{
		throw null;
	}

	public static Int128 MinMagnitude(Int128 x, Int128 y)
	{
		throw null;
	}

	public static Int128 operator +(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator &(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator |(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator checked +(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator checked --(Int128 value)
	{
		throw null;
	}

	public static Int128 operator checked /(Int128 left, Int128 right)
	{
		throw null;
	}

	public static explicit operator checked Int128(double value)
	{
		throw null;
	}

	public static explicit operator checked byte(Int128 value)
	{
		throw null;
	}

	public static explicit operator checked char(Int128 value)
	{
		throw null;
	}

	public static explicit operator checked short(Int128 value)
	{
		throw null;
	}

	public static explicit operator checked int(Int128 value)
	{
		throw null;
	}

	public static explicit operator checked long(Int128 value)
	{
		throw null;
	}

	public static explicit operator checked IntPtr(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked sbyte(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked ushort(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked uint(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked ulong(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked UInt128(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked UIntPtr(Int128 value)
	{
		throw null;
	}

	public static explicit operator checked Int128(float value)
	{
		throw null;
	}

	public static Int128 operator checked ++(Int128 value)
	{
		throw null;
	}

	public static Int128 operator checked *(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator checked -(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator checked -(Int128 value)
	{
		throw null;
	}

	public static Int128 operator --(Int128 value)
	{
		throw null;
	}

	public static Int128 operator /(Int128 left, Int128 right)
	{
		throw null;
	}

	public static bool operator ==(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator ^(Int128 left, Int128 right)
	{
		throw null;
	}

	public static explicit operator Int128(decimal value)
	{
		throw null;
	}

	public static explicit operator Int128(double value)
	{
		throw null;
	}

	public static explicit operator byte(Int128 value)
	{
		throw null;
	}

	public static explicit operator char(Int128 value)
	{
		throw null;
	}

	public static explicit operator decimal(Int128 value)
	{
		throw null;
	}

	public static explicit operator double(Int128 value)
	{
		throw null;
	}

	public static explicit operator Half(Int128 value)
	{
		throw null;
	}

	public static explicit operator short(Int128 value)
	{
		throw null;
	}

	public static explicit operator int(Int128 value)
	{
		throw null;
	}

	public static explicit operator long(Int128 value)
	{
		throw null;
	}

	public static explicit operator IntPtr(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator sbyte(Int128 value)
	{
		throw null;
	}

	public static explicit operator float(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator UInt128(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ushort(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator uint(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ulong(Int128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator UIntPtr(Int128 value)
	{
		throw null;
	}

	public static explicit operator Int128(float value)
	{
		throw null;
	}

	public static bool operator >(Int128 left, Int128 right)
	{
		throw null;
	}

	public static bool operator >=(Int128 left, Int128 right)
	{
		throw null;
	}

	public static implicit operator Int128(byte value)
	{
		throw null;
	}

	public static implicit operator Int128(char value)
	{
		throw null;
	}

	public static implicit operator Int128(short value)
	{
		throw null;
	}

	public static implicit operator Int128(int value)
	{
		throw null;
	}

	public static implicit operator Int128(long value)
	{
		throw null;
	}

	public static implicit operator Int128(IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Int128(sbyte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Int128(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Int128(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Int128(ulong value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Int128(UIntPtr value)
	{
		throw null;
	}

	public static Int128 operator ++(Int128 value)
	{
		throw null;
	}

	public static bool operator !=(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator <<(Int128 value, int shiftAmount)
	{
		throw null;
	}

	public static bool operator <(Int128 left, Int128 right)
	{
		throw null;
	}

	public static bool operator <=(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator %(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator *(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator ~(Int128 value)
	{
		throw null;
	}

	public static Int128 operator >>(Int128 value, int shiftAmount)
	{
		throw null;
	}

	public static Int128 operator -(Int128 left, Int128 right)
	{
		throw null;
	}

	public static Int128 operator -(Int128 value)
	{
		throw null;
	}

	public static Int128 operator +(Int128 value)
	{
		throw null;
	}

	public static Int128 operator >>>(Int128 value, int shiftAmount)
	{
		throw null;
	}

	public static Int128 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static Int128 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static Int128 Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static Int128 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static Int128 Parse(string s)
	{
		throw null;
	}

	public static Int128 Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static Int128 Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static Int128 Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static Int128 PopCount(Int128 value)
	{
		throw null;
	}

	public static Int128 RotateLeft(Int128 value, int rotateAmount)
	{
		throw null;
	}

	public static Int128 RotateRight(Int128 value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(Int128 value)
	{
		throw null;
	}

	int IBinaryInteger<Int128>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<Int128>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<Int128>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int128 value)
	{
		throw null;
	}

	static bool IBinaryInteger<Int128>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int128 value)
	{
		throw null;
	}

	bool IBinaryInteger<Int128>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<Int128>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsCanonical(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsComplexNumber(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsFinite(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsImaginaryNumber(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsInfinity(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsInteger(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsNaN(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsNegativeInfinity(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsNormal(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsPositiveInfinity(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsRealNumber(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsSubnormal(Int128 value)
	{
		throw null;
	}

	static bool INumberBase<Int128>.IsZero(Int128 value)
	{
		throw null;
	}

	static Int128 INumberBase<Int128>.MaxMagnitudeNumber(Int128 x, Int128 y)
	{
		throw null;
	}

	static Int128 INumberBase<Int128>.MinMagnitudeNumber(Int128 x, Int128 y)
	{
		throw null;
	}

	static bool INumberBase<Int128>.TryConvertFromChecked<TOther>(TOther value, out Int128 result)
	{
		throw null;
	}

	static bool INumberBase<Int128>.TryConvertFromSaturating<TOther>(TOther value, out Int128 result)
	{
		throw null;
	}

	static bool INumberBase<Int128>.TryConvertFromTruncating<TOther>(TOther value, out Int128 result)
	{
		throw null;
	}

	static bool INumberBase<Int128>.TryConvertToChecked<TOther>(Int128 value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<Int128>.TryConvertToSaturating<TOther>(Int128 value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<Int128>.TryConvertToTruncating<TOther>(Int128 value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static Int128 INumber<Int128>.MaxNumber(Int128 x, Int128 y)
	{
		throw null;
	}

	static Int128 INumber<Int128>.MinNumber(Int128 x, Int128 y)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public string ToString(IFormatProvider? provider)
	{
		throw null;
	}

	public string ToString([StringSyntax("NumericFormat")] string? format)
	{
		throw null;
	}

	public string ToString([StringSyntax("NumericFormat")] string? format, IFormatProvider? provider)
	{
		throw null;
	}

	public static Int128 TrailingZeroCount(Int128 value)
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax("NumericFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax("NumericFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out Int128 result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Int128 result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out Int128 result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Int128 result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Int128 result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out Int128 result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Int128 result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Int128 result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out Int128 result)
	{
		throw null;
	}
}
