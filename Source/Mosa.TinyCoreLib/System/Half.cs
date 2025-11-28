using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

public readonly struct Half : IComparable, IComparable<Half>, IEquatable<Half>, IFormattable, IParsable<Half>, ISpanFormattable, ISpanParsable<Half>, IAdditionOperators<Half, Half, Half>, IAdditiveIdentity<Half, Half>, IBinaryFloatingPointIeee754<Half>, IBinaryNumber<Half>, IBitwiseOperators<Half, Half, Half>, IComparisonOperators<Half, Half, bool>, IEqualityOperators<Half, Half, bool>, IDecrementOperators<Half>, IDivisionOperators<Half, Half, Half>, IIncrementOperators<Half>, IModulusOperators<Half, Half, Half>, IMultiplicativeIdentity<Half, Half>, IMultiplyOperators<Half, Half, Half>, INumber<Half>, INumberBase<Half>, ISubtractionOperators<Half, Half, Half>, IUnaryNegationOperators<Half, Half>, IUnaryPlusOperators<Half, Half>, IUtf8SpanFormattable, IUtf8SpanParsable<Half>, IExponentialFunctions<Half>, IFloatingPointConstants<Half>, IFloatingPoint<Half>, ISignedNumber<Half>, IFloatingPointIeee754<Half>, IHyperbolicFunctions<Half>, ILogarithmicFunctions<Half>, IPowerFunctions<Half>, IRootFunctions<Half>, ITrigonometricFunctions<Half>, IMinMaxValue<Half>
{
	private readonly int _dummyPrimitive;

	public static Half E
	{
		get
		{
			throw null;
		}
	}

	public static Half Epsilon
	{
		get
		{
			throw null;
		}
	}

	public static Half MaxValue
	{
		get
		{
			throw null;
		}
	}

	public static Half MinValue
	{
		get
		{
			throw null;
		}
	}

	public static Half MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	public static Half NaN
	{
		get
		{
			throw null;
		}
	}

	public static Half NegativeInfinity
	{
		get
		{
			throw null;
		}
	}

	public static Half NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public static Half NegativeZero
	{
		get
		{
			throw null;
		}
	}

	public static Half One
	{
		get
		{
			throw null;
		}
	}

	public static Half Pi
	{
		get
		{
			throw null;
		}
	}

	public static Half PositiveInfinity
	{
		get
		{
			throw null;
		}
	}

	static Half IAdditiveIdentity<Half, Half>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static Half IBinaryNumber<Half>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<Half>.Radix
	{
		get
		{
			throw null;
		}
	}

	public static Half Tau
	{
		get
		{
			throw null;
		}
	}

	public static Half Zero
	{
		get
		{
			throw null;
		}
	}

	public static Half Abs(Half value)
	{
		throw null;
	}

	public static Half Acos(Half x)
	{
		throw null;
	}

	public static Half Acosh(Half x)
	{
		throw null;
	}

	public static Half AcosPi(Half x)
	{
		throw null;
	}

	public static Half Asin(Half x)
	{
		throw null;
	}

	public static Half Asinh(Half x)
	{
		throw null;
	}

	public static Half AsinPi(Half x)
	{
		throw null;
	}

	public static Half Atan(Half x)
	{
		throw null;
	}

	public static Half Atan2(Half y, Half x)
	{
		throw null;
	}

	public static Half Atan2Pi(Half y, Half x)
	{
		throw null;
	}

	public static Half Atanh(Half x)
	{
		throw null;
	}

	public static Half AtanPi(Half x)
	{
		throw null;
	}

	public static Half BitDecrement(Half x)
	{
		throw null;
	}

	public static Half BitIncrement(Half x)
	{
		throw null;
	}

	public static Half Cbrt(Half x)
	{
		throw null;
	}

	public static Half Ceiling(Half x)
	{
		throw null;
	}

	public static Half Clamp(Half value, Half min, Half max)
	{
		throw null;
	}

	public int CompareTo(Half other)
	{
		throw null;
	}

	public int CompareTo(object? obj)
	{
		throw null;
	}

	public static Half CopySign(Half value, Half sign)
	{
		throw null;
	}

	public static Half Cos(Half x)
	{
		throw null;
	}

	public static Half Cosh(Half x)
	{
		throw null;
	}

	public static Half CosPi(Half x)
	{
		throw null;
	}

	public static Half CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static Half CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static Half CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static Half DegreesToRadians(Half degrees)
	{
		throw null;
	}

	public bool Equals(Half other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static Half Exp(Half x)
	{
		throw null;
	}

	public static Half Exp10(Half x)
	{
		throw null;
	}

	public static Half Exp10M1(Half x)
	{
		throw null;
	}

	public static Half Exp2(Half x)
	{
		throw null;
	}

	public static Half Exp2M1(Half x)
	{
		throw null;
	}

	public static Half ExpM1(Half x)
	{
		throw null;
	}

	public static Half Floor(Half x)
	{
		throw null;
	}

	public static Half FusedMultiplyAdd(Half left, Half right, Half addend)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static Half Hypot(Half x, Half y)
	{
		throw null;
	}

	public static Half Ieee754Remainder(Half left, Half right)
	{
		throw null;
	}

	public static int ILogB(Half x)
	{
		throw null;
	}

	public static bool IsEvenInteger(Half value)
	{
		throw null;
	}

	public static bool IsFinite(Half value)
	{
		throw null;
	}

	public static bool IsInfinity(Half value)
	{
		throw null;
	}

	public static bool IsInteger(Half value)
	{
		throw null;
	}

	public static bool IsNaN(Half value)
	{
		throw null;
	}

	public static bool IsNegative(Half value)
	{
		throw null;
	}

	public static bool IsNegativeInfinity(Half value)
	{
		throw null;
	}

	public static bool IsNormal(Half value)
	{
		throw null;
	}

	public static bool IsOddInteger(Half value)
	{
		throw null;
	}

	public static bool IsPositive(Half value)
	{
		throw null;
	}

	public static bool IsPositiveInfinity(Half value)
	{
		throw null;
	}

	public static bool IsPow2(Half value)
	{
		throw null;
	}

	public static bool IsRealNumber(Half value)
	{
		throw null;
	}

	public static bool IsSubnormal(Half value)
	{
		throw null;
	}

	public static Half Lerp(Half value1, Half value2, Half amount)
	{
		throw null;
	}

	public static Half Log(Half x)
	{
		throw null;
	}

	public static Half Log(Half x, Half newBase)
	{
		throw null;
	}

	public static Half Log10(Half x)
	{
		throw null;
	}

	public static Half Log10P1(Half x)
	{
		throw null;
	}

	public static Half Log2(Half value)
	{
		throw null;
	}

	public static Half Log2P1(Half x)
	{
		throw null;
	}

	public static Half LogP1(Half x)
	{
		throw null;
	}

	public static Half Max(Half x, Half y)
	{
		throw null;
	}

	public static Half MaxMagnitude(Half x, Half y)
	{
		throw null;
	}

	public static Half MaxMagnitudeNumber(Half x, Half y)
	{
		throw null;
	}

	public static Half MaxNumber(Half x, Half y)
	{
		throw null;
	}

	public static Half Min(Half x, Half y)
	{
		throw null;
	}

	public static Half MinMagnitude(Half x, Half y)
	{
		throw null;
	}

	public static Half MinMagnitudeNumber(Half x, Half y)
	{
		throw null;
	}

	public static Half MinNumber(Half x, Half y)
	{
		throw null;
	}

	public static Half operator +(Half left, Half right)
	{
		throw null;
	}

	public static explicit operator checked byte(Half value)
	{
		throw null;
	}

	public static explicit operator checked char(Half value)
	{
		throw null;
	}

	public static explicit operator checked short(Half value)
	{
		throw null;
	}

	public static explicit operator checked int(Half value)
	{
		throw null;
	}

	public static explicit operator checked long(Half value)
	{
		throw null;
	}

	public static explicit operator checked Int128(Half value)
	{
		throw null;
	}

	public static explicit operator checked IntPtr(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked sbyte(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked ushort(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked uint(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked ulong(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked UInt128(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked UIntPtr(Half value)
	{
		throw null;
	}

	public static Half operator --(Half value)
	{
		throw null;
	}

	public static Half operator /(Half left, Half right)
	{
		throw null;
	}

	public static bool operator ==(Half left, Half right)
	{
		throw null;
	}

	public static explicit operator Half(char value)
	{
		throw null;
	}

	public static explicit operator Half(decimal value)
	{
		throw null;
	}

	public static explicit operator Half(double value)
	{
		throw null;
	}

	public static explicit operator byte(Half value)
	{
		throw null;
	}

	public static explicit operator char(Half value)
	{
		throw null;
	}

	public static explicit operator decimal(Half value)
	{
		throw null;
	}

	public static explicit operator double(Half value)
	{
		throw null;
	}

	public static explicit operator Int128(Half value)
	{
		throw null;
	}

	public static explicit operator short(Half value)
	{
		throw null;
	}

	public static explicit operator int(Half value)
	{
		throw null;
	}

	public static explicit operator long(Half value)
	{
		throw null;
	}

	public static explicit operator IntPtr(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator sbyte(Half value)
	{
		throw null;
	}

	public static explicit operator float(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator UInt128(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ushort(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator uint(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ulong(Half value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator UIntPtr(Half value)
	{
		throw null;
	}

	public static explicit operator Half(short value)
	{
		throw null;
	}

	public static explicit operator Half(int value)
	{
		throw null;
	}

	public static explicit operator Half(long value)
	{
		throw null;
	}

	public static explicit operator Half(IntPtr value)
	{
		throw null;
	}

	public static explicit operator Half(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Half(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Half(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Half(ulong value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Half(UIntPtr value)
	{
		throw null;
	}

	public static bool operator >(Half left, Half right)
	{
		throw null;
	}

	public static bool operator >=(Half left, Half right)
	{
		throw null;
	}

	public static implicit operator Half(byte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Half(sbyte value)
	{
		throw null;
	}

	public static Half operator ++(Half value)
	{
		throw null;
	}

	public static bool operator !=(Half left, Half right)
	{
		throw null;
	}

	public static bool operator <(Half left, Half right)
	{
		throw null;
	}

	public static bool operator <=(Half left, Half right)
	{
		throw null;
	}

	public static Half operator %(Half left, Half right)
	{
		throw null;
	}

	public static Half operator *(Half left, Half right)
	{
		throw null;
	}

	public static Half operator -(Half left, Half right)
	{
		throw null;
	}

	public static Half operator -(Half value)
	{
		throw null;
	}

	public static Half operator +(Half value)
	{
		throw null;
	}

	public static Half Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static Half Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static Half Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static Half Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static Half Parse(string s)
	{
		throw null;
	}

	public static Half Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static Half Parse(string s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static Half Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static Half Pow(Half x, Half y)
	{
		throw null;
	}

	public static Half RadiansToDegrees(Half radians)
	{
		throw null;
	}

	public static Half ReciprocalEstimate(Half x)
	{
		throw null;
	}

	public static Half ReciprocalSqrtEstimate(Half x)
	{
		throw null;
	}

	public static Half RootN(Half x, int n)
	{
		throw null;
	}

	public static Half Round(Half x)
	{
		throw null;
	}

	public static Half Round(Half x, int digits)
	{
		throw null;
	}

	public static Half Round(Half x, int digits, MidpointRounding mode)
	{
		throw null;
	}

	public static Half Round(Half x, MidpointRounding mode)
	{
		throw null;
	}

	public static Half ScaleB(Half x, int n)
	{
		throw null;
	}

	public static int Sign(Half value)
	{
		throw null;
	}

	public static Half Sin(Half x)
	{
		throw null;
	}

	public static (Half Sin, Half Cos) SinCos(Half x)
	{
		throw null;
	}

	public static (Half SinPi, Half CosPi) SinCosPi(Half x)
	{
		throw null;
	}

	public static Half Sinh(Half x)
	{
		throw null;
	}

	public static Half SinPi(Half x)
	{
		throw null;
	}

	public static Half Sqrt(Half x)
	{
		throw null;
	}

	static Half IBitwiseOperators<Half, Half, Half>.operator &(Half left, Half right)
	{
		throw null;
	}

	static Half IBitwiseOperators<Half, Half, Half>.operator |(Half left, Half right)
	{
		throw null;
	}

	static Half IBitwiseOperators<Half, Half, Half>.operator ^(Half left, Half right)
	{
		throw null;
	}

	static Half IBitwiseOperators<Half, Half, Half>.operator ~(Half value)
	{
		throw null;
	}

	int IFloatingPoint<Half>.GetExponentByteCount()
	{
		throw null;
	}

	int IFloatingPoint<Half>.GetExponentShortestBitLength()
	{
		throw null;
	}

	int IFloatingPoint<Half>.GetSignificandBitLength()
	{
		throw null;
	}

	int IFloatingPoint<Half>.GetSignificandByteCount()
	{
		throw null;
	}

	bool IFloatingPoint<Half>.TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<Half>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<Half>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<Half>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static bool INumberBase<Half>.IsCanonical(Half value)
	{
		throw null;
	}

	static bool INumberBase<Half>.IsComplexNumber(Half value)
	{
		throw null;
	}

	static bool INumberBase<Half>.IsImaginaryNumber(Half value)
	{
		throw null;
	}

	static bool INumberBase<Half>.IsZero(Half value)
	{
		throw null;
	}

	static bool INumberBase<Half>.TryConvertFromChecked<TOther>(TOther value, out Half result)
	{
		throw null;
	}

	static bool INumberBase<Half>.TryConvertFromSaturating<TOther>(TOther value, out Half result)
	{
		throw null;
	}

	static bool INumberBase<Half>.TryConvertFromTruncating<TOther>(TOther value, out Half result)
	{
		throw null;
	}

	static bool INumberBase<Half>.TryConvertToChecked<TOther>(Half value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<Half>.TryConvertToSaturating<TOther>(Half value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<Half>.TryConvertToTruncating<TOther>(Half value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	public static Half Tan(Half x)
	{
		throw null;
	}

	public static Half Tanh(Half x)
	{
		throw null;
	}

	public static Half TanPi(Half x)
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

	public static Half Truncate(Half x)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out Half result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out Half result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Half result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Half result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out Half result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Half result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Half result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out Half result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Half result)
	{
		throw null;
	}
}
