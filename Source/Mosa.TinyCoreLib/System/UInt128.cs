using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

[CLSCompliant(false)]
public readonly struct UInt128 : IComparable, IComparable<UInt128>, IEquatable<UInt128>, IFormattable, IParsable<UInt128>, ISpanFormattable, ISpanParsable<UInt128>, IAdditionOperators<UInt128, UInt128, UInt128>, IAdditiveIdentity<UInt128, UInt128>, IBinaryInteger<UInt128>, IBinaryNumber<UInt128>, IBitwiseOperators<UInt128, UInt128, UInt128>, IComparisonOperators<UInt128, UInt128, bool>, IEqualityOperators<UInt128, UInt128, bool>, IDecrementOperators<UInt128>, IDivisionOperators<UInt128, UInt128, UInt128>, IIncrementOperators<UInt128>, IModulusOperators<UInt128, UInt128, UInt128>, IMultiplicativeIdentity<UInt128, UInt128>, IMultiplyOperators<UInt128, UInt128, UInt128>, INumber<UInt128>, INumberBase<UInt128>, ISubtractionOperators<UInt128, UInt128, UInt128>, IUnaryNegationOperators<UInt128, UInt128>, IUnaryPlusOperators<UInt128, UInt128>, IUtf8SpanFormattable, IUtf8SpanParsable<UInt128>, IShiftOperators<UInt128, int, UInt128>, IMinMaxValue<UInt128>, IUnsignedNumber<UInt128>
{
	private readonly int _dummyPrimitive;

	public static UInt128 MaxValue
	{
		get
		{
			throw null;
		}
	}

	public static UInt128 MinValue
	{
		get
		{
			throw null;
		}
	}

	public static UInt128 One
	{
		get
		{
			throw null;
		}
	}

	static UInt128 IAdditiveIdentity<UInt128, UInt128>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static UInt128 IBinaryNumber<UInt128>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static UInt128 IMultiplicativeIdentity<UInt128, UInt128>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<UInt128>.Radix
	{
		get
		{
			throw null;
		}
	}

	public static UInt128 Zero
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public UInt128(ulong upper, ulong lower)
	{
		throw null;
	}

	public static UInt128 Clamp(UInt128 value, UInt128 min, UInt128 max)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public int CompareTo(UInt128 value)
	{
		throw null;
	}

	public static UInt128 CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static UInt128 CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static UInt128 CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (UInt128 Quotient, UInt128 Remainder) DivRem(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(UInt128 other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool IsEvenInteger(UInt128 value)
	{
		throw null;
	}

	public static bool IsOddInteger(UInt128 value)
	{
		throw null;
	}

	public static bool IsPow2(UInt128 value)
	{
		throw null;
	}

	public static UInt128 LeadingZeroCount(UInt128 value)
	{
		throw null;
	}

	public static UInt128 Log2(UInt128 value)
	{
		throw null;
	}

	public static UInt128 Max(UInt128 x, UInt128 y)
	{
		throw null;
	}

	public static UInt128 Min(UInt128 x, UInt128 y)
	{
		throw null;
	}

	public static UInt128 operator +(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator &(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator |(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator checked +(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator checked --(UInt128 value)
	{
		throw null;
	}

	public static UInt128 operator checked /(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static explicit operator checked UInt128(double value)
	{
		throw null;
	}

	public static explicit operator checked UInt128(short value)
	{
		throw null;
	}

	public static explicit operator checked UInt128(int value)
	{
		throw null;
	}

	public static explicit operator checked UInt128(long value)
	{
		throw null;
	}

	public static explicit operator checked UInt128(IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked UInt128(sbyte value)
	{
		throw null;
	}

	public static explicit operator checked UInt128(float value)
	{
		throw null;
	}

	public static explicit operator checked byte(UInt128 value)
	{
		throw null;
	}

	public static explicit operator checked char(UInt128 value)
	{
		throw null;
	}

	public static explicit operator checked short(UInt128 value)
	{
		throw null;
	}

	public static explicit operator checked int(UInt128 value)
	{
		throw null;
	}

	public static explicit operator checked long(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked Int128(UInt128 value)
	{
		throw null;
	}

	public static explicit operator checked IntPtr(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked sbyte(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked ushort(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked uint(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked ulong(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked UIntPtr(UInt128 value)
	{
		throw null;
	}

	public static UInt128 operator checked ++(UInt128 value)
	{
		throw null;
	}

	public static UInt128 operator checked *(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator checked -(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator checked -(UInt128 value)
	{
		throw null;
	}

	public static UInt128 operator --(UInt128 value)
	{
		throw null;
	}

	public static UInt128 operator /(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static bool operator ==(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator ^(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static explicit operator UInt128(decimal value)
	{
		throw null;
	}

	public static explicit operator UInt128(double value)
	{
		throw null;
	}

	public static explicit operator UInt128(short value)
	{
		throw null;
	}

	public static explicit operator UInt128(int value)
	{
		throw null;
	}

	public static explicit operator UInt128(long value)
	{
		throw null;
	}

	public static explicit operator UInt128(IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator UInt128(sbyte value)
	{
		throw null;
	}

	public static explicit operator UInt128(float value)
	{
		throw null;
	}

	public static explicit operator byte(UInt128 value)
	{
		throw null;
	}

	public static explicit operator char(UInt128 value)
	{
		throw null;
	}

	public static explicit operator decimal(UInt128 value)
	{
		throw null;
	}

	public static explicit operator double(UInt128 value)
	{
		throw null;
	}

	public static explicit operator Half(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Int128(UInt128 value)
	{
		throw null;
	}

	public static explicit operator short(UInt128 value)
	{
		throw null;
	}

	public static explicit operator int(UInt128 value)
	{
		throw null;
	}

	public static explicit operator long(UInt128 value)
	{
		throw null;
	}

	public static explicit operator IntPtr(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator sbyte(UInt128 value)
	{
		throw null;
	}

	public static explicit operator float(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ushort(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator uint(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ulong(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator UIntPtr(UInt128 value)
	{
		throw null;
	}

	public static bool operator >(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static bool operator >=(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static implicit operator UInt128(byte value)
	{
		throw null;
	}

	public static implicit operator UInt128(char value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator UInt128(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator UInt128(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator UInt128(ulong value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator UInt128(UIntPtr value)
	{
		throw null;
	}

	public static UInt128 operator ++(UInt128 value)
	{
		throw null;
	}

	public static bool operator !=(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator <<(UInt128 value, int shiftAmount)
	{
		throw null;
	}

	public static bool operator <(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static bool operator <=(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator %(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator *(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator ~(UInt128 value)
	{
		throw null;
	}

	public static UInt128 operator >>(UInt128 value, int shiftAmount)
	{
		throw null;
	}

	public static UInt128 operator -(UInt128 left, UInt128 right)
	{
		throw null;
	}

	public static UInt128 operator -(UInt128 value)
	{
		throw null;
	}

	public static UInt128 operator +(UInt128 value)
	{
		throw null;
	}

	public static UInt128 operator >>>(UInt128 value, int shiftAmount)
	{
		throw null;
	}

	public static UInt128 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static UInt128 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static UInt128 Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static UInt128 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static UInt128 Parse(string s)
	{
		throw null;
	}

	public static UInt128 Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static UInt128 Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static UInt128 Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static UInt128 PopCount(UInt128 value)
	{
		throw null;
	}

	public static UInt128 RotateLeft(UInt128 value, int rotateAmount)
	{
		throw null;
	}

	public static UInt128 RotateRight(UInt128 value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(UInt128 value)
	{
		throw null;
	}

	int IBinaryInteger<UInt128>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<UInt128>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<UInt128>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt128 value)
	{
		throw null;
	}

	static bool IBinaryInteger<UInt128>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt128 value)
	{
		throw null;
	}

	bool IBinaryInteger<UInt128>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<UInt128>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static UInt128 INumberBase<UInt128>.Abs(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsCanonical(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsComplexNumber(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsFinite(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsImaginaryNumber(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsInfinity(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsInteger(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsNaN(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsNegative(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsNegativeInfinity(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsNormal(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsPositive(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsPositiveInfinity(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsRealNumber(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsSubnormal(UInt128 value)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.IsZero(UInt128 value)
	{
		throw null;
	}

	static UInt128 INumberBase<UInt128>.MaxMagnitude(UInt128 x, UInt128 y)
	{
		throw null;
	}

	static UInt128 INumberBase<UInt128>.MaxMagnitudeNumber(UInt128 x, UInt128 y)
	{
		throw null;
	}

	static UInt128 INumberBase<UInt128>.MinMagnitude(UInt128 x, UInt128 y)
	{
		throw null;
	}

	static UInt128 INumberBase<UInt128>.MinMagnitudeNumber(UInt128 x, UInt128 y)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.TryConvertFromChecked<TOther>(TOther value, out UInt128 result)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.TryConvertFromSaturating<TOther>(TOther value, out UInt128 result)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.TryConvertFromTruncating<TOther>(TOther value, out UInt128 result)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.TryConvertToChecked<TOther>(UInt128 value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.TryConvertToSaturating<TOther>(UInt128 value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<UInt128>.TryConvertToTruncating<TOther>(UInt128 value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static UInt128 INumber<UInt128>.CopySign(UInt128 value, UInt128 sign)
	{
		throw null;
	}

	static UInt128 INumber<UInt128>.MaxNumber(UInt128 x, UInt128 y)
	{
		throw null;
	}

	static UInt128 INumber<UInt128>.MinNumber(UInt128 x, UInt128 y)
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

	public static UInt128 TrailingZeroCount(UInt128 value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out UInt128 result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out UInt128 result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out UInt128 result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out UInt128 result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out UInt128 result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out UInt128 result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out UInt128 result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out UInt128 result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out UInt128 result)
	{
		throw null;
	}
}
