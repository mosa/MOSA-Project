using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.Serialization;

namespace System;

public readonly struct Decimal : IComparable, IComparable<decimal>, IConvertible, IEquatable<decimal>, IFormattable, IParsable<decimal>, ISpanFormattable, ISpanParsable<decimal>, IAdditionOperators<decimal, decimal, decimal>, IAdditiveIdentity<decimal, decimal>, IComparisonOperators<decimal, decimal, bool>, IEqualityOperators<decimal, decimal, bool>, IDecrementOperators<decimal>, IDivisionOperators<decimal, decimal, decimal>, IFloatingPoint<decimal>, IFloatingPointConstants<decimal>, INumberBase<decimal>, IIncrementOperators<decimal>, IMultiplicativeIdentity<decimal, decimal>, IMultiplyOperators<decimal, decimal, decimal>, ISubtractionOperators<decimal, decimal, decimal>, IUnaryNegationOperators<decimal, decimal>, IUnaryPlusOperators<decimal, decimal>, IUtf8SpanFormattable, IUtf8SpanParsable<decimal>, IModulusOperators<decimal, decimal, decimal>, INumber<decimal>, ISignedNumber<decimal>, IMinMaxValue<decimal>, IDeserializationCallback, ISerializable
{
	private readonly int _dummyPrimitive;

	public const decimal MaxValue = 79228162514264337593543950335m;

	public const decimal MinusOne = -1m;

	public const decimal MinValue = -79228162514264337593543950335m;

	public const decimal One = 1m;

	public const decimal Zero = 0m;

	public byte Scale
	{
		get
		{
			throw null;
		}
	}

	static decimal IAdditiveIdentity<decimal, decimal>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static decimal IFloatingPointConstants<decimal>.E
	{
		get
		{
			throw null;
		}
	}

	static decimal IFloatingPointConstants<decimal>.Pi
	{
		get
		{
			throw null;
		}
	}

	static decimal IFloatingPointConstants<decimal>.Tau
	{
		get
		{
			throw null;
		}
	}

	static decimal IMinMaxValue<decimal>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static decimal IMinMaxValue<decimal>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static decimal IMultiplicativeIdentity<decimal, decimal>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static decimal INumberBase<decimal>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<decimal>.Radix
	{
		get
		{
			throw null;
		}
	}

	static decimal INumberBase<decimal>.Zero
	{
		get
		{
			throw null;
		}
	}

	static decimal ISignedNumber<decimal>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public Decimal(double value)
	{
		throw null;
	}

	public Decimal(int value)
	{
		throw null;
	}

	public Decimal(int lo, int mid, int hi, bool isNegative, byte scale)
	{
		throw null;
	}

	public Decimal(int[] bits)
	{
		throw null;
	}

	public Decimal(long value)
	{
		throw null;
	}

	public Decimal(ReadOnlySpan<int> bits)
	{
		throw null;
	}

	public Decimal(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public Decimal(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public Decimal(ulong value)
	{
		throw null;
	}

	public static decimal Abs(decimal value)
	{
		throw null;
	}

	public static decimal Add(decimal d1, decimal d2)
	{
		throw null;
	}

	public static decimal Ceiling(decimal d)
	{
		throw null;
	}

	public static decimal Clamp(decimal value, decimal min, decimal max)
	{
		throw null;
	}

	public static int Compare(decimal d1, decimal d2)
	{
		throw null;
	}

	public int CompareTo(decimal value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static decimal CopySign(decimal value, decimal sign)
	{
		throw null;
	}

	public static decimal CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static decimal CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static decimal CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static decimal Divide(decimal d1, decimal d2)
	{
		throw null;
	}

	public bool Equals(decimal value)
	{
		throw null;
	}

	public static bool Equals(decimal d1, decimal d2)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public static decimal Floor(decimal d)
	{
		throw null;
	}

	public static decimal FromOACurrency(long cy)
	{
		throw null;
	}

	public static int[] GetBits(decimal d)
	{
		throw null;
	}

	public static int GetBits(decimal d, Span<int> destination)
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

	public static bool IsCanonical(decimal value)
	{
		throw null;
	}

	public static bool IsEvenInteger(decimal value)
	{
		throw null;
	}

	public static bool IsInteger(decimal value)
	{
		throw null;
	}

	public static bool IsNegative(decimal value)
	{
		throw null;
	}

	public static bool IsOddInteger(decimal value)
	{
		throw null;
	}

	public static bool IsPositive(decimal value)
	{
		throw null;
	}

	public static decimal Max(decimal x, decimal y)
	{
		throw null;
	}

	public static decimal MaxMagnitude(decimal x, decimal y)
	{
		throw null;
	}

	public static decimal Min(decimal x, decimal y)
	{
		throw null;
	}

	public static decimal MinMagnitude(decimal x, decimal y)
	{
		throw null;
	}

	public static decimal Multiply(decimal d1, decimal d2)
	{
		throw null;
	}

	public static decimal Negate(decimal d)
	{
		throw null;
	}

	public static decimal operator +(decimal d1, decimal d2)
	{
		throw null;
	}

	public static decimal operator --(decimal d)
	{
		throw null;
	}

	public static decimal operator /(decimal d1, decimal d2)
	{
		throw null;
	}

	public static bool operator ==(decimal d1, decimal d2)
	{
		throw null;
	}

	public static explicit operator byte(decimal value)
	{
		throw null;
	}

	public static explicit operator char(decimal value)
	{
		throw null;
	}

	public static explicit operator double(decimal value)
	{
		throw null;
	}

	public static explicit operator short(decimal value)
	{
		throw null;
	}

	public static explicit operator int(decimal value)
	{
		throw null;
	}

	public static explicit operator long(decimal value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator sbyte(decimal value)
	{
		throw null;
	}

	public static explicit operator float(decimal value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ushort(decimal value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator uint(decimal value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ulong(decimal value)
	{
		throw null;
	}

	public static explicit operator decimal(double value)
	{
		throw null;
	}

	public static explicit operator decimal(float value)
	{
		throw null;
	}

	public static bool operator >(decimal d1, decimal d2)
	{
		throw null;
	}

	public static bool operator >=(decimal d1, decimal d2)
	{
		throw null;
	}

	public static implicit operator decimal(byte value)
	{
		throw null;
	}

	public static implicit operator decimal(char value)
	{
		throw null;
	}

	public static implicit operator decimal(short value)
	{
		throw null;
	}

	public static implicit operator decimal(int value)
	{
		throw null;
	}

	public static implicit operator decimal(long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator decimal(sbyte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator decimal(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator decimal(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator decimal(ulong value)
	{
		throw null;
	}

	public static decimal operator ++(decimal d)
	{
		throw null;
	}

	public static bool operator !=(decimal d1, decimal d2)
	{
		throw null;
	}

	public static bool operator <(decimal d1, decimal d2)
	{
		throw null;
	}

	public static bool operator <=(decimal d1, decimal d2)
	{
		throw null;
	}

	public static decimal operator %(decimal d1, decimal d2)
	{
		throw null;
	}

	public static decimal operator *(decimal d1, decimal d2)
	{
		throw null;
	}

	public static decimal operator -(decimal d1, decimal d2)
	{
		throw null;
	}

	public static decimal operator -(decimal d)
	{
		throw null;
	}

	public static decimal operator +(decimal d)
	{
		throw null;
	}

	public static decimal Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static decimal Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static decimal Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static decimal Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static decimal Parse(string s)
	{
		throw null;
	}

	public static decimal Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static decimal Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static decimal Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static decimal Remainder(decimal d1, decimal d2)
	{
		throw null;
	}

	public static decimal Round(decimal d)
	{
		throw null;
	}

	public static decimal Round(decimal d, int decimals)
	{
		throw null;
	}

	public static decimal Round(decimal d, int decimals, MidpointRounding mode)
	{
		throw null;
	}

	public static decimal Round(decimal d, MidpointRounding mode)
	{
		throw null;
	}

	public static int Sign(decimal d)
	{
		throw null;
	}

	public static decimal Subtract(decimal d1, decimal d2)
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

	int IFloatingPoint<decimal>.GetExponentByteCount()
	{
		throw null;
	}

	int IFloatingPoint<decimal>.GetExponentShortestBitLength()
	{
		throw null;
	}

	int IFloatingPoint<decimal>.GetSignificandBitLength()
	{
		throw null;
	}

	int IFloatingPoint<decimal>.GetSignificandByteCount()
	{
		throw null;
	}

	bool IFloatingPoint<decimal>.TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<decimal>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<decimal>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<decimal>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsComplexNumber(decimal value)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsFinite(decimal value)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsImaginaryNumber(decimal value)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsInfinity(decimal value)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsNaN(decimal value)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsNegativeInfinity(decimal value)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsNormal(decimal value)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsPositiveInfinity(decimal value)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsRealNumber(decimal value)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsSubnormal(decimal value)
	{
		throw null;
	}

	static bool INumberBase<decimal>.IsZero(decimal value)
	{
		throw null;
	}

	static decimal INumberBase<decimal>.MaxMagnitudeNumber(decimal x, decimal y)
	{
		throw null;
	}

	static decimal INumberBase<decimal>.MinMagnitudeNumber(decimal x, decimal y)
	{
		throw null;
	}

	static bool INumberBase<decimal>.TryConvertFromChecked<TOther>(TOther value, out decimal result)
	{
		throw null;
	}

	static bool INumberBase<decimal>.TryConvertFromSaturating<TOther>(TOther value, out decimal result)
	{
		throw null;
	}

	static bool INumberBase<decimal>.TryConvertFromTruncating<TOther>(TOther value, out decimal result)
	{
		throw null;
	}

	static bool INumberBase<decimal>.TryConvertToChecked<TOther>(decimal value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<decimal>.TryConvertToSaturating<TOther>(decimal value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<decimal>.TryConvertToTruncating<TOther>(decimal value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static decimal INumber<decimal>.MaxNumber(decimal x, decimal y)
	{
		throw null;
	}

	static decimal INumber<decimal>.MinNumber(decimal x, decimal y)
	{
		throw null;
	}

	void IDeserializationCallback.OnDeserialization(object? sender)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static byte ToByte(decimal value)
	{
		throw null;
	}

	public static double ToDouble(decimal d)
	{
		throw null;
	}

	public static short ToInt16(decimal value)
	{
		throw null;
	}

	public static int ToInt32(decimal d)
	{
		throw null;
	}

	public static long ToInt64(decimal d)
	{
		throw null;
	}

	public static long ToOACurrency(decimal value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(decimal value)
	{
		throw null;
	}

	public static float ToSingle(decimal d)
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

	[CLSCompliant(false)]
	public static ushort ToUInt16(decimal value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(decimal d)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(decimal d)
	{
		throw null;
	}

	public static decimal Truncate(decimal d)
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

	public static bool TryGetBits(decimal d, Span<int> destination, out int valuesWritten)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out decimal result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out decimal result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out decimal result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out decimal result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out decimal result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out decimal result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out decimal result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out decimal result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out decimal result)
	{
		throw null;
	}
}
