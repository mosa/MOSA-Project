using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

public readonly struct Int32 : IComparable, IComparable<int>, IConvertible, IEquatable<int>, IFormattable, IParsable<int>, ISpanFormattable, ISpanParsable<int>, IAdditionOperators<int, int, int>, IAdditiveIdentity<int, int>, IBinaryInteger<int>, IBinaryNumber<int>, IBitwiseOperators<int, int, int>, IComparisonOperators<int, int, bool>, IEqualityOperators<int, int, bool>, IDecrementOperators<int>, IDivisionOperators<int, int, int>, IIncrementOperators<int>, IModulusOperators<int, int, int>, IMultiplicativeIdentity<int, int>, IMultiplyOperators<int, int, int>, INumber<int>, INumberBase<int>, ISubtractionOperators<int, int, int>, IUnaryNegationOperators<int, int>, IUnaryPlusOperators<int, int>, IUtf8SpanFormattable, IUtf8SpanParsable<int>, IShiftOperators<int, int, int>, IMinMaxValue<int>, ISignedNumber<int>
{
	private readonly int _dummyPrimitive;

	public const int MaxValue = 2147483647;

	public const int MinValue = -2147483648;

	static int IAdditiveIdentity<int, int>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static int IBinaryNumber<int>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static int IMinMaxValue<int>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static int IMinMaxValue<int>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static int IMultiplicativeIdentity<int, int>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<int>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<int>.Radix
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<int>.Zero
	{
		get
		{
			throw null;
		}
	}

	static int ISignedNumber<int>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public static int Abs(int value)
	{
		throw null;
	}

	public static int Clamp(int value, int min, int max)
	{
		throw null;
	}

	public int CompareTo(int value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static int CopySign(int value, int sign)
	{
		throw null;
	}

	public static int CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static int CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static int CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (int Quotient, int Remainder) DivRem(int left, int right)
	{
		throw null;
	}

	public bool Equals(int obj)
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

	public static bool IsEvenInteger(int value)
	{
		throw null;
	}

	public static bool IsNegative(int value)
	{
		throw null;
	}

	public static bool IsOddInteger(int value)
	{
		throw null;
	}

	public static bool IsPositive(int value)
	{
		throw null;
	}

	public static bool IsPow2(int value)
	{
		throw null;
	}

	public static int LeadingZeroCount(int value)
	{
		throw null;
	}

	public static int Log2(int value)
	{
		throw null;
	}

	public static int Max(int x, int y)
	{
		throw null;
	}

	public static int MaxMagnitude(int x, int y)
	{
		throw null;
	}

	public static int Min(int x, int y)
	{
		throw null;
	}

	public static int MinMagnitude(int x, int y)
	{
		throw null;
	}

	public static int Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static int Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static int Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static int Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static int Parse(string s)
	{
		throw null;
	}

	public static int Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static int Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static int Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static int PopCount(int value)
	{
		throw null;
	}

	public static int RotateLeft(int value, int rotateAmount)
	{
		throw null;
	}

	public static int RotateRight(int value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(int value)
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

	static int IAdditionOperators<int, int, int>.operator +(int left, int right)
	{
		throw null;
	}

	static int IAdditionOperators<int, int, int>.operator checked +(int left, int right)
	{
		throw null;
	}

	int IBinaryInteger<int>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<int>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<int>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out int value)
	{
		throw null;
	}

	static bool IBinaryInteger<int>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out int value)
	{
		throw null;
	}

	bool IBinaryInteger<int>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<int>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static int IBitwiseOperators<int, int, int>.operator &(int left, int right)
	{
		throw null;
	}

	static int IBitwiseOperators<int, int, int>.operator |(int left, int right)
	{
		throw null;
	}

	static int IBitwiseOperators<int, int, int>.operator ^(int left, int right)
	{
		throw null;
	}

	static int IBitwiseOperators<int, int, int>.operator ~(int value)
	{
		throw null;
	}

	static bool IComparisonOperators<int, int, bool>.operator >(int left, int right)
	{
		throw null;
	}

	static bool IComparisonOperators<int, int, bool>.operator >=(int left, int right)
	{
		throw null;
	}

	static bool IComparisonOperators<int, int, bool>.operator <(int left, int right)
	{
		throw null;
	}

	static bool IComparisonOperators<int, int, bool>.operator <=(int left, int right)
	{
		throw null;
	}

	static int IDecrementOperators<int>.operator checked --(int value)
	{
		throw null;
	}

	static int IDecrementOperators<int>.operator --(int value)
	{
		throw null;
	}

	static int IDivisionOperators<int, int, int>.operator /(int left, int right)
	{
		throw null;
	}

	static bool IEqualityOperators<int, int, bool>.operator ==(int left, int right)
	{
		throw null;
	}

	static bool IEqualityOperators<int, int, bool>.operator !=(int left, int right)
	{
		throw null;
	}

	static int IIncrementOperators<int>.operator checked ++(int value)
	{
		throw null;
	}

	static int IIncrementOperators<int>.operator ++(int value)
	{
		throw null;
	}

	static int IModulusOperators<int, int, int>.operator %(int left, int right)
	{
		throw null;
	}

	static int IMultiplyOperators<int, int, int>.operator checked *(int left, int right)
	{
		throw null;
	}

	static int IMultiplyOperators<int, int, int>.operator *(int left, int right)
	{
		throw null;
	}

	static bool INumberBase<int>.IsCanonical(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsComplexNumber(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsFinite(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsImaginaryNumber(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsInfinity(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsInteger(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsNaN(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsNegativeInfinity(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsNormal(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsPositiveInfinity(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsRealNumber(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsSubnormal(int value)
	{
		throw null;
	}

	static bool INumberBase<int>.IsZero(int value)
	{
		throw null;
	}

	static int INumberBase<int>.MaxMagnitudeNumber(int x, int y)
	{
		throw null;
	}

	static int INumberBase<int>.MinMagnitudeNumber(int x, int y)
	{
		throw null;
	}

	static bool INumberBase<int>.TryConvertFromChecked<TOther>(TOther value, out int result)
	{
		throw null;
	}

	static bool INumberBase<int>.TryConvertFromSaturating<TOther>(TOther value, out int result)
	{
		throw null;
	}

	static bool INumberBase<int>.TryConvertFromTruncating<TOther>(TOther value, out int result)
	{
		throw null;
	}

	static bool INumberBase<int>.TryConvertToChecked<TOther>(int value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<int>.TryConvertToSaturating<TOther>(int value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<int>.TryConvertToTruncating<TOther>(int value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static int INumber<int>.MaxNumber(int x, int y)
	{
		throw null;
	}

	static int INumber<int>.MinNumber(int x, int y)
	{
		throw null;
	}

	static int IShiftOperators<int, int, int>.operator <<(int value, int shiftAmount)
	{
		throw null;
	}

	static int IShiftOperators<int, int, int>.operator >>(int value, int shiftAmount)
	{
		throw null;
	}

	static int IShiftOperators<int, int, int>.operator >>>(int value, int shiftAmount)
	{
		throw null;
	}

	static int ISubtractionOperators<int, int, int>.operator checked -(int left, int right)
	{
		throw null;
	}

	static int ISubtractionOperators<int, int, int>.operator -(int left, int right)
	{
		throw null;
	}

	static int IUnaryNegationOperators<int, int>.operator checked -(int value)
	{
		throw null;
	}

	static int IUnaryNegationOperators<int, int>.operator -(int value)
	{
		throw null;
	}

	static int IUnaryPlusOperators<int, int>.operator +(int value)
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

	public static int TrailingZeroCount(int value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out int result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out int result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out int result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out int result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out int result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out int result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out int result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out int result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out int result)
	{
		throw null;
	}
}
