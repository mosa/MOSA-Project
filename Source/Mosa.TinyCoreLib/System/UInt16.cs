using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

[CLSCompliant(false)]
public readonly struct UInt16 : IComparable, IComparable<ushort>, IConvertible, IEquatable<ushort>, IFormattable, IParsable<ushort>, ISpanFormattable, ISpanParsable<ushort>, IAdditionOperators<ushort, ushort, ushort>, IAdditiveIdentity<ushort, ushort>, IBinaryInteger<ushort>, IBinaryNumber<ushort>, IBitwiseOperators<ushort, ushort, ushort>, IComparisonOperators<ushort, ushort, bool>, IEqualityOperators<ushort, ushort, bool>, IDecrementOperators<ushort>, IDivisionOperators<ushort, ushort, ushort>, IIncrementOperators<ushort>, IModulusOperators<ushort, ushort, ushort>, IMultiplicativeIdentity<ushort, ushort>, IMultiplyOperators<ushort, ushort, ushort>, INumber<ushort>, INumberBase<ushort>, ISubtractionOperators<ushort, ushort, ushort>, IUnaryNegationOperators<ushort, ushort>, IUnaryPlusOperators<ushort, ushort>, IUtf8SpanFormattable, IUtf8SpanParsable<ushort>, IShiftOperators<ushort, int, ushort>, IMinMaxValue<ushort>, IUnsignedNumber<ushort>
{
	private readonly ushort _dummyPrimitive;

	public const ushort MaxValue = 65535;

	public const ushort MinValue = 0;

	static ushort IAdditiveIdentity<ushort, ushort>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static ushort IBinaryNumber<ushort>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static ushort IMinMaxValue<ushort>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static ushort IMinMaxValue<ushort>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static ushort IMultiplicativeIdentity<ushort, ushort>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static ushort INumberBase<ushort>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<ushort>.Radix
	{
		get
		{
			throw null;
		}
	}

	static ushort INumberBase<ushort>.Zero
	{
		get
		{
			throw null;
		}
	}

	public static ushort Clamp(ushort value, ushort min, ushort max)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public int CompareTo(ushort value)
	{
		throw null;
	}

	public static ushort CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static ushort CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static ushort CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (ushort Quotient, ushort Remainder) DivRem(ushort left, ushort right)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(ushort obj)
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

	public static bool IsEvenInteger(ushort value)
	{
		throw null;
	}

	public static bool IsOddInteger(ushort value)
	{
		throw null;
	}

	public static bool IsPow2(ushort value)
	{
		throw null;
	}

	public static ushort LeadingZeroCount(ushort value)
	{
		throw null;
	}

	public static ushort Log2(ushort value)
	{
		throw null;
	}

	public static ushort Max(ushort x, ushort y)
	{
		throw null;
	}

	public static ushort Min(ushort x, ushort y)
	{
		throw null;
	}

	public static ushort Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static ushort Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static ushort Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static ushort Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static ushort Parse(string s)
	{
		throw null;
	}

	public static ushort Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static ushort Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static ushort Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static ushort PopCount(ushort value)
	{
		throw null;
	}

	public static ushort RotateLeft(ushort value, int rotateAmount)
	{
		throw null;
	}

	public static ushort RotateRight(ushort value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(ushort value)
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

	static ushort IAdditionOperators<ushort, ushort, ushort>.operator +(ushort left, ushort right)
	{
		throw null;
	}

	static ushort IAdditionOperators<ushort, ushort, ushort>.operator checked +(ushort left, ushort right)
	{
		throw null;
	}

	int IBinaryInteger<ushort>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<ushort>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<ushort>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out ushort value)
	{
		throw null;
	}

	static bool IBinaryInteger<ushort>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out ushort value)
	{
		throw null;
	}

	bool IBinaryInteger<ushort>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<ushort>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static ushort IBitwiseOperators<ushort, ushort, ushort>.operator &(ushort left, ushort right)
	{
		throw null;
	}

	static ushort IBitwiseOperators<ushort, ushort, ushort>.operator |(ushort left, ushort right)
	{
		throw null;
	}

	static ushort IBitwiseOperators<ushort, ushort, ushort>.operator ^(ushort left, ushort right)
	{
		throw null;
	}

	static ushort IBitwiseOperators<ushort, ushort, ushort>.operator ~(ushort value)
	{
		throw null;
	}

	static bool IComparisonOperators<ushort, ushort, bool>.operator >(ushort left, ushort right)
	{
		throw null;
	}

	static bool IComparisonOperators<ushort, ushort, bool>.operator >=(ushort left, ushort right)
	{
		throw null;
	}

	static bool IComparisonOperators<ushort, ushort, bool>.operator <(ushort left, ushort right)
	{
		throw null;
	}

	static bool IComparisonOperators<ushort, ushort, bool>.operator <=(ushort left, ushort right)
	{
		throw null;
	}

	static ushort IDecrementOperators<ushort>.operator checked --(ushort value)
	{
		throw null;
	}

	static ushort IDecrementOperators<ushort>.operator --(ushort value)
	{
		throw null;
	}

	static ushort IDivisionOperators<ushort, ushort, ushort>.operator /(ushort left, ushort right)
	{
		throw null;
	}

	static bool IEqualityOperators<ushort, ushort, bool>.operator ==(ushort left, ushort right)
	{
		throw null;
	}

	static bool IEqualityOperators<ushort, ushort, bool>.operator !=(ushort left, ushort right)
	{
		throw null;
	}

	static ushort IIncrementOperators<ushort>.operator checked ++(ushort value)
	{
		throw null;
	}

	static ushort IIncrementOperators<ushort>.operator ++(ushort value)
	{
		throw null;
	}

	static ushort IModulusOperators<ushort, ushort, ushort>.operator %(ushort left, ushort right)
	{
		throw null;
	}

	static ushort IMultiplyOperators<ushort, ushort, ushort>.operator checked *(ushort left, ushort right)
	{
		throw null;
	}

	static ushort IMultiplyOperators<ushort, ushort, ushort>.operator *(ushort left, ushort right)
	{
		throw null;
	}

	static ushort INumberBase<ushort>.Abs(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsCanonical(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsComplexNumber(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsFinite(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsImaginaryNumber(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsInfinity(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsInteger(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsNaN(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsNegative(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsNegativeInfinity(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsNormal(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsPositive(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsPositiveInfinity(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsRealNumber(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsSubnormal(ushort value)
	{
		throw null;
	}

	static bool INumberBase<ushort>.IsZero(ushort value)
	{
		throw null;
	}

	static ushort INumberBase<ushort>.MaxMagnitude(ushort x, ushort y)
	{
		throw null;
	}

	static ushort INumberBase<ushort>.MaxMagnitudeNumber(ushort x, ushort y)
	{
		throw null;
	}

	static ushort INumberBase<ushort>.MinMagnitude(ushort x, ushort y)
	{
		throw null;
	}

	static ushort INumberBase<ushort>.MinMagnitudeNumber(ushort x, ushort y)
	{
		throw null;
	}

	static bool INumberBase<ushort>.TryConvertFromChecked<TOther>(TOther value, out ushort result)
	{
		throw null;
	}

	static bool INumberBase<ushort>.TryConvertFromSaturating<TOther>(TOther value, out ushort result)
	{
		throw null;
	}

	static bool INumberBase<ushort>.TryConvertFromTruncating<TOther>(TOther value, out ushort result)
	{
		throw null;
	}

	static bool INumberBase<ushort>.TryConvertToChecked<TOther>(ushort value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<ushort>.TryConvertToSaturating<TOther>(ushort value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<ushort>.TryConvertToTruncating<TOther>(ushort value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static ushort INumber<ushort>.CopySign(ushort value, ushort sign)
	{
		throw null;
	}

	static ushort INumber<ushort>.MaxNumber(ushort x, ushort y)
	{
		throw null;
	}

	static ushort INumber<ushort>.MinNumber(ushort x, ushort y)
	{
		throw null;
	}

	static ushort IShiftOperators<ushort, int, ushort>.operator <<(ushort value, int shiftAmount)
	{
		throw null;
	}

	static ushort IShiftOperators<ushort, int, ushort>.operator >>(ushort value, int shiftAmount)
	{
		throw null;
	}

	static ushort IShiftOperators<ushort, int, ushort>.operator >>>(ushort value, int shiftAmount)
	{
		throw null;
	}

	static ushort ISubtractionOperators<ushort, ushort, ushort>.operator checked -(ushort left, ushort right)
	{
		throw null;
	}

	static ushort ISubtractionOperators<ushort, ushort, ushort>.operator -(ushort left, ushort right)
	{
		throw null;
	}

	static ushort IUnaryNegationOperators<ushort, ushort>.operator checked -(ushort value)
	{
		throw null;
	}

	static ushort IUnaryNegationOperators<ushort, ushort>.operator -(ushort value)
	{
		throw null;
	}

	static ushort IUnaryPlusOperators<ushort, ushort>.operator +(ushort value)
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

	public static ushort TrailingZeroCount(ushort value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out ushort result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out ushort result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out ushort result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out ushort result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out ushort result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out ushort result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out ushort result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out ushort result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out ushort result)
	{
		throw null;
	}
}
