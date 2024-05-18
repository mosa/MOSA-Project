using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

[CLSCompliant(false)]
public readonly struct UInt32 : IComparable, IComparable<uint>, IConvertible, IEquatable<uint>, IFormattable, IParsable<uint>, ISpanFormattable, ISpanParsable<uint>, IAdditionOperators<uint, uint, uint>, IAdditiveIdentity<uint, uint>, IBinaryInteger<uint>, IBinaryNumber<uint>, IBitwiseOperators<uint, uint, uint>, IComparisonOperators<uint, uint, bool>, IEqualityOperators<uint, uint, bool>, IDecrementOperators<uint>, IDivisionOperators<uint, uint, uint>, IIncrementOperators<uint>, IModulusOperators<uint, uint, uint>, IMultiplicativeIdentity<uint, uint>, IMultiplyOperators<uint, uint, uint>, INumber<uint>, INumberBase<uint>, ISubtractionOperators<uint, uint, uint>, IUnaryNegationOperators<uint, uint>, IUnaryPlusOperators<uint, uint>, IUtf8SpanFormattable, IUtf8SpanParsable<uint>, IShiftOperators<uint, int, uint>, IMinMaxValue<uint>, IUnsignedNumber<uint>
{
	private readonly uint _dummyPrimitive;

	public const uint MaxValue = 4294967295u;

	public const uint MinValue = 0u;

	static uint IAdditiveIdentity<uint, uint>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static uint IBinaryNumber<uint>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static uint IMinMaxValue<uint>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static uint IMinMaxValue<uint>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static uint IMultiplicativeIdentity<uint, uint>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static uint INumberBase<uint>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<uint>.Radix
	{
		get
		{
			throw null;
		}
	}

	static uint INumberBase<uint>.Zero
	{
		get
		{
			throw null;
		}
	}

	public static uint Clamp(uint value, uint min, uint max)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public int CompareTo(uint value)
	{
		throw null;
	}

	public static uint CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static uint CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static uint CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (uint Quotient, uint Remainder) DivRem(uint left, uint right)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(uint obj)
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

	public static bool IsEvenInteger(uint value)
	{
		throw null;
	}

	public static bool IsOddInteger(uint value)
	{
		throw null;
	}

	public static bool IsPow2(uint value)
	{
		throw null;
	}

	public static uint LeadingZeroCount(uint value)
	{
		throw null;
	}

	public static uint Log2(uint value)
	{
		throw null;
	}

	public static uint Max(uint x, uint y)
	{
		throw null;
	}

	public static uint Min(uint x, uint y)
	{
		throw null;
	}

	public static uint Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static uint Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static uint Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static uint Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static uint Parse(string s)
	{
		throw null;
	}

	public static uint Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static uint Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static uint Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static uint PopCount(uint value)
	{
		throw null;
	}

	public static uint RotateLeft(uint value, int rotateAmount)
	{
		throw null;
	}

	public static uint RotateRight(uint value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(uint value)
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

	static uint IAdditionOperators<uint, uint, uint>.operator +(uint left, uint right)
	{
		throw null;
	}

	static uint IAdditionOperators<uint, uint, uint>.operator checked +(uint left, uint right)
	{
		throw null;
	}

	int IBinaryInteger<uint>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<uint>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<uint>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out uint value)
	{
		throw null;
	}

	static bool IBinaryInteger<uint>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out uint value)
	{
		throw null;
	}

	bool IBinaryInteger<uint>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<uint>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static uint IBitwiseOperators<uint, uint, uint>.operator &(uint left, uint right)
	{
		throw null;
	}

	static uint IBitwiseOperators<uint, uint, uint>.operator |(uint left, uint right)
	{
		throw null;
	}

	static uint IBitwiseOperators<uint, uint, uint>.operator ^(uint left, uint right)
	{
		throw null;
	}

	static uint IBitwiseOperators<uint, uint, uint>.operator ~(uint value)
	{
		throw null;
	}

	static bool IComparisonOperators<uint, uint, bool>.operator >(uint left, uint right)
	{
		throw null;
	}

	static bool IComparisonOperators<uint, uint, bool>.operator >=(uint left, uint right)
	{
		throw null;
	}

	static bool IComparisonOperators<uint, uint, bool>.operator <(uint left, uint right)
	{
		throw null;
	}

	static bool IComparisonOperators<uint, uint, bool>.operator <=(uint left, uint right)
	{
		throw null;
	}

	static uint IDecrementOperators<uint>.operator checked --(uint value)
	{
		throw null;
	}

	static uint IDecrementOperators<uint>.operator --(uint value)
	{
		throw null;
	}

	static uint IDivisionOperators<uint, uint, uint>.operator /(uint left, uint right)
	{
		throw null;
	}

	static bool IEqualityOperators<uint, uint, bool>.operator ==(uint left, uint right)
	{
		throw null;
	}

	static bool IEqualityOperators<uint, uint, bool>.operator !=(uint left, uint right)
	{
		throw null;
	}

	static uint IIncrementOperators<uint>.operator checked ++(uint value)
	{
		throw null;
	}

	static uint IIncrementOperators<uint>.operator ++(uint value)
	{
		throw null;
	}

	static uint IModulusOperators<uint, uint, uint>.operator %(uint left, uint right)
	{
		throw null;
	}

	static uint IMultiplyOperators<uint, uint, uint>.operator checked *(uint left, uint right)
	{
		throw null;
	}

	static uint IMultiplyOperators<uint, uint, uint>.operator *(uint left, uint right)
	{
		throw null;
	}

	static uint INumberBase<uint>.Abs(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsCanonical(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsComplexNumber(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsFinite(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsImaginaryNumber(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsInfinity(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsInteger(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsNaN(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsNegative(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsNegativeInfinity(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsNormal(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsPositive(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsPositiveInfinity(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsRealNumber(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsSubnormal(uint value)
	{
		throw null;
	}

	static bool INumberBase<uint>.IsZero(uint value)
	{
		throw null;
	}

	static uint INumberBase<uint>.MaxMagnitude(uint x, uint y)
	{
		throw null;
	}

	static uint INumberBase<uint>.MaxMagnitudeNumber(uint x, uint y)
	{
		throw null;
	}

	static uint INumberBase<uint>.MinMagnitude(uint x, uint y)
	{
		throw null;
	}

	static uint INumberBase<uint>.MinMagnitudeNumber(uint x, uint y)
	{
		throw null;
	}

	static bool INumberBase<uint>.TryConvertFromChecked<TOther>(TOther value, out uint result)
	{
		throw null;
	}

	static bool INumberBase<uint>.TryConvertFromSaturating<TOther>(TOther value, out uint result)
	{
		throw null;
	}

	static bool INumberBase<uint>.TryConvertFromTruncating<TOther>(TOther value, out uint result)
	{
		throw null;
	}

	static bool INumberBase<uint>.TryConvertToChecked<TOther>(uint value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<uint>.TryConvertToSaturating<TOther>(uint value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<uint>.TryConvertToTruncating<TOther>(uint value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static uint INumber<uint>.CopySign(uint value, uint sign)
	{
		throw null;
	}

	static uint INumber<uint>.MaxNumber(uint x, uint y)
	{
		throw null;
	}

	static uint INumber<uint>.MinNumber(uint x, uint y)
	{
		throw null;
	}

	static uint IShiftOperators<uint, int, uint>.operator <<(uint value, int shiftAmount)
	{
		throw null;
	}

	static uint IShiftOperators<uint, int, uint>.operator >>(uint value, int shiftAmount)
	{
		throw null;
	}

	static uint IShiftOperators<uint, int, uint>.operator >>>(uint value, int shiftAmount)
	{
		throw null;
	}

	static uint ISubtractionOperators<uint, uint, uint>.operator checked -(uint left, uint right)
	{
		throw null;
	}

	static uint ISubtractionOperators<uint, uint, uint>.operator -(uint left, uint right)
	{
		throw null;
	}

	static uint IUnaryNegationOperators<uint, uint>.operator checked -(uint value)
	{
		throw null;
	}

	static uint IUnaryNegationOperators<uint, uint>.operator -(uint value)
	{
		throw null;
	}

	static uint IUnaryPlusOperators<uint, uint>.operator +(uint value)
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

	public static uint TrailingZeroCount(uint value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out uint result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out uint result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out uint result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out uint result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out uint result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out uint result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out uint result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out uint result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out uint result)
	{
		throw null;
	}
}
