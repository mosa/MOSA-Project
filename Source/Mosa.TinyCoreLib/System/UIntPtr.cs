using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.Serialization;

namespace System;

[CLSCompliant(false)]
public readonly struct UIntPtr : IComparable, IComparable<UIntPtr>, IEquatable<UIntPtr>, IFormattable, IParsable<UIntPtr>, ISpanFormattable, ISpanParsable<UIntPtr>, IAdditionOperators<UIntPtr, UIntPtr, UIntPtr>, IAdditiveIdentity<UIntPtr, UIntPtr>, IBinaryInteger<UIntPtr>, IBinaryNumber<UIntPtr>, IBitwiseOperators<UIntPtr, UIntPtr, UIntPtr>, IComparisonOperators<UIntPtr, UIntPtr, bool>, IEqualityOperators<UIntPtr, UIntPtr, bool>, IDecrementOperators<UIntPtr>, IDivisionOperators<UIntPtr, UIntPtr, UIntPtr>, IIncrementOperators<UIntPtr>, IModulusOperators<UIntPtr, UIntPtr, UIntPtr>, IMultiplicativeIdentity<UIntPtr, UIntPtr>, IMultiplyOperators<UIntPtr, UIntPtr, UIntPtr>, INumber<UIntPtr>, INumberBase<UIntPtr>, ISubtractionOperators<UIntPtr, UIntPtr, UIntPtr>, IUnaryNegationOperators<UIntPtr, UIntPtr>, IUnaryPlusOperators<UIntPtr, UIntPtr>, IUtf8SpanFormattable, IUtf8SpanParsable<UIntPtr>, IShiftOperators<UIntPtr, int, UIntPtr>, IMinMaxValue<UIntPtr>, IUnsignedNumber<UIntPtr>, ISerializable
{
	private unsafe readonly void* _dummyPrimitive;

	public static readonly UIntPtr Zero;

	public static UIntPtr MaxValue
	{
		get
		{
			throw null;
		}
	}

	public static UIntPtr MinValue
	{
		get
		{
			throw null;
		}
	}

	public static int Size
	{
		get
		{
			throw null;
		}
	}

	static UIntPtr IAdditiveIdentity<UIntPtr, UIntPtr>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static UIntPtr IBinaryNumber<UIntPtr>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static UIntPtr IMinMaxValue<UIntPtr>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static UIntPtr IMinMaxValue<UIntPtr>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static UIntPtr IMultiplicativeIdentity<UIntPtr, UIntPtr>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static UIntPtr INumberBase<UIntPtr>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<UIntPtr>.Radix
	{
		get
		{
			throw null;
		}
	}

	static UIntPtr INumberBase<UIntPtr>.Zero
	{
		get
		{
			throw null;
		}
	}

	public UIntPtr(uint value)
	{
		throw null;
	}

	public UIntPtr(ulong value)
	{
		throw null;
	}

	public unsafe UIntPtr(void* value)
	{
		throw null;
	}

	public static UIntPtr Add(UIntPtr pointer, int offset)
	{
		throw null;
	}

	public static UIntPtr Clamp(UIntPtr value, UIntPtr min, UIntPtr max)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public int CompareTo(UIntPtr value)
	{
		throw null;
	}

	public static UIntPtr CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static UIntPtr CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static UIntPtr CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (UIntPtr Quotient, UIntPtr Remainder) DivRem(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(UIntPtr other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool IsEvenInteger(UIntPtr value)
	{
		throw null;
	}

	public static bool IsOddInteger(UIntPtr value)
	{
		throw null;
	}

	public static bool IsPow2(UIntPtr value)
	{
		throw null;
	}

	public static UIntPtr LeadingZeroCount(UIntPtr value)
	{
		throw null;
	}

	public static UIntPtr Log2(UIntPtr value)
	{
		throw null;
	}

	public static UIntPtr Max(UIntPtr x, UIntPtr y)
	{
		throw null;
	}

	public static UIntPtr Min(UIntPtr x, UIntPtr y)
	{
		throw null;
	}

	public static UIntPtr operator +(UIntPtr pointer, int offset)
	{
		throw null;
	}

	public static bool operator ==(UIntPtr value1, UIntPtr value2)
	{
		throw null;
	}

	public static explicit operator UIntPtr(uint value)
	{
		throw null;
	}

	public static explicit operator UIntPtr(ulong value)
	{
		throw null;
	}

	public static explicit operator uint(UIntPtr value)
	{
		throw null;
	}

	public static explicit operator ulong(UIntPtr value)
	{
		throw null;
	}

	public unsafe static explicit operator void*(UIntPtr value)
	{
		throw null;
	}

	public unsafe static explicit operator UIntPtr(void* value)
	{
		throw null;
	}

	public static bool operator !=(UIntPtr value1, UIntPtr value2)
	{
		throw null;
	}

	public static UIntPtr operator -(UIntPtr pointer, int offset)
	{
		throw null;
	}

	public static UIntPtr Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static UIntPtr Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static UIntPtr Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static UIntPtr Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static UIntPtr Parse(string s)
	{
		throw null;
	}

	public static UIntPtr Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static UIntPtr Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static UIntPtr Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static UIntPtr PopCount(UIntPtr value)
	{
		throw null;
	}

	public static UIntPtr RotateLeft(UIntPtr value, int rotateAmount)
	{
		throw null;
	}

	public static UIntPtr RotateRight(UIntPtr value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(UIntPtr value)
	{
		throw null;
	}

	public static UIntPtr Subtract(UIntPtr pointer, int offset)
	{
		throw null;
	}

	static UIntPtr IAdditionOperators<UIntPtr, UIntPtr, UIntPtr>.operator +(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr IAdditionOperators<UIntPtr, UIntPtr, UIntPtr>.operator checked +(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	int IBinaryInteger<UIntPtr>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<UIntPtr>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<UIntPtr>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UIntPtr value)
	{
		throw null;
	}

	static bool IBinaryInteger<UIntPtr>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UIntPtr value)
	{
		throw null;
	}

	bool IBinaryInteger<UIntPtr>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<UIntPtr>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static UIntPtr IBitwiseOperators<UIntPtr, UIntPtr, UIntPtr>.operator &(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr IBitwiseOperators<UIntPtr, UIntPtr, UIntPtr>.operator |(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr IBitwiseOperators<UIntPtr, UIntPtr, UIntPtr>.operator ^(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr IBitwiseOperators<UIntPtr, UIntPtr, UIntPtr>.operator ~(UIntPtr value)
	{
		throw null;
	}

	static bool IComparisonOperators<UIntPtr, UIntPtr, bool>.operator >(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static bool IComparisonOperators<UIntPtr, UIntPtr, bool>.operator >=(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static bool IComparisonOperators<UIntPtr, UIntPtr, bool>.operator <(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static bool IComparisonOperators<UIntPtr, UIntPtr, bool>.operator <=(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr IDecrementOperators<UIntPtr>.operator checked --(UIntPtr value)
	{
		throw null;
	}

	static UIntPtr IDecrementOperators<UIntPtr>.operator --(UIntPtr value)
	{
		throw null;
	}

	static UIntPtr IDivisionOperators<UIntPtr, UIntPtr, UIntPtr>.operator /(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr IIncrementOperators<UIntPtr>.operator checked ++(UIntPtr value)
	{
		throw null;
	}

	static UIntPtr IIncrementOperators<UIntPtr>.operator ++(UIntPtr value)
	{
		throw null;
	}

	static UIntPtr IModulusOperators<UIntPtr, UIntPtr, UIntPtr>.operator %(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr IMultiplyOperators<UIntPtr, UIntPtr, UIntPtr>.operator checked *(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr IMultiplyOperators<UIntPtr, UIntPtr, UIntPtr>.operator *(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr INumberBase<UIntPtr>.Abs(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsCanonical(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsComplexNumber(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsFinite(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsImaginaryNumber(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsInfinity(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsInteger(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsNaN(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsNegative(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsNegativeInfinity(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsNormal(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsPositive(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsPositiveInfinity(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsRealNumber(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsSubnormal(UIntPtr value)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.IsZero(UIntPtr value)
	{
		throw null;
	}

	static UIntPtr INumberBase<UIntPtr>.MaxMagnitude(UIntPtr x, UIntPtr y)
	{
		throw null;
	}

	static UIntPtr INumberBase<UIntPtr>.MaxMagnitudeNumber(UIntPtr x, UIntPtr y)
	{
		throw null;
	}

	static UIntPtr INumberBase<UIntPtr>.MinMagnitude(UIntPtr x, UIntPtr y)
	{
		throw null;
	}

	static UIntPtr INumberBase<UIntPtr>.MinMagnitudeNumber(UIntPtr x, UIntPtr y)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.TryConvertFromChecked<TOther>(TOther value, out UIntPtr result)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.TryConvertFromSaturating<TOther>(TOther value, out UIntPtr result)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.TryConvertFromTruncating<TOther>(TOther value, out UIntPtr result)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.TryConvertToChecked<TOther>(UIntPtr value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.TryConvertToSaturating<TOther>(UIntPtr value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<UIntPtr>.TryConvertToTruncating<TOther>(UIntPtr value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static UIntPtr INumber<UIntPtr>.CopySign(UIntPtr value, UIntPtr sign)
	{
		throw null;
	}

	static UIntPtr INumber<UIntPtr>.MaxNumber(UIntPtr x, UIntPtr y)
	{
		throw null;
	}

	static UIntPtr INumber<UIntPtr>.MinNumber(UIntPtr x, UIntPtr y)
	{
		throw null;
	}

	static UIntPtr IShiftOperators<UIntPtr, int, UIntPtr>.operator <<(UIntPtr value, int shiftAmount)
	{
		throw null;
	}

	static UIntPtr IShiftOperators<UIntPtr, int, UIntPtr>.operator >>(UIntPtr value, int shiftAmount)
	{
		throw null;
	}

	static UIntPtr IShiftOperators<UIntPtr, int, UIntPtr>.operator >>>(UIntPtr value, int shiftAmount)
	{
		throw null;
	}

	static UIntPtr ISubtractionOperators<UIntPtr, UIntPtr, UIntPtr>.operator checked -(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr ISubtractionOperators<UIntPtr, UIntPtr, UIntPtr>.operator -(UIntPtr left, UIntPtr right)
	{
		throw null;
	}

	static UIntPtr IUnaryNegationOperators<UIntPtr, UIntPtr>.operator checked -(UIntPtr value)
	{
		throw null;
	}

	static UIntPtr IUnaryNegationOperators<UIntPtr, UIntPtr>.operator -(UIntPtr value)
	{
		throw null;
	}

	static UIntPtr IUnaryPlusOperators<UIntPtr, UIntPtr>.operator +(UIntPtr value)
	{
		throw null;
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public unsafe void* ToPointer()
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

	public uint ToUInt32()
	{
		throw null;
	}

	public ulong ToUInt64()
	{
		throw null;
	}

	public static UIntPtr TrailingZeroCount(UIntPtr value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out UIntPtr result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out UIntPtr result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out UIntPtr result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out UIntPtr result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out UIntPtr result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out UIntPtr result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out UIntPtr result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out UIntPtr result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out UIntPtr result)
	{
		throw null;
	}
}
