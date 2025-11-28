using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

[CLSCompliant(false)]
public readonly struct UInt64 : IComparable, IComparable<ulong>, IConvertible, IEquatable<ulong>, IFormattable, IParsable<ulong>, ISpanFormattable, ISpanParsable<ulong>, IAdditionOperators<ulong, ulong, ulong>, IAdditiveIdentity<ulong, ulong>, IBinaryInteger<ulong>, IBinaryNumber<ulong>, IBitwiseOperators<ulong, ulong, ulong>, IComparisonOperators<ulong, ulong, bool>, IEqualityOperators<ulong, ulong, bool>, IDecrementOperators<ulong>, IDivisionOperators<ulong, ulong, ulong>, IIncrementOperators<ulong>, IModulusOperators<ulong, ulong, ulong>, IMultiplicativeIdentity<ulong, ulong>, IMultiplyOperators<ulong, ulong, ulong>, INumber<ulong>, INumberBase<ulong>, ISubtractionOperators<ulong, ulong, ulong>, IUnaryNegationOperators<ulong, ulong>, IUnaryPlusOperators<ulong, ulong>, IUtf8SpanFormattable, IUtf8SpanParsable<ulong>, IShiftOperators<ulong, int, ulong>, IMinMaxValue<ulong>, IUnsignedNumber<ulong>
{
	private readonly ulong _dummyPrimitive;

	public const ulong MaxValue = 18446744073709551615uL;

	public const ulong MinValue = 0uL;

	static ulong IAdditiveIdentity<ulong, ulong>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static ulong IBinaryNumber<ulong>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static ulong IMinMaxValue<ulong>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static ulong IMinMaxValue<ulong>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static ulong IMultiplicativeIdentity<ulong, ulong>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static ulong INumberBase<ulong>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<ulong>.Radix
	{
		get
		{
			throw null;
		}
	}

	static ulong INumberBase<ulong>.Zero
	{
		get
		{
			throw null;
		}
	}

	public static ulong Clamp(ulong value, ulong min, ulong max)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public int CompareTo(ulong value)
	{
		throw null;
	}

	public static ulong CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static ulong CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static ulong CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (ulong Quotient, ulong Remainder) DivRem(ulong left, ulong right)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(ulong obj)
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

	public static bool IsEvenInteger(ulong value)
	{
		throw null;
	}

	public static bool IsOddInteger(ulong value)
	{
		throw null;
	}

	public static bool IsPow2(ulong value)
	{
		throw null;
	}

	public static ulong LeadingZeroCount(ulong value)
	{
		throw null;
	}

	public static ulong Log2(ulong value)
	{
		throw null;
	}

	public static ulong Max(ulong x, ulong y)
	{
		throw null;
	}

	public static ulong Min(ulong x, ulong y)
	{
		throw null;
	}

	public static ulong Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static ulong Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static ulong Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static ulong Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static ulong Parse(string s)
	{
		throw null;
	}

	public static ulong Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static ulong Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static ulong Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static ulong PopCount(ulong value)
	{
		throw null;
	}

	public static ulong RotateLeft(ulong value, int rotateAmount)
	{
		throw null;
	}

	public static ulong RotateRight(ulong value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(ulong value)
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

	static ulong IAdditionOperators<ulong, ulong, ulong>.operator +(ulong left, ulong right)
	{
		throw null;
	}

	static ulong IAdditionOperators<ulong, ulong, ulong>.operator checked +(ulong left, ulong right)
	{
		throw null;
	}

	int IBinaryInteger<ulong>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<ulong>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<ulong>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out ulong value)
	{
		throw null;
	}

	static bool IBinaryInteger<ulong>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out ulong value)
	{
		throw null;
	}

	bool IBinaryInteger<ulong>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<ulong>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static ulong IBitwiseOperators<ulong, ulong, ulong>.operator &(ulong left, ulong right)
	{
		throw null;
	}

	static ulong IBitwiseOperators<ulong, ulong, ulong>.operator |(ulong left, ulong right)
	{
		throw null;
	}

	static ulong IBitwiseOperators<ulong, ulong, ulong>.operator ^(ulong left, ulong right)
	{
		throw null;
	}

	static ulong IBitwiseOperators<ulong, ulong, ulong>.operator ~(ulong value)
	{
		throw null;
	}

	static bool IComparisonOperators<ulong, ulong, bool>.operator >(ulong left, ulong right)
	{
		throw null;
	}

	static bool IComparisonOperators<ulong, ulong, bool>.operator >=(ulong left, ulong right)
	{
		throw null;
	}

	static bool IComparisonOperators<ulong, ulong, bool>.operator <(ulong left, ulong right)
	{
		throw null;
	}

	static bool IComparisonOperators<ulong, ulong, bool>.operator <=(ulong left, ulong right)
	{
		throw null;
	}

	static ulong IDecrementOperators<ulong>.operator checked --(ulong value)
	{
		throw null;
	}

	static ulong IDecrementOperators<ulong>.operator --(ulong value)
	{
		throw null;
	}

	static ulong IDivisionOperators<ulong, ulong, ulong>.operator /(ulong left, ulong right)
	{
		throw null;
	}

	static bool IEqualityOperators<ulong, ulong, bool>.operator ==(ulong left, ulong right)
	{
		throw null;
	}

	static bool IEqualityOperators<ulong, ulong, bool>.operator !=(ulong left, ulong right)
	{
		throw null;
	}

	static ulong IIncrementOperators<ulong>.operator checked ++(ulong value)
	{
		throw null;
	}

	static ulong IIncrementOperators<ulong>.operator ++(ulong value)
	{
		throw null;
	}

	static ulong IModulusOperators<ulong, ulong, ulong>.operator %(ulong left, ulong right)
	{
		throw null;
	}

	static ulong IMultiplyOperators<ulong, ulong, ulong>.operator checked *(ulong left, ulong right)
	{
		throw null;
	}

	static ulong IMultiplyOperators<ulong, ulong, ulong>.operator *(ulong left, ulong right)
	{
		throw null;
	}

	static ulong INumberBase<ulong>.Abs(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsCanonical(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsComplexNumber(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsFinite(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsImaginaryNumber(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsInfinity(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsInteger(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsNaN(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsNegative(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsNegativeInfinity(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsNormal(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsPositive(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsPositiveInfinity(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsRealNumber(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsSubnormal(ulong value)
	{
		throw null;
	}

	static bool INumberBase<ulong>.IsZero(ulong value)
	{
		throw null;
	}

	static ulong INumberBase<ulong>.MaxMagnitude(ulong x, ulong y)
	{
		throw null;
	}

	static ulong INumberBase<ulong>.MaxMagnitudeNumber(ulong x, ulong y)
	{
		throw null;
	}

	static ulong INumberBase<ulong>.MinMagnitude(ulong x, ulong y)
	{
		throw null;
	}

	static ulong INumberBase<ulong>.MinMagnitudeNumber(ulong x, ulong y)
	{
		throw null;
	}

	static bool INumberBase<ulong>.TryConvertFromChecked<TOther>(TOther value, out ulong result)
	{
		throw null;
	}

	static bool INumberBase<ulong>.TryConvertFromSaturating<TOther>(TOther value, out ulong result)
	{
		throw null;
	}

	static bool INumberBase<ulong>.TryConvertFromTruncating<TOther>(TOther value, out ulong result)
	{
		throw null;
	}

	static bool INumberBase<ulong>.TryConvertToChecked<TOther>(ulong value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<ulong>.TryConvertToSaturating<TOther>(ulong value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<ulong>.TryConvertToTruncating<TOther>(ulong value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static ulong INumber<ulong>.CopySign(ulong value, ulong sign)
	{
		throw null;
	}

	static ulong INumber<ulong>.MaxNumber(ulong x, ulong y)
	{
		throw null;
	}

	static ulong INumber<ulong>.MinNumber(ulong x, ulong y)
	{
		throw null;
	}

	static ulong IShiftOperators<ulong, int, ulong>.operator <<(ulong value, int shiftAmount)
	{
		throw null;
	}

	static ulong IShiftOperators<ulong, int, ulong>.operator >>(ulong value, int shiftAmount)
	{
		throw null;
	}

	static ulong IShiftOperators<ulong, int, ulong>.operator >>>(ulong value, int shiftAmount)
	{
		throw null;
	}

	static ulong ISubtractionOperators<ulong, ulong, ulong>.operator checked -(ulong left, ulong right)
	{
		throw null;
	}

	static ulong ISubtractionOperators<ulong, ulong, ulong>.operator -(ulong left, ulong right)
	{
		throw null;
	}

	static ulong IUnaryNegationOperators<ulong, ulong>.operator checked -(ulong value)
	{
		throw null;
	}

	static ulong IUnaryNegationOperators<ulong, ulong>.operator -(ulong value)
	{
		throw null;
	}

	static ulong IUnaryPlusOperators<ulong, ulong>.operator +(ulong value)
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

	public static ulong TrailingZeroCount(ulong value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out ulong result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out ulong result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out ulong result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out ulong result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out ulong result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out ulong result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out ulong result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out ulong result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out ulong result)
	{
		throw null;
	}
}
