using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

public readonly struct Int64 : IComparable, IComparable<long>, IConvertible, IEquatable<long>, IFormattable, IParsable<long>, ISpanFormattable, ISpanParsable<long>, IAdditionOperators<long, long, long>, IAdditiveIdentity<long, long>, IBinaryInteger<long>, IBinaryNumber<long>, IBitwiseOperators<long, long, long>, IComparisonOperators<long, long, bool>, IEqualityOperators<long, long, bool>, IDecrementOperators<long>, IDivisionOperators<long, long, long>, IIncrementOperators<long>, IModulusOperators<long, long, long>, IMultiplicativeIdentity<long, long>, IMultiplyOperators<long, long, long>, INumber<long>, INumberBase<long>, ISubtractionOperators<long, long, long>, IUnaryNegationOperators<long, long>, IUnaryPlusOperators<long, long>, IUtf8SpanFormattable, IUtf8SpanParsable<long>, IShiftOperators<long, int, long>, IMinMaxValue<long>, ISignedNumber<long>
{
	private readonly long _dummyPrimitive;

	public const long MaxValue = 9223372036854775807L;

	public const long MinValue = -9223372036854775808L;

	static long IAdditiveIdentity<long, long>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static long IBinaryNumber<long>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static long IMinMaxValue<long>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static long IMinMaxValue<long>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static long IMultiplicativeIdentity<long, long>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static long INumberBase<long>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<long>.Radix
	{
		get
		{
			throw null;
		}
	}

	static long INumberBase<long>.Zero
	{
		get
		{
			throw null;
		}
	}

	static long ISignedNumber<long>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public static long Abs(long value)
	{
		throw null;
	}

	public static long Clamp(long value, long min, long max)
	{
		throw null;
	}

	public int CompareTo(long value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static long CopySign(long value, long sign)
	{
		throw null;
	}

	public static long CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static long CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static long CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (long Quotient, long Remainder) DivRem(long left, long right)
	{
		throw null;
	}

	public bool Equals(long obj)
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

	public static bool IsEvenInteger(long value)
	{
		throw null;
	}

	public static bool IsNegative(long value)
	{
		throw null;
	}

	public static bool IsOddInteger(long value)
	{
		throw null;
	}

	public static bool IsPositive(long value)
	{
		throw null;
	}

	public static bool IsPow2(long value)
	{
		throw null;
	}

	public static long LeadingZeroCount(long value)
	{
		throw null;
	}

	public static long Log2(long value)
	{
		throw null;
	}

	public static long Max(long x, long y)
	{
		throw null;
	}

	public static long MaxMagnitude(long x, long y)
	{
		throw null;
	}

	public static long Min(long x, long y)
	{
		throw null;
	}

	public static long MinMagnitude(long x, long y)
	{
		throw null;
	}

	public static long Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static long Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static long Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static long Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static long Parse(string s)
	{
		throw null;
	}

	public static long Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static long Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static long Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static long PopCount(long value)
	{
		throw null;
	}

	public static long RotateLeft(long value, int rotateAmount)
	{
		throw null;
	}

	public static long RotateRight(long value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(long value)
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

	static long IAdditionOperators<long, long, long>.operator +(long left, long right)
	{
		throw null;
	}

	static long IAdditionOperators<long, long, long>.operator checked +(long left, long right)
	{
		throw null;
	}

	int IBinaryInteger<long>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<long>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<long>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out long value)
	{
		throw null;
	}

	static bool IBinaryInteger<long>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out long value)
	{
		throw null;
	}

	bool IBinaryInteger<long>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<long>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static long IBitwiseOperators<long, long, long>.operator &(long left, long right)
	{
		throw null;
	}

	static long IBitwiseOperators<long, long, long>.operator |(long left, long right)
	{
		throw null;
	}

	static long IBitwiseOperators<long, long, long>.operator ^(long left, long right)
	{
		throw null;
	}

	static long IBitwiseOperators<long, long, long>.operator ~(long value)
	{
		throw null;
	}

	static bool IComparisonOperators<long, long, bool>.operator >(long left, long right)
	{
		throw null;
	}

	static bool IComparisonOperators<long, long, bool>.operator >=(long left, long right)
	{
		throw null;
	}

	static bool IComparisonOperators<long, long, bool>.operator <(long left, long right)
	{
		throw null;
	}

	static bool IComparisonOperators<long, long, bool>.operator <=(long left, long right)
	{
		throw null;
	}

	static long IDecrementOperators<long>.operator checked --(long value)
	{
		throw null;
	}

	static long IDecrementOperators<long>.operator --(long value)
	{
		throw null;
	}

	static long IDivisionOperators<long, long, long>.operator /(long left, long right)
	{
		throw null;
	}

	static bool IEqualityOperators<long, long, bool>.operator ==(long left, long right)
	{
		throw null;
	}

	static bool IEqualityOperators<long, long, bool>.operator !=(long left, long right)
	{
		throw null;
	}

	static long IIncrementOperators<long>.operator checked ++(long value)
	{
		throw null;
	}

	static long IIncrementOperators<long>.operator ++(long value)
	{
		throw null;
	}

	static long IModulusOperators<long, long, long>.operator %(long left, long right)
	{
		throw null;
	}

	static long IMultiplyOperators<long, long, long>.operator checked *(long left, long right)
	{
		throw null;
	}

	static long IMultiplyOperators<long, long, long>.operator *(long left, long right)
	{
		throw null;
	}

	static bool INumberBase<long>.IsCanonical(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsComplexNumber(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsFinite(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsImaginaryNumber(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsInfinity(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsInteger(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsNaN(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsNegativeInfinity(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsNormal(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsPositiveInfinity(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsRealNumber(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsSubnormal(long value)
	{
		throw null;
	}

	static bool INumberBase<long>.IsZero(long value)
	{
		throw null;
	}

	static long INumberBase<long>.MaxMagnitudeNumber(long x, long y)
	{
		throw null;
	}

	static long INumberBase<long>.MinMagnitudeNumber(long x, long y)
	{
		throw null;
	}

	static bool INumberBase<long>.TryConvertFromChecked<TOther>(TOther value, out long result)
	{
		throw null;
	}

	static bool INumberBase<long>.TryConvertFromSaturating<TOther>(TOther value, out long result)
	{
		throw null;
	}

	static bool INumberBase<long>.TryConvertFromTruncating<TOther>(TOther value, out long result)
	{
		throw null;
	}

	static bool INumberBase<long>.TryConvertToChecked<TOther>(long value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<long>.TryConvertToSaturating<TOther>(long value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<long>.TryConvertToTruncating<TOther>(long value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static long INumber<long>.MaxNumber(long x, long y)
	{
		throw null;
	}

	static long INumber<long>.MinNumber(long x, long y)
	{
		throw null;
	}

	static long IShiftOperators<long, int, long>.operator <<(long value, int shiftAmount)
	{
		throw null;
	}

	static long IShiftOperators<long, int, long>.operator >>(long value, int shiftAmount)
	{
		throw null;
	}

	static long IShiftOperators<long, int, long>.operator >>>(long value, int shiftAmount)
	{
		throw null;
	}

	static long ISubtractionOperators<long, long, long>.operator checked -(long left, long right)
	{
		throw null;
	}

	static long ISubtractionOperators<long, long, long>.operator -(long left, long right)
	{
		throw null;
	}

	static long IUnaryNegationOperators<long, long>.operator checked -(long value)
	{
		throw null;
	}

	static long IUnaryNegationOperators<long, long>.operator -(long value)
	{
		throw null;
	}

	static long IUnaryPlusOperators<long, long>.operator +(long value)
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

	public static long TrailingZeroCount(long value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out long result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out long result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out long result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out long result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out long result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out long result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out long result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out long result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out long result)
	{
		throw null;
	}
}
