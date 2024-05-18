using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

public readonly struct Char : IComparable, IComparable<char>, IConvertible, IEquatable<char>, IFormattable, IParsable<char>, ISpanFormattable, ISpanParsable<char>, IAdditionOperators<char, char, char>, IAdditiveIdentity<char, char>, IBinaryInteger<char>, IBinaryNumber<char>, IBitwiseOperators<char, char, char>, IComparisonOperators<char, char, bool>, IEqualityOperators<char, char, bool>, IDecrementOperators<char>, IDivisionOperators<char, char, char>, IIncrementOperators<char>, IModulusOperators<char, char, char>, IMultiplicativeIdentity<char, char>, IMultiplyOperators<char, char, char>, INumber<char>, INumberBase<char>, ISubtractionOperators<char, char, char>, IUnaryNegationOperators<char, char>, IUnaryPlusOperators<char, char>, IUtf8SpanFormattable, IUtf8SpanParsable<char>, IShiftOperators<char, int, char>, IMinMaxValue<char>, IUnsignedNumber<char>
{
	private readonly char _dummyPrimitive;

	public const char MaxValue = '\uffff';

	public const char MinValue = '\0';

	static char IAdditiveIdentity<char, char>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static char IBinaryNumber<char>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static char IMinMaxValue<char>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static char IMinMaxValue<char>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static char IMultiplicativeIdentity<char, char>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static char INumberBase<char>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<char>.Radix
	{
		get
		{
			throw null;
		}
	}

	static char INumberBase<char>.Zero
	{
		get
		{
			throw null;
		}
	}

	public int CompareTo(char value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static string ConvertFromUtf32(int utf32)
	{
		throw null;
	}

	public static int ConvertToUtf32(char highSurrogate, char lowSurrogate)
	{
		throw null;
	}

	public static int ConvertToUtf32(string s, int index)
	{
		throw null;
	}

	public bool Equals(char obj)
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

	public static double GetNumericValue(char c)
	{
		throw null;
	}

	public static double GetNumericValue(string s, int index)
	{
		throw null;
	}

	public TypeCode GetTypeCode()
	{
		throw null;
	}

	public static UnicodeCategory GetUnicodeCategory(char c)
	{
		throw null;
	}

	public static UnicodeCategory GetUnicodeCategory(string s, int index)
	{
		throw null;
	}

	public static bool IsAscii(char c)
	{
		throw null;
	}

	public static bool IsAsciiDigit(char c)
	{
		throw null;
	}

	public static bool IsAsciiHexDigit(char c)
	{
		throw null;
	}

	public static bool IsAsciiHexDigitLower(char c)
	{
		throw null;
	}

	public static bool IsAsciiHexDigitUpper(char c)
	{
		throw null;
	}

	public static bool IsAsciiLetter(char c)
	{
		throw null;
	}

	public static bool IsAsciiLetterLower(char c)
	{
		throw null;
	}

	public static bool IsAsciiLetterOrDigit(char c)
	{
		throw null;
	}

	public static bool IsAsciiLetterUpper(char c)
	{
		throw null;
	}

	public static bool IsBetween(char c, char minInclusive, char maxInclusive)
	{
		throw null;
	}

	public static bool IsControl(char c)
	{
		throw null;
	}

	public static bool IsControl(string s, int index)
	{
		throw null;
	}

	public static bool IsDigit(char c)
	{
		throw null;
	}

	public static bool IsDigit(string s, int index)
	{
		throw null;
	}

	public static bool IsHighSurrogate(char c)
	{
		throw null;
	}

	public static bool IsHighSurrogate(string s, int index)
	{
		throw null;
	}

	public static bool IsLetter(char c)
	{
		throw null;
	}

	public static bool IsLetter(string s, int index)
	{
		throw null;
	}

	public static bool IsLetterOrDigit(char c)
	{
		throw null;
	}

	public static bool IsLetterOrDigit(string s, int index)
	{
		throw null;
	}

	public static bool IsLower(char c)
	{
		throw null;
	}

	public static bool IsLower(string s, int index)
	{
		throw null;
	}

	public static bool IsLowSurrogate(char c)
	{
		throw null;
	}

	public static bool IsLowSurrogate(string s, int index)
	{
		throw null;
	}

	public static bool IsNumber(char c)
	{
		throw null;
	}

	public static bool IsNumber(string s, int index)
	{
		throw null;
	}

	public static bool IsPunctuation(char c)
	{
		throw null;
	}

	public static bool IsPunctuation(string s, int index)
	{
		throw null;
	}

	public static bool IsSeparator(char c)
	{
		throw null;
	}

	public static bool IsSeparator(string s, int index)
	{
		throw null;
	}

	public static bool IsSurrogate(char c)
	{
		throw null;
	}

	public static bool IsSurrogate(string s, int index)
	{
		throw null;
	}

	public static bool IsSurrogatePair(char highSurrogate, char lowSurrogate)
	{
		throw null;
	}

	public static bool IsSurrogatePair(string s, int index)
	{
		throw null;
	}

	public static bool IsSymbol(char c)
	{
		throw null;
	}

	public static bool IsSymbol(string s, int index)
	{
		throw null;
	}

	public static bool IsUpper(char c)
	{
		throw null;
	}

	public static bool IsUpper(string s, int index)
	{
		throw null;
	}

	public static bool IsWhiteSpace(char c)
	{
		throw null;
	}

	public static bool IsWhiteSpace(string s, int index)
	{
		throw null;
	}

	public static char Parse(string s)
	{
		throw null;
	}

	bool IConvertible.ToBoolean(IFormatProvider? provider)
	{
		throw null;
	}

	byte IConvertible.ToByte(IFormatProvider? provider)
	{
		throw null;
	}

	char IConvertible.ToChar(IFormatProvider? provider)
	{
		throw null;
	}

	DateTime IConvertible.ToDateTime(IFormatProvider? provider)
	{
		throw null;
	}

	decimal IConvertible.ToDecimal(IFormatProvider? provider)
	{
		throw null;
	}

	double IConvertible.ToDouble(IFormatProvider? provider)
	{
		throw null;
	}

	short IConvertible.ToInt16(IFormatProvider? provider)
	{
		throw null;
	}

	int IConvertible.ToInt32(IFormatProvider? provider)
	{
		throw null;
	}

	long IConvertible.ToInt64(IFormatProvider? provider)
	{
		throw null;
	}

	sbyte IConvertible.ToSByte(IFormatProvider? provider)
	{
		throw null;
	}

	float IConvertible.ToSingle(IFormatProvider? provider)
	{
		throw null;
	}

	object IConvertible.ToType(Type type, IFormatProvider? provider)
	{
		throw null;
	}

	ushort IConvertible.ToUInt16(IFormatProvider? provider)
	{
		throw null;
	}

	uint IConvertible.ToUInt32(IFormatProvider? provider)
	{
		throw null;
	}

	ulong IConvertible.ToUInt64(IFormatProvider? provider)
	{
		throw null;
	}

	string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
	{
		throw null;
	}

	static char IParsable<char>.Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	static bool IParsable<char>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out char result)
	{
		throw null;
	}

	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	bool IUtf8SpanFormattable.TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	static char ISpanParsable<char>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	static bool ISpanParsable<char>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out char result)
	{
		throw null;
	}

	static char IAdditionOperators<char, char, char>.operator +(char left, char right)
	{
		throw null;
	}

	static char IAdditionOperators<char, char, char>.operator checked +(char left, char right)
	{
		throw null;
	}

	int IBinaryInteger<char>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<char>.GetShortestBitLength()
	{
		throw null;
	}

	static char IBinaryInteger<char>.LeadingZeroCount(char value)
	{
		throw null;
	}

	static char IBinaryInteger<char>.PopCount(char value)
	{
		throw null;
	}

	static char IBinaryInteger<char>.RotateLeft(char value, int rotateAmount)
	{
		throw null;
	}

	static char IBinaryInteger<char>.RotateRight(char value, int rotateAmount)
	{
		throw null;
	}

	static char IBinaryInteger<char>.TrailingZeroCount(char value)
	{
		throw null;
	}

	static bool IBinaryInteger<char>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out char value)
	{
		throw null;
	}

	static bool IBinaryInteger<char>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out char value)
	{
		throw null;
	}

	bool IBinaryInteger<char>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<char>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static bool IBinaryNumber<char>.IsPow2(char value)
	{
		throw null;
	}

	static char IBinaryNumber<char>.Log2(char value)
	{
		throw null;
	}

	static char IBitwiseOperators<char, char, char>.operator &(char left, char right)
	{
		throw null;
	}

	static char IBitwiseOperators<char, char, char>.operator |(char left, char right)
	{
		throw null;
	}

	static char IBitwiseOperators<char, char, char>.operator ^(char left, char right)
	{
		throw null;
	}

	static char IBitwiseOperators<char, char, char>.operator ~(char value)
	{
		throw null;
	}

	static bool IComparisonOperators<char, char, bool>.operator >(char left, char right)
	{
		throw null;
	}

	static bool IComparisonOperators<char, char, bool>.operator >=(char left, char right)
	{
		throw null;
	}

	static bool IComparisonOperators<char, char, bool>.operator <(char left, char right)
	{
		throw null;
	}

	static bool IComparisonOperators<char, char, bool>.operator <=(char left, char right)
	{
		throw null;
	}

	static char IDecrementOperators<char>.operator checked --(char value)
	{
		throw null;
	}

	static char IDecrementOperators<char>.operator --(char value)
	{
		throw null;
	}

	static char IDivisionOperators<char, char, char>.operator /(char left, char right)
	{
		throw null;
	}

	static bool IEqualityOperators<char, char, bool>.operator ==(char left, char right)
	{
		throw null;
	}

	static bool IEqualityOperators<char, char, bool>.operator !=(char left, char right)
	{
		throw null;
	}

	static char IIncrementOperators<char>.operator checked ++(char value)
	{
		throw null;
	}

	static char IIncrementOperators<char>.operator ++(char value)
	{
		throw null;
	}

	static char IModulusOperators<char, char, char>.operator %(char left, char right)
	{
		throw null;
	}

	static char IMultiplyOperators<char, char, char>.operator checked *(char left, char right)
	{
		throw null;
	}

	static char IMultiplyOperators<char, char, char>.operator *(char left, char right)
	{
		throw null;
	}

	static char INumberBase<char>.Abs(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsCanonical(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsComplexNumber(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsEvenInteger(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsFinite(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsImaginaryNumber(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsInfinity(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsInteger(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsNaN(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsNegative(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsNegativeInfinity(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsNormal(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsOddInteger(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsPositive(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsPositiveInfinity(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsRealNumber(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsSubnormal(char value)
	{
		throw null;
	}

	static bool INumberBase<char>.IsZero(char value)
	{
		throw null;
	}

	static char INumberBase<char>.MaxMagnitude(char x, char y)
	{
		throw null;
	}

	static char INumberBase<char>.MaxMagnitudeNumber(char x, char y)
	{
		throw null;
	}

	static char INumberBase<char>.MinMagnitude(char x, char y)
	{
		throw null;
	}

	static char INumberBase<char>.MinMagnitudeNumber(char x, char y)
	{
		throw null;
	}

	static char INumberBase<char>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	static char INumberBase<char>.Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	static bool INumberBase<char>.TryConvertFromChecked<TOther>(TOther value, out char result)
	{
		throw null;
	}

	static bool INumberBase<char>.TryConvertFromSaturating<TOther>(TOther value, out char result)
	{
		throw null;
	}

	static bool INumberBase<char>.TryConvertFromTruncating<TOther>(TOther value, out char result)
	{
		throw null;
	}

	static bool INumberBase<char>.TryConvertToChecked<TOther>(char value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<char>.TryConvertToSaturating<TOther>(char value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<char>.TryConvertToTruncating<TOther>(char value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<char>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out char result)
	{
		throw null;
	}

	static bool INumberBase<char>.TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out char result)
	{
		throw null;
	}

	static char IShiftOperators<char, int, char>.operator <<(char value, int shiftAmount)
	{
		throw null;
	}

	static char IShiftOperators<char, int, char>.operator >>(char value, int shiftAmount)
	{
		throw null;
	}

	static char IShiftOperators<char, int, char>.operator >>>(char value, int shiftAmount)
	{
		throw null;
	}

	static char ISubtractionOperators<char, char, char>.operator checked -(char left, char right)
	{
		throw null;
	}

	static char ISubtractionOperators<char, char, char>.operator -(char left, char right)
	{
		throw null;
	}

	static char IUnaryNegationOperators<char, char>.operator checked -(char value)
	{
		throw null;
	}

	static char IUnaryNegationOperators<char, char>.operator -(char value)
	{
		throw null;
	}

	static char IUnaryPlusOperators<char, char>.operator +(char value)
	{
		throw null;
	}

	public static char ToLower(char c)
	{
		throw null;
	}

	public static char ToLower(char c, CultureInfo culture)
	{
		throw null;
	}

	public static char ToLowerInvariant(char c)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static string ToString(char c)
	{
		throw null;
	}

	public string ToString(IFormatProvider? provider)
	{
		throw null;
	}

	public static char ToUpper(char c)
	{
		throw null;
	}

	public static char ToUpper(char c, CultureInfo culture)
	{
		throw null;
	}

	public static char ToUpperInvariant(char c)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out char result)
	{
		throw null;
	}
}
