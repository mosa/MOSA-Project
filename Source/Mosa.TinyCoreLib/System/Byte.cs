using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

public readonly struct Byte : IComparable, IComparable<byte>, IConvertible, IEquatable<byte>, IFormattable, IParsable<byte>, ISpanFormattable, ISpanParsable<byte>, IAdditionOperators<byte, byte, byte>, IAdditiveIdentity<byte, byte>, IBinaryInteger<byte>, IBinaryNumber<byte>, IBitwiseOperators<byte, byte, byte>, IComparisonOperators<byte, byte, bool>, IEqualityOperators<byte, byte, bool>, IDecrementOperators<byte>, IDivisionOperators<byte, byte, byte>, IIncrementOperators<byte>, IModulusOperators<byte, byte, byte>, IMultiplicativeIdentity<byte, byte>, IMultiplyOperators<byte, byte, byte>, INumber<byte>, INumberBase<byte>, ISubtractionOperators<byte, byte, byte>, IUnaryNegationOperators<byte, byte>, IUnaryPlusOperators<byte, byte>, IUtf8SpanFormattable, IUtf8SpanParsable<byte>, IShiftOperators<byte, int, byte>, IMinMaxValue<byte>, IUnsignedNumber<byte>
{
	private readonly byte _dummyPrimitive;

	public const byte MaxValue = 255;

	public const byte MinValue = 0;

	static byte IAdditiveIdentity<byte, byte>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static byte IBinaryNumber<byte>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static byte IMinMaxValue<byte>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static byte IMinMaxValue<byte>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static byte IMultiplicativeIdentity<byte, byte>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static byte INumberBase<byte>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<byte>.Radix
	{
		get
		{
			throw null;
		}
	}

	static byte INumberBase<byte>.Zero
	{
		get
		{
			throw null;
		}
	}

	public static byte Clamp(byte value, byte min, byte max)
	{
		throw null;
	}

	public int CompareTo(byte value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static byte CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static byte CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static byte CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (byte Quotient, byte Remainder) DivRem(byte left, byte right)
	{
		throw null;
	}

	public bool Equals(byte obj)
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

	public static bool IsEvenInteger(byte value)
	{
		throw null;
	}

	public static bool IsOddInteger(byte value)
	{
		throw null;
	}

	public static bool IsPow2(byte value)
	{
		throw null;
	}

	public static byte LeadingZeroCount(byte value)
	{
		throw null;
	}

	public static byte Log2(byte value)
	{
		throw null;
	}

	public static byte Max(byte x, byte y)
	{
		throw null;
	}

	public static byte Min(byte x, byte y)
	{
		throw null;
	}

	public static byte Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static byte Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static byte Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static byte Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static byte Parse(string s)
	{
		throw null;
	}

	public static byte Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static byte Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static byte Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static byte PopCount(byte value)
	{
		throw null;
	}

	public static byte RotateLeft(byte value, int rotateAmount)
	{
		throw null;
	}

	public static byte RotateRight(byte value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(byte value)
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

	static byte IAdditionOperators<byte, byte, byte>.operator +(byte left, byte right)
	{
		throw null;
	}

	static byte IAdditionOperators<byte, byte, byte>.operator checked +(byte left, byte right)
	{
		throw null;
	}

	int IBinaryInteger<byte>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<byte>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<byte>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out byte value)
	{
		throw null;
	}

	static bool IBinaryInteger<byte>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out byte value)
	{
		throw null;
	}

	bool IBinaryInteger<byte>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<byte>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static byte IBitwiseOperators<byte, byte, byte>.operator &(byte left, byte right)
	{
		throw null;
	}

	static byte IBitwiseOperators<byte, byte, byte>.operator |(byte left, byte right)
	{
		throw null;
	}

	static byte IBitwiseOperators<byte, byte, byte>.operator ^(byte left, byte right)
	{
		throw null;
	}

	static byte IBitwiseOperators<byte, byte, byte>.operator ~(byte value)
	{
		throw null;
	}

	static bool IComparisonOperators<byte, byte, bool>.operator >(byte left, byte right)
	{
		throw null;
	}

	static bool IComparisonOperators<byte, byte, bool>.operator >=(byte left, byte right)
	{
		throw null;
	}

	static bool IComparisonOperators<byte, byte, bool>.operator <(byte left, byte right)
	{
		throw null;
	}

	static bool IComparisonOperators<byte, byte, bool>.operator <=(byte left, byte right)
	{
		throw null;
	}

	static byte IDecrementOperators<byte>.operator checked --(byte value)
	{
		throw null;
	}

	static byte IDecrementOperators<byte>.operator --(byte value)
	{
		throw null;
	}

	static byte IDivisionOperators<byte, byte, byte>.operator /(byte left, byte right)
	{
		throw null;
	}

	static bool IEqualityOperators<byte, byte, bool>.operator ==(byte left, byte right)
	{
		throw null;
	}

	static bool IEqualityOperators<byte, byte, bool>.operator !=(byte left, byte right)
	{
		throw null;
	}

	static byte IIncrementOperators<byte>.operator checked ++(byte value)
	{
		throw null;
	}

	static byte IIncrementOperators<byte>.operator ++(byte value)
	{
		throw null;
	}

	static byte IModulusOperators<byte, byte, byte>.operator %(byte left, byte right)
	{
		throw null;
	}

	static byte IMultiplyOperators<byte, byte, byte>.operator checked *(byte left, byte right)
	{
		throw null;
	}

	static byte IMultiplyOperators<byte, byte, byte>.operator *(byte left, byte right)
	{
		throw null;
	}

	static byte INumberBase<byte>.Abs(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsCanonical(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsComplexNumber(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsFinite(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsImaginaryNumber(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsInfinity(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsInteger(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsNaN(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsNegative(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsNegativeInfinity(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsNormal(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsPositive(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsPositiveInfinity(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsRealNumber(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsSubnormal(byte value)
	{
		throw null;
	}

	static bool INumberBase<byte>.IsZero(byte value)
	{
		throw null;
	}

	static byte INumberBase<byte>.MaxMagnitude(byte x, byte y)
	{
		throw null;
	}

	static byte INumberBase<byte>.MaxMagnitudeNumber(byte x, byte y)
	{
		throw null;
	}

	static byte INumberBase<byte>.MinMagnitude(byte x, byte y)
	{
		throw null;
	}

	static byte INumberBase<byte>.MinMagnitudeNumber(byte x, byte y)
	{
		throw null;
	}

	static bool INumberBase<byte>.TryConvertFromChecked<TOther>(TOther value, out byte result)
	{
		throw null;
	}

	static bool INumberBase<byte>.TryConvertFromSaturating<TOther>(TOther value, out byte result)
	{
		throw null;
	}

	static bool INumberBase<byte>.TryConvertFromTruncating<TOther>(TOther value, out byte result)
	{
		throw null;
	}

	static bool INumberBase<byte>.TryConvertToChecked<TOther>(byte value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<byte>.TryConvertToSaturating<TOther>(byte value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<byte>.TryConvertToTruncating<TOther>(byte value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static byte INumber<byte>.CopySign(byte value, byte sign)
	{
		throw null;
	}

	static byte INumber<byte>.MaxNumber(byte x, byte y)
	{
		throw null;
	}

	static byte INumber<byte>.MinNumber(byte x, byte y)
	{
		throw null;
	}

	static byte IShiftOperators<byte, int, byte>.operator <<(byte value, int shiftAmount)
	{
		throw null;
	}

	static byte IShiftOperators<byte, int, byte>.operator >>(byte value, int shiftAmount)
	{
		throw null;
	}

	static byte IShiftOperators<byte, int, byte>.operator >>>(byte value, int shiftAmount)
	{
		throw null;
	}

	static byte ISubtractionOperators<byte, byte, byte>.operator checked -(byte left, byte right)
	{
		throw null;
	}

	static byte ISubtractionOperators<byte, byte, byte>.operator -(byte left, byte right)
	{
		throw null;
	}

	static byte IUnaryNegationOperators<byte, byte>.operator checked -(byte value)
	{
		throw null;
	}

	static byte IUnaryNegationOperators<byte, byte>.operator -(byte value)
	{
		throw null;
	}

	static byte IUnaryPlusOperators<byte, byte>.operator +(byte value)
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

	public static byte TrailingZeroCount(byte value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out byte result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out byte result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out byte result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out byte result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out byte result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out byte result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out byte result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out byte result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out byte result)
	{
		throw null;
	}
}
