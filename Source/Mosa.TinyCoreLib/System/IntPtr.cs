using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.Serialization;

namespace System;

public readonly struct IntPtr : IComparable, IComparable<IntPtr>, IEquatable<IntPtr>, IFormattable, IParsable<IntPtr>, ISpanFormattable, ISpanParsable<IntPtr>, IAdditionOperators<IntPtr, IntPtr, IntPtr>, IAdditiveIdentity<IntPtr, IntPtr>, IBinaryInteger<IntPtr>, IBinaryNumber<IntPtr>, IBitwiseOperators<IntPtr, IntPtr, IntPtr>, IComparisonOperators<IntPtr, IntPtr, bool>, IEqualityOperators<IntPtr, IntPtr, bool>, IDecrementOperators<IntPtr>, IDivisionOperators<IntPtr, IntPtr, IntPtr>, IIncrementOperators<IntPtr>, IModulusOperators<IntPtr, IntPtr, IntPtr>, IMultiplicativeIdentity<IntPtr, IntPtr>, IMultiplyOperators<IntPtr, IntPtr, IntPtr>, INumber<IntPtr>, INumberBase<IntPtr>, ISubtractionOperators<IntPtr, IntPtr, IntPtr>, IUnaryNegationOperators<IntPtr, IntPtr>, IUnaryPlusOperators<IntPtr, IntPtr>, IUtf8SpanFormattable, IUtf8SpanParsable<IntPtr>, IShiftOperators<IntPtr, int, IntPtr>, IMinMaxValue<IntPtr>, ISignedNumber<IntPtr>, ISerializable
{
	private unsafe readonly void* _dummyPrimitive;

	public static readonly IntPtr Zero;

	public static IntPtr MaxValue
	{
		get
		{
			throw null;
		}
	}

	public static IntPtr MinValue
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

	static IntPtr IAdditiveIdentity<IntPtr, IntPtr>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static IntPtr IBinaryNumber<IntPtr>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static IntPtr IMinMaxValue<IntPtr>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static IntPtr IMinMaxValue<IntPtr>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static IntPtr IMultiplicativeIdentity<IntPtr, IntPtr>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static IntPtr INumberBase<IntPtr>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<IntPtr>.Radix
	{
		get
		{
			throw null;
		}
	}

	static IntPtr INumberBase<IntPtr>.Zero
	{
		get
		{
			throw null;
		}
	}

	static IntPtr ISignedNumber<IntPtr>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public IntPtr(int value)
	{
		throw null;
	}

	public IntPtr(long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe IntPtr(void* value)
	{
		throw null;
	}

	public static IntPtr Abs(IntPtr value)
	{
		throw null;
	}

	public static IntPtr Add(IntPtr pointer, int offset)
	{
		throw null;
	}

	public static IntPtr Clamp(IntPtr value, IntPtr min, IntPtr max)
	{
		throw null;
	}

	public int CompareTo(IntPtr value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static IntPtr CopySign(IntPtr value, IntPtr sign)
	{
		throw null;
	}

	public static IntPtr CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static IntPtr CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static IntPtr CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static (IntPtr Quotient, IntPtr Remainder) DivRem(IntPtr left, IntPtr right)
	{
		throw null;
	}

	public bool Equals(IntPtr other)
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

	public static bool IsEvenInteger(IntPtr value)
	{
		throw null;
	}

	public static bool IsNegative(IntPtr value)
	{
		throw null;
	}

	public static bool IsOddInteger(IntPtr value)
	{
		throw null;
	}

	public static bool IsPositive(IntPtr value)
	{
		throw null;
	}

	public static bool IsPow2(IntPtr value)
	{
		throw null;
	}

	public static IntPtr LeadingZeroCount(IntPtr value)
	{
		throw null;
	}

	public static IntPtr Log2(IntPtr value)
	{
		throw null;
	}

	public static IntPtr Max(IntPtr x, IntPtr y)
	{
		throw null;
	}

	public static IntPtr MaxMagnitude(IntPtr x, IntPtr y)
	{
		throw null;
	}

	public static IntPtr Min(IntPtr x, IntPtr y)
	{
		throw null;
	}

	public static IntPtr MinMagnitude(IntPtr x, IntPtr y)
	{
		throw null;
	}

	public static IntPtr operator +(IntPtr pointer, int offset)
	{
		throw null;
	}

	public static bool operator ==(IntPtr value1, IntPtr value2)
	{
		throw null;
	}

	public static explicit operator IntPtr(int value)
	{
		throw null;
	}

	public static explicit operator IntPtr(long value)
	{
		throw null;
	}

	public static explicit operator int(IntPtr value)
	{
		throw null;
	}

	public static explicit operator long(IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static explicit operator void*(IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static explicit operator IntPtr(void* value)
	{
		throw null;
	}

	public static bool operator !=(IntPtr value1, IntPtr value2)
	{
		throw null;
	}

	public static IntPtr operator -(IntPtr pointer, int offset)
	{
		throw null;
	}

	public static IntPtr Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static IntPtr Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static IntPtr Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static IntPtr Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static IntPtr Parse(string s)
	{
		throw null;
	}

	public static IntPtr Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static IntPtr Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static IntPtr Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static IntPtr PopCount(IntPtr value)
	{
		throw null;
	}

	public static IntPtr RotateLeft(IntPtr value, int rotateAmount)
	{
		throw null;
	}

	public static IntPtr RotateRight(IntPtr value, int rotateAmount)
	{
		throw null;
	}

	public static int Sign(IntPtr value)
	{
		throw null;
	}

	public static IntPtr Subtract(IntPtr pointer, int offset)
	{
		throw null;
	}

	static IntPtr IAdditionOperators<IntPtr, IntPtr, IntPtr>.operator +(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static IntPtr IAdditionOperators<IntPtr, IntPtr, IntPtr>.operator checked +(IntPtr left, IntPtr right)
	{
		throw null;
	}

	int IBinaryInteger<IntPtr>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<IntPtr>.GetShortestBitLength()
	{
		throw null;
	}

	static bool IBinaryInteger<IntPtr>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out IntPtr value)
	{
		throw null;
	}

	static bool IBinaryInteger<IntPtr>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out IntPtr value)
	{
		throw null;
	}

	bool IBinaryInteger<IntPtr>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<IntPtr>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static IntPtr IBitwiseOperators<IntPtr, IntPtr, IntPtr>.operator &(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static IntPtr IBitwiseOperators<IntPtr, IntPtr, IntPtr>.operator |(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static IntPtr IBitwiseOperators<IntPtr, IntPtr, IntPtr>.operator ^(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static IntPtr IBitwiseOperators<IntPtr, IntPtr, IntPtr>.operator ~(IntPtr value)
	{
		throw null;
	}

	static bool IComparisonOperators<IntPtr, IntPtr, bool>.operator >(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static bool IComparisonOperators<IntPtr, IntPtr, bool>.operator >=(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static bool IComparisonOperators<IntPtr, IntPtr, bool>.operator <(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static bool IComparisonOperators<IntPtr, IntPtr, bool>.operator <=(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static IntPtr IDecrementOperators<IntPtr>.operator checked --(IntPtr value)
	{
		throw null;
	}

	static IntPtr IDecrementOperators<IntPtr>.operator --(IntPtr value)
	{
		throw null;
	}

	static IntPtr IDivisionOperators<IntPtr, IntPtr, IntPtr>.operator /(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static IntPtr IIncrementOperators<IntPtr>.operator checked ++(IntPtr value)
	{
		throw null;
	}

	static IntPtr IIncrementOperators<IntPtr>.operator ++(IntPtr value)
	{
		throw null;
	}

	static IntPtr IModulusOperators<IntPtr, IntPtr, IntPtr>.operator %(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static IntPtr IMultiplyOperators<IntPtr, IntPtr, IntPtr>.operator checked *(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static IntPtr IMultiplyOperators<IntPtr, IntPtr, IntPtr>.operator *(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsCanonical(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsComplexNumber(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsFinite(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsImaginaryNumber(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsInfinity(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsInteger(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsNaN(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsNegativeInfinity(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsNormal(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsPositiveInfinity(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsRealNumber(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsSubnormal(IntPtr value)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.IsZero(IntPtr value)
	{
		throw null;
	}

	static IntPtr INumberBase<IntPtr>.MaxMagnitudeNumber(IntPtr x, IntPtr y)
	{
		throw null;
	}

	static IntPtr INumberBase<IntPtr>.MinMagnitudeNumber(IntPtr x, IntPtr y)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.TryConvertFromChecked<TOther>(TOther value, out IntPtr result)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.TryConvertFromSaturating<TOther>(TOther value, out IntPtr result)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.TryConvertFromTruncating<TOther>(TOther value, out IntPtr result)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.TryConvertToChecked<TOther>(IntPtr value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.TryConvertToSaturating<TOther>(IntPtr value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<IntPtr>.TryConvertToTruncating<TOther>(IntPtr value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static IntPtr INumber<IntPtr>.MaxNumber(IntPtr x, IntPtr y)
	{
		throw null;
	}

	static IntPtr INumber<IntPtr>.MinNumber(IntPtr x, IntPtr y)
	{
		throw null;
	}

	static IntPtr IShiftOperators<IntPtr, int, IntPtr>.operator <<(IntPtr value, int shiftAmount)
	{
		throw null;
	}

	static IntPtr IShiftOperators<IntPtr, int, IntPtr>.operator >>(IntPtr value, int shiftAmount)
	{
		throw null;
	}

	static IntPtr IShiftOperators<IntPtr, int, IntPtr>.operator >>>(IntPtr value, int shiftAmount)
	{
		throw null;
	}

	static IntPtr ISubtractionOperators<IntPtr, IntPtr, IntPtr>.operator checked -(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static IntPtr ISubtractionOperators<IntPtr, IntPtr, IntPtr>.operator -(IntPtr left, IntPtr right)
	{
		throw null;
	}

	static IntPtr IUnaryNegationOperators<IntPtr, IntPtr>.operator checked -(IntPtr value)
	{
		throw null;
	}

	static IntPtr IUnaryNegationOperators<IntPtr, IntPtr>.operator -(IntPtr value)
	{
		throw null;
	}

	static IntPtr IUnaryPlusOperators<IntPtr, IntPtr>.operator +(IntPtr value)
	{
		throw null;
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public int ToInt32()
	{
		throw null;
	}

	public long ToInt64()
	{
		throw null;
	}

	[CLSCompliant(false)]
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

	public static IntPtr TrailingZeroCount(IntPtr value)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out IntPtr result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out IntPtr result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out IntPtr result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out IntPtr result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out IntPtr result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out IntPtr result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out IntPtr result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out IntPtr result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out IntPtr result)
	{
		throw null;
	}
}
