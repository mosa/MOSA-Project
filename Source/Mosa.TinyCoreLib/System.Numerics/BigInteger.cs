using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Numerics;

public readonly struct BigInteger : IComparable, IComparable<BigInteger>, IEquatable<BigInteger>, IFormattable, IParsable<BigInteger>, ISpanFormattable, ISpanParsable<BigInteger>, IAdditionOperators<BigInteger, BigInteger, BigInteger>, IAdditiveIdentity<BigInteger, BigInteger>, IBinaryInteger<BigInteger>, IBinaryNumber<BigInteger>, IBitwiseOperators<BigInteger, BigInteger, BigInteger>, IComparisonOperators<BigInteger, BigInteger, bool>, IEqualityOperators<BigInteger, BigInteger, bool>, IDecrementOperators<BigInteger>, IDivisionOperators<BigInteger, BigInteger, BigInteger>, IIncrementOperators<BigInteger>, IModulusOperators<BigInteger, BigInteger, BigInteger>, IMultiplicativeIdentity<BigInteger, BigInteger>, IMultiplyOperators<BigInteger, BigInteger, BigInteger>, INumber<BigInteger>, INumberBase<BigInteger>, ISubtractionOperators<BigInteger, BigInteger, BigInteger>, IUnaryNegationOperators<BigInteger, BigInteger>, IUnaryPlusOperators<BigInteger, BigInteger>, IUtf8SpanFormattable, IUtf8SpanParsable<BigInteger>, IShiftOperators<BigInteger, int, BigInteger>, ISignedNumber<BigInteger>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public bool IsEven
	{
		get
		{
			throw null;
		}
	}

	public bool IsOne
	{
		get
		{
			throw null;
		}
	}

	public bool IsPowerOfTwo
	{
		get
		{
			throw null;
		}
	}

	public bool IsZero
	{
		get
		{
			throw null;
		}
	}

	public static BigInteger MinusOne
	{
		get
		{
			throw null;
		}
	}

	public static BigInteger One
	{
		get
		{
			throw null;
		}
	}

	public int Sign
	{
		get
		{
			throw null;
		}
	}

	static BigInteger IAdditiveIdentity<BigInteger, BigInteger>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static BigInteger IBinaryNumber<BigInteger>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static BigInteger IMultiplicativeIdentity<BigInteger, BigInteger>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<BigInteger>.Radix
	{
		get
		{
			throw null;
		}
	}

	static BigInteger ISignedNumber<BigInteger>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public static BigInteger Zero
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public BigInteger(byte[] value)
	{
		throw null;
	}

	public BigInteger(decimal value)
	{
		throw null;
	}

	public BigInteger(double value)
	{
		throw null;
	}

	public BigInteger(int value)
	{
		throw null;
	}

	public BigInteger(long value)
	{
		throw null;
	}

	public BigInteger(ReadOnlySpan<byte> value, bool isUnsigned = false, bool isBigEndian = false)
	{
		throw null;
	}

	public BigInteger(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public BigInteger(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public BigInteger(ulong value)
	{
		throw null;
	}

	public static BigInteger Abs(BigInteger value)
	{
		throw null;
	}

	public static BigInteger Add(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger Clamp(BigInteger value, BigInteger min, BigInteger max)
	{
		throw null;
	}

	public static int Compare(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public int CompareTo(long other)
	{
		throw null;
	}

	public int CompareTo(BigInteger other)
	{
		throw null;
	}

	public int CompareTo(object? obj)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public int CompareTo(ulong other)
	{
		throw null;
	}

	public static BigInteger CopySign(BigInteger value, BigInteger sign)
	{
		throw null;
	}

	public static BigInteger CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static BigInteger CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static BigInteger CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static BigInteger Divide(BigInteger dividend, BigInteger divisor)
	{
		throw null;
	}

	public static (BigInteger Quotient, BigInteger Remainder) DivRem(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger DivRem(BigInteger dividend, BigInteger divisor, out BigInteger remainder)
	{
		throw null;
	}

	public bool Equals(long other)
	{
		throw null;
	}

	public bool Equals(BigInteger other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public bool Equals(ulong other)
	{
		throw null;
	}

	public long GetBitLength()
	{
		throw null;
	}

	public int GetByteCount(bool isUnsigned = false)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static BigInteger GreatestCommonDivisor(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static bool IsEvenInteger(BigInteger value)
	{
		throw null;
	}

	public static bool IsNegative(BigInteger value)
	{
		throw null;
	}

	public static bool IsOddInteger(BigInteger value)
	{
		throw null;
	}

	public static bool IsPositive(BigInteger value)
	{
		throw null;
	}

	public static bool IsPow2(BigInteger value)
	{
		throw null;
	}

	public static BigInteger LeadingZeroCount(BigInteger value)
	{
		throw null;
	}

	public static double Log(BigInteger value)
	{
		throw null;
	}

	public static double Log(BigInteger value, double baseValue)
	{
		throw null;
	}

	public static double Log10(BigInteger value)
	{
		throw null;
	}

	public static BigInteger Log2(BigInteger value)
	{
		throw null;
	}

	public static BigInteger Max(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger MaxMagnitude(BigInteger x, BigInteger y)
	{
		throw null;
	}

	public static BigInteger Min(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger MinMagnitude(BigInteger x, BigInteger y)
	{
		throw null;
	}

	public static BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus)
	{
		throw null;
	}

	public static BigInteger Multiply(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger Negate(BigInteger value)
	{
		throw null;
	}

	public static BigInteger operator +(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger operator &(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger operator |(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger operator --(BigInteger value)
	{
		throw null;
	}

	public static BigInteger operator /(BigInteger dividend, BigInteger divisor)
	{
		throw null;
	}

	public static bool operator ==(long left, BigInteger right)
	{
		throw null;
	}

	public static bool operator ==(BigInteger left, long right)
	{
		throw null;
	}

	public static bool operator ==(BigInteger left, BigInteger right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator ==(BigInteger left, ulong right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator ==(ulong left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger operator ^(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static explicit operator BigInteger(decimal value)
	{
		throw null;
	}

	public static explicit operator BigInteger(double value)
	{
		throw null;
	}

	public static explicit operator BigInteger(Half value)
	{
		throw null;
	}

	public static explicit operator byte(BigInteger value)
	{
		throw null;
	}

	public static explicit operator char(BigInteger value)
	{
		throw null;
	}

	public static explicit operator decimal(BigInteger value)
	{
		throw null;
	}

	public static explicit operator double(BigInteger value)
	{
		throw null;
	}

	public static explicit operator Half(BigInteger value)
	{
		throw null;
	}

	public static explicit operator Int128(BigInteger value)
	{
		throw null;
	}

	public static explicit operator short(BigInteger value)
	{
		throw null;
	}

	public static explicit operator int(BigInteger value)
	{
		throw null;
	}

	public static explicit operator long(BigInteger value)
	{
		throw null;
	}

	public static explicit operator IntPtr(BigInteger value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator sbyte(BigInteger value)
	{
		throw null;
	}

	public static explicit operator float(BigInteger value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator UInt128(BigInteger value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ushort(BigInteger value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator uint(BigInteger value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ulong(BigInteger value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator UIntPtr(BigInteger value)
	{
		throw null;
	}

	public static explicit operator BigInteger(Complex value)
	{
		throw null;
	}

	public static explicit operator BigInteger(float value)
	{
		throw null;
	}

	public static bool operator >(long left, BigInteger right)
	{
		throw null;
	}

	public static bool operator >(BigInteger left, long right)
	{
		throw null;
	}

	public static bool operator >(BigInteger left, BigInteger right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator >(BigInteger left, ulong right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator >(ulong left, BigInteger right)
	{
		throw null;
	}

	public static bool operator >=(long left, BigInteger right)
	{
		throw null;
	}

	public static bool operator >=(BigInteger left, long right)
	{
		throw null;
	}

	public static bool operator >=(BigInteger left, BigInteger right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator >=(BigInteger left, ulong right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator >=(ulong left, BigInteger right)
	{
		throw null;
	}

	public static implicit operator BigInteger(byte value)
	{
		throw null;
	}

	public static implicit operator BigInteger(char value)
	{
		throw null;
	}

	public static implicit operator BigInteger(Int128 value)
	{
		throw null;
	}

	public static implicit operator BigInteger(short value)
	{
		throw null;
	}

	public static implicit operator BigInteger(int value)
	{
		throw null;
	}

	public static implicit operator BigInteger(long value)
	{
		throw null;
	}

	public static implicit operator BigInteger(IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator BigInteger(sbyte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator BigInteger(UInt128 value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator BigInteger(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator BigInteger(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator BigInteger(ulong value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator BigInteger(UIntPtr value)
	{
		throw null;
	}

	public static BigInteger operator ++(BigInteger value)
	{
		throw null;
	}

	public static bool operator !=(long left, BigInteger right)
	{
		throw null;
	}

	public static bool operator !=(BigInteger left, long right)
	{
		throw null;
	}

	public static bool operator !=(BigInteger left, BigInteger right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator !=(BigInteger left, ulong right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator !=(ulong left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger operator <<(BigInteger value, int shift)
	{
		throw null;
	}

	public static bool operator <(long left, BigInteger right)
	{
		throw null;
	}

	public static bool operator <(BigInteger left, long right)
	{
		throw null;
	}

	public static bool operator <(BigInteger left, BigInteger right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator <(BigInteger left, ulong right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator <(ulong left, BigInteger right)
	{
		throw null;
	}

	public static bool operator <=(long left, BigInteger right)
	{
		throw null;
	}

	public static bool operator <=(BigInteger left, long right)
	{
		throw null;
	}

	public static bool operator <=(BigInteger left, BigInteger right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator <=(BigInteger left, ulong right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool operator <=(ulong left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger operator %(BigInteger dividend, BigInteger divisor)
	{
		throw null;
	}

	public static BigInteger operator *(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger operator ~(BigInteger value)
	{
		throw null;
	}

	public static BigInteger operator >>(BigInteger value, int shift)
	{
		throw null;
	}

	public static BigInteger operator -(BigInteger left, BigInteger right)
	{
		throw null;
	}

	public static BigInteger operator -(BigInteger value)
	{
		throw null;
	}

	public static BigInteger operator +(BigInteger value)
	{
		throw null;
	}

	public static BigInteger operator >>>(BigInteger value, int shiftAmount)
	{
		throw null;
	}

	public static BigInteger Parse(ReadOnlySpan<char> value, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static BigInteger Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static BigInteger Parse(string value)
	{
		throw null;
	}

	public static BigInteger Parse(string value, NumberStyles style)
	{
		throw null;
	}

	public static BigInteger Parse(string value, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static BigInteger Parse(string value, IFormatProvider? provider)
	{
		throw null;
	}

	public static BigInteger PopCount(BigInteger value)
	{
		throw null;
	}

	public static BigInteger Pow(BigInteger value, int exponent)
	{
		throw null;
	}

	public static BigInteger Remainder(BigInteger dividend, BigInteger divisor)
	{
		throw null;
	}

	public static BigInteger RotateLeft(BigInteger value, int rotateAmount)
	{
		throw null;
	}

	public static BigInteger RotateRight(BigInteger value, int rotateAmount)
	{
		throw null;
	}

	public static BigInteger Subtract(BigInteger left, BigInteger right)
	{
		throw null;
	}

	static bool IBinaryInteger<BigInteger>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out BigInteger value)
	{
		throw null;
	}

	static bool IBinaryInteger<BigInteger>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out BigInteger value)
	{
		throw null;
	}

	int IBinaryInteger<BigInteger>.GetByteCount()
	{
		throw null;
	}

	int IBinaryInteger<BigInteger>.GetShortestBitLength()
	{
		throw null;
	}

	bool IBinaryInteger<BigInteger>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IBinaryInteger<BigInteger>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsCanonical(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsComplexNumber(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsFinite(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsImaginaryNumber(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsInfinity(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsInteger(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsNaN(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsNegativeInfinity(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsNormal(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsPositiveInfinity(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsRealNumber(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsSubnormal(BigInteger value)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.IsZero(BigInteger value)
	{
		throw null;
	}

	static BigInteger INumberBase<BigInteger>.MaxMagnitudeNumber(BigInteger x, BigInteger y)
	{
		throw null;
	}

	static BigInteger INumberBase<BigInteger>.MinMagnitudeNumber(BigInteger x, BigInteger y)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.TryConvertFromChecked<TOther>(TOther value, out BigInteger result)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.TryConvertFromSaturating<TOther>(TOther value, out BigInteger result)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.TryConvertFromTruncating<TOther>(TOther value, out BigInteger result)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.TryConvertToChecked<TOther>(BigInteger value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.TryConvertToSaturating<TOther>(BigInteger value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<BigInteger>.TryConvertToTruncating<TOther>(BigInteger value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static BigInteger INumber<BigInteger>.MaxNumber(BigInteger x, BigInteger y)
	{
		throw null;
	}

	static BigInteger INumber<BigInteger>.MinNumber(BigInteger x, BigInteger y)
	{
		throw null;
	}

	static int INumber<BigInteger>.Sign(BigInteger value)
	{
		throw null;
	}

	public byte[] ToByteArray()
	{
		throw null;
	}

	public byte[] ToByteArray(bool isUnsigned = false, bool isBigEndian = false)
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

	public static BigInteger TrailingZeroCount(BigInteger value)
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax("NumericFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> value, NumberStyles style, IFormatProvider? provider, out BigInteger result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BigInteger result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> value, out BigInteger result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? value, NumberStyles style, IFormatProvider? provider, out BigInteger result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out BigInteger result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? value, out BigInteger result)
	{
		throw null;
	}

	public bool TryWriteBytes(Span<byte> destination, out int bytesWritten, bool isUnsigned = false, bool isBigEndian = false)
	{
		throw null;
	}
}
