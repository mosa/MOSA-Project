using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

[CLSCompliant(false)]
public readonly struct SByte : IComparable, IComparable<sbyte>, IConvertible, IEquatable<sbyte>, IFormattable, IParsable<sbyte>, ISpanFormattable, ISpanParsable<sbyte>, IAdditionOperators<sbyte, sbyte, sbyte>, IAdditiveIdentity<sbyte, sbyte>, IBinaryInteger<sbyte>, IBinaryNumber<sbyte>, IBitwiseOperators<sbyte, sbyte, sbyte>, IComparisonOperators<sbyte, sbyte, bool>, IEqualityOperators<sbyte, sbyte, bool>, IDecrementOperators<sbyte>, IDivisionOperators<sbyte, sbyte, sbyte>, IIncrementOperators<sbyte>, IModulusOperators<sbyte, sbyte, sbyte>, IMultiplicativeIdentity<sbyte, sbyte>, IMultiplyOperators<sbyte, sbyte, sbyte>, INumber<sbyte>, INumberBase<sbyte>, ISubtractionOperators<sbyte, sbyte, sbyte>, IUnaryNegationOperators<sbyte, sbyte>, IUnaryPlusOperators<sbyte, sbyte>, IUtf8SpanFormattable, IUtf8SpanParsable<sbyte>, IShiftOperators<sbyte, int, sbyte>, IMinMaxValue<sbyte>, ISignedNumber<sbyte>
{
	private readonly sbyte _dummyPrimitive;

	public const sbyte MaxValue = 127;

	public const sbyte MinValue = -128;

	static sbyte IAdditiveIdentity<sbyte, sbyte>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static sbyte IBinaryNumber<sbyte>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static sbyte IMinMaxValue<sbyte>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static sbyte IMinMaxValue<sbyte>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static sbyte IMultiplicativeIdentity<sbyte, sbyte>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static sbyte INumberBase<sbyte>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<sbyte>.Radix
	{
		get
		{
			throw null;
		}
	}

	static sbyte INumberBase<sbyte>.Zero
	{
		get
		{
			throw null;
		}
	}

	static sbyte ISignedNumber<sbyte>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public static sbyte Abs(sbyte value)
	{
		throw null;
	}

	public static sbyte Clamp(sbyte value, sbyte min, sbyte max)
	{
		throw null;
	}

	public int CompareTo(object? obj)
	{
		throw null;
	}

	public int CompareTo(sbyte value)
	{
		throw null;
	}

	public static sbyte CopySign(sbyte value, sbyte sign)
	{
		throw null;
	}

	public static sbyte CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static sbyte CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static sbyte CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (sbyte Quotient, sbyte Remainder) DivRem(sbyte left, sbyte right)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(sbyte obj)
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

	public static bool IsEvenInteger(sbyte value)
	{
		throw null;
	}

	public static bool IsNegative(sbyte value)
	{
		throw null;
	}

	public static bool IsOddInteger(sbyte value)
	{
		throw null;
	}

	public static bool IsPositive(sbyte value)
	{
		throw null;
	}

	public static bool IsPow2(sbyte value)
	{
		throw null;
	}

	public static sbyte LeadingZeroCount(sbyte value)
	{
		throw null;
	}

	public static sbyte Log2(sbyte value)
	{
		throw null;
	}

	public static sbyte Max(sbyte x, sbyte y)
	{
		throw null;
	}

	public static sbyte MaxMagnitude(sbyte x, sbyte y)
	{
		throw null;
	}

	public static sbyte Min(sbyte x, sbyte y)
	{
		throw null;
	}

	public static sbyte MinMagnitude(sbyte x, sbyte y)
	{
		throw null;
	}

	public static sbyte Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static sbyte Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static sbyte Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static sbyte Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static sbyte Parse(string s)
	{
		throw null;
	}

	public static sbyte Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static sbyte Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static sbyte Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static sbyte PopCount(sbyte value)
	{
		throw null;
	}

	public static sbyte RotateLeft(sbyte value, int rotateAmount)
	{
		throw null;
	}

	public static sbyte RotateRight(sbyte value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(sbyte value)
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

	static sbyte IAdditionOperators<sbyte, sbyte, sbyte>.operator +(sbyte left, sbyte right)
	{
		throw null;
	}

	static sbyte IAdditionOperators<sbyte, sbyte, sbyte>.operator checked +(sbyte left, sbyte right)
	{
		throw null;
	}

	int IBinaryInteger<sbyte>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<sbyte>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<sbyte>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out sbyte value)
	{
		throw null;
	}

	static bool IBinaryInteger<sbyte>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out sbyte value)
	{
		throw null;
	}

	bool IBinaryInteger<sbyte>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<sbyte>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static sbyte IBitwiseOperators<sbyte, sbyte, sbyte>.operator &(sbyte left, sbyte right)
	{
		throw null;
	}

	static sbyte IBitwiseOperators<sbyte, sbyte, sbyte>.operator |(sbyte left, sbyte right)
	{
		throw null;
	}

	static sbyte IBitwiseOperators<sbyte, sbyte, sbyte>.operator ^(sbyte left, sbyte right)
	{
		throw null;
	}

	static sbyte IBitwiseOperators<sbyte, sbyte, sbyte>.operator ~(sbyte value)
	{
		throw null;
	}

	static bool IComparisonOperators<sbyte, sbyte, bool>.operator >(sbyte left, sbyte right)
	{
		throw null;
	}

	static bool IComparisonOperators<sbyte, sbyte, bool>.operator >=(sbyte left, sbyte right)
	{
		throw null;
	}

	static bool IComparisonOperators<sbyte, sbyte, bool>.operator <(sbyte left, sbyte right)
	{
		throw null;
	}

	static bool IComparisonOperators<sbyte, sbyte, bool>.operator <=(sbyte left, sbyte right)
	{
		throw null;
	}

	static sbyte IDecrementOperators<sbyte>.operator checked --(sbyte value)
	{
		throw null;
	}

	static sbyte IDecrementOperators<sbyte>.operator --(sbyte value)
	{
		throw null;
	}

	static sbyte IDivisionOperators<sbyte, sbyte, sbyte>.operator /(sbyte left, sbyte right)
	{
		throw null;
	}

	static bool IEqualityOperators<sbyte, sbyte, bool>.operator ==(sbyte left, sbyte right)
	{
		throw null;
	}

	static bool IEqualityOperators<sbyte, sbyte, bool>.operator !=(sbyte left, sbyte right)
	{
		throw null;
	}

	static sbyte IIncrementOperators<sbyte>.operator checked ++(sbyte value)
	{
		throw null;
	}

	static sbyte IIncrementOperators<sbyte>.operator ++(sbyte value)
	{
		throw null;
	}

	static sbyte IModulusOperators<sbyte, sbyte, sbyte>.operator %(sbyte left, sbyte right)
	{
		throw null;
	}

	static sbyte IMultiplyOperators<sbyte, sbyte, sbyte>.operator checked *(sbyte left, sbyte right)
	{
		throw null;
	}

	static sbyte IMultiplyOperators<sbyte, sbyte, sbyte>.operator *(sbyte left, sbyte right)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsCanonical(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsComplexNumber(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsFinite(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsImaginaryNumber(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsInfinity(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsInteger(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsNaN(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsNegativeInfinity(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsNormal(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsPositiveInfinity(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsRealNumber(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsSubnormal(sbyte value)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.IsZero(sbyte value)
	{
		throw null;
	}

	static sbyte INumberBase<sbyte>.MaxMagnitudeNumber(sbyte x, sbyte y)
	{
		throw null;
	}

	static sbyte INumberBase<sbyte>.MinMagnitudeNumber(sbyte x, sbyte y)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.TryConvertFromChecked<TOther>(TOther value, out sbyte result)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.TryConvertFromSaturating<TOther>(TOther value, out sbyte result)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.TryConvertFromTruncating<TOther>(TOther value, out sbyte result)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.TryConvertToChecked<TOther>(sbyte value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.TryConvertToSaturating<TOther>(sbyte value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<sbyte>.TryConvertToTruncating<TOther>(sbyte value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static sbyte INumber<sbyte>.MaxNumber(sbyte x, sbyte y)
	{
		throw null;
	}

	static sbyte INumber<sbyte>.MinNumber(sbyte x, sbyte y)
	{
		throw null;
	}

	static sbyte IShiftOperators<sbyte, int, sbyte>.operator <<(sbyte value, int shiftAmount)
	{
		throw null;
	}

	static sbyte IShiftOperators<sbyte, int, sbyte>.operator >>(sbyte value, int shiftAmount)
	{
		throw null;
	}

	static sbyte IShiftOperators<sbyte, int, sbyte>.operator >>>(sbyte value, int shiftAmount)
	{
		throw null;
	}

	static sbyte ISubtractionOperators<sbyte, sbyte, sbyte>.operator checked -(sbyte left, sbyte right)
	{
		throw null;
	}

	static sbyte ISubtractionOperators<sbyte, sbyte, sbyte>.operator -(sbyte left, sbyte right)
	{
		throw null;
	}

	static sbyte IUnaryNegationOperators<sbyte, sbyte>.operator checked -(sbyte value)
	{
		throw null;
	}

	static sbyte IUnaryNegationOperators<sbyte, sbyte>.operator -(sbyte value)
	{
		throw null;
	}

	static sbyte IUnaryPlusOperators<sbyte, sbyte>.operator +(sbyte value)
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

	public static sbyte TrailingZeroCount(sbyte value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out sbyte result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out sbyte result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out sbyte result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out sbyte result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out sbyte result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out sbyte result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out sbyte result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out sbyte result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out sbyte result)
	{
		throw null;
	}
}
