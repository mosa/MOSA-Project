using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

public readonly struct Int16 : IComparable, IComparable<short>, IConvertible, IEquatable<short>, IFormattable, IParsable<short>, ISpanFormattable, ISpanParsable<short>, IAdditionOperators<short, short, short>, IAdditiveIdentity<short, short>, IBinaryInteger<short>, IBinaryNumber<short>, IBitwiseOperators<short, short, short>, IComparisonOperators<short, short, bool>, IEqualityOperators<short, short, bool>, IDecrementOperators<short>, IDivisionOperators<short, short, short>, IIncrementOperators<short>, IModulusOperators<short, short, short>, IMultiplicativeIdentity<short, short>, IMultiplyOperators<short, short, short>, INumber<short>, INumberBase<short>, ISubtractionOperators<short, short, short>, IUnaryNegationOperators<short, short>, IUnaryPlusOperators<short, short>, IUtf8SpanFormattable, IUtf8SpanParsable<short>, IShiftOperators<short, int, short>, IMinMaxValue<short>, ISignedNumber<short>
{
	private readonly short _dummyPrimitive;

	public const short MaxValue = 32767;

	public const short MinValue = -32768;

	static short IAdditiveIdentity<short, short>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static short IBinaryNumber<short>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static short IMinMaxValue<short>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static short IMinMaxValue<short>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static short IMultiplicativeIdentity<short, short>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static short INumberBase<short>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<short>.Radix
	{
		get
		{
			throw null;
		}
	}

	static short INumberBase<short>.Zero
	{
		get
		{
			throw null;
		}
	}

	static short ISignedNumber<short>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public static short Abs(short value)
	{
		throw null;
	}

	public static short Clamp(short value, short min, short max)
	{
		throw null;
	}

	public int CompareTo(short value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static short CopySign(short value, short sign)
	{
		throw null;
	}

	public static short CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static short CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static short CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (short Quotient, short Remainder) DivRem(short left, short right)
	{
		throw null;
	}

	public bool Equals(short obj)
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

	public TypeCode GetTypeCode()
	{
		throw null;
	}

	public static bool IsEvenInteger(short value)
	{
		throw null;
	}

	public static bool IsNegative(short value)
	{
		throw null;
	}

	public static bool IsOddInteger(short value)
	{
		throw null;
	}

	public static bool IsPositive(short value)
	{
		throw null;
	}

	public static bool IsPow2(short value)
	{
		throw null;
	}

	public static short LeadingZeroCount(short value)
	{
		throw null;
	}

	public static short Log2(short value)
	{
		throw null;
	}

	public static short Max(short x, short y)
	{
		throw null;
	}

	public static short MaxMagnitude(short x, short y)
	{
		throw null;
	}

	public static short Min(short x, short y)
	{
		throw null;
	}

	public static short MinMagnitude(short x, short y)
	{
		throw null;
	}

	public static short Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static short Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static short Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static short Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static short Parse(string s)
	{
		throw null;
	}

	public static short Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static short Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static short Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static short PopCount(short value)
	{
		throw null;
	}

	public static short RotateLeft(short value, int rotateAmount)
	{
		throw null;
	}

	public static short RotateRight(short value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(short value)
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

	static short IAdditionOperators<short, short, short>.operator +(short left, short right)
	{
		throw null;
	}

	static short IAdditionOperators<short, short, short>.operator checked +(short left, short right)
	{
		throw null;
	}

	int IBinaryInteger<short>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<short>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<short>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out short value)
	{
		throw null;
	}

	static bool IBinaryInteger<short>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out short value)
	{
		throw null;
	}

	bool IBinaryInteger<short>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<short>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static short IBitwiseOperators<short, short, short>.operator &(short left, short right)
	{
		throw null;
	}

	static short IBitwiseOperators<short, short, short>.operator |(short left, short right)
	{
		throw null;
	}

	static short IBitwiseOperators<short, short, short>.operator ^(short left, short right)
	{
		throw null;
	}

	static short IBitwiseOperators<short, short, short>.operator ~(short value)
	{
		throw null;
	}

	static bool IComparisonOperators<short, short, bool>.operator >(short left, short right)
	{
		throw null;
	}

	static bool IComparisonOperators<short, short, bool>.operator >=(short left, short right)
	{
		throw null;
	}

	static bool IComparisonOperators<short, short, bool>.operator <(short left, short right)
	{
		throw null;
	}

	static bool IComparisonOperators<short, short, bool>.operator <=(short left, short right)
	{
		throw null;
	}

	static short IDecrementOperators<short>.operator checked --(short value)
	{
		throw null;
	}

	static short IDecrementOperators<short>.operator --(short value)
	{
		throw null;
	}

	static short IDivisionOperators<short, short, short>.operator /(short left, short right)
	{
		throw null;
	}

	static bool IEqualityOperators<short, short, bool>.operator ==(short left, short right)
	{
		throw null;
	}

	static bool IEqualityOperators<short, short, bool>.operator !=(short left, short right)
	{
		throw null;
	}

	static short IIncrementOperators<short>.operator checked ++(short value)
	{
		throw null;
	}

	static short IIncrementOperators<short>.operator ++(short value)
	{
		throw null;
	}

	static short IModulusOperators<short, short, short>.operator %(short left, short right)
	{
		throw null;
	}

	static short IMultiplyOperators<short, short, short>.operator checked *(short left, short right)
	{
		throw null;
	}

	static short IMultiplyOperators<short, short, short>.operator *(short left, short right)
	{
		throw null;
	}

	static bool INumberBase<short>.IsCanonical(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsComplexNumber(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsFinite(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsImaginaryNumber(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsInfinity(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsInteger(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsNaN(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsNegativeInfinity(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsNormal(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsPositiveInfinity(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsRealNumber(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsSubnormal(short value)
	{
		throw null;
	}

	static bool INumberBase<short>.IsZero(short value)
	{
		throw null;
	}

	static short INumberBase<short>.MaxMagnitudeNumber(short x, short y)
	{
		throw null;
	}

	static short INumberBase<short>.MinMagnitudeNumber(short x, short y)
	{
		throw null;
	}

	static bool INumberBase<short>.TryConvertFromChecked<TOther>(TOther value, out short result)
	{
		throw null;
	}

	static bool INumberBase<short>.TryConvertFromSaturating<TOther>(TOther value, out short result)
	{
		throw null;
	}

	static bool INumberBase<short>.TryConvertFromTruncating<TOther>(TOther value, out short result)
	{
		throw null;
	}

	static bool INumberBase<short>.TryConvertToChecked<TOther>(short value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<short>.TryConvertToSaturating<TOther>(short value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<short>.TryConvertToTruncating<TOther>(short value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static short INumber<short>.MaxNumber(short x, short y)
	{
		throw null;
	}

	static short INumber<short>.MinNumber(short x, short y)
	{
		throw null;
	}

	static short IShiftOperators<short, int, short>.operator <<(short value, int shiftAmount)
	{
		throw null;
	}

	static short IShiftOperators<short, int, short>.operator >>(short value, int shiftAmount)
	{
		throw null;
	}

	static short IShiftOperators<short, int, short>.operator >>>(short value, int shiftAmount)
	{
		throw null;
	}

	static short ISubtractionOperators<short, short, short>.operator checked -(short left, short right)
	{
		throw null;
	}

	static short ISubtractionOperators<short, short, short>.operator -(short left, short right)
	{
		throw null;
	}

	static short IUnaryNegationOperators<short, short>.operator checked -(short value)
	{
		throw null;
	}

	static short IUnaryNegationOperators<short, short>.operator -(short value)
	{
		throw null;
	}

	static short IUnaryPlusOperators<short, short>.operator +(short value)
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

	public static short TrailingZeroCount(short value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out short result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out short result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out short result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out short result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out short result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out short result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out short result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out short result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out short result)
	{
		throw null;
	}
}
