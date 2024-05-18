using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

public readonly struct Single : IComparable, IComparable<float>, IConvertible, IEquatable<float>, IFormattable, IParsable<float>, ISpanFormattable, ISpanParsable<float>, IAdditionOperators<float, float, float>, IAdditiveIdentity<float, float>, IBinaryFloatingPointIeee754<float>, IBinaryNumber<float>, IBitwiseOperators<float, float, float>, IComparisonOperators<float, float, bool>, IEqualityOperators<float, float, bool>, IDecrementOperators<float>, IDivisionOperators<float, float, float>, IIncrementOperators<float>, IModulusOperators<float, float, float>, IMultiplicativeIdentity<float, float>, IMultiplyOperators<float, float, float>, INumber<float>, INumberBase<float>, ISubtractionOperators<float, float, float>, IUnaryNegationOperators<float, float>, IUnaryPlusOperators<float, float>, IUtf8SpanFormattable, IUtf8SpanParsable<float>, IExponentialFunctions<float>, IFloatingPointConstants<float>, IFloatingPoint<float>, ISignedNumber<float>, IFloatingPointIeee754<float>, IHyperbolicFunctions<float>, ILogarithmicFunctions<float>, IPowerFunctions<float>, IRootFunctions<float>, ITrigonometricFunctions<float>, IMinMaxValue<float>
{
	private readonly float _dummyPrimitive;

	public const float E = 2.7182817f;

	public const float Epsilon = 1E-45f;

	public const float MaxValue = 3.4028235E+38f;

	public const float MinValue = -3.4028235E+38f;

	public const float NaN = 0f / 0f;

	public const float NegativeInfinity = -1f / 0f;

	public const float NegativeZero = -0f;

	public const float Pi = 3.1415927f;

	public const float PositiveInfinity = 1f / 0f;

	public const float Tau = 6.2831855f;

	static float IAdditiveIdentity<float, float>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static float IBinaryNumber<float>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static float IFloatingPointConstants<float>.E
	{
		get
		{
			throw null;
		}
	}

	static float IFloatingPointConstants<float>.Pi
	{
		get
		{
			throw null;
		}
	}

	static float IFloatingPointConstants<float>.Tau
	{
		get
		{
			throw null;
		}
	}

	static float IFloatingPointIeee754<float>.Epsilon
	{
		get
		{
			throw null;
		}
	}

	static float IFloatingPointIeee754<float>.NaN
	{
		get
		{
			throw null;
		}
	}

	static float IFloatingPointIeee754<float>.NegativeInfinity
	{
		get
		{
			throw null;
		}
	}

	static float IFloatingPointIeee754<float>.NegativeZero
	{
		get
		{
			throw null;
		}
	}

	static float IFloatingPointIeee754<float>.PositiveInfinity
	{
		get
		{
			throw null;
		}
	}

	static float IMinMaxValue<float>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static float IMinMaxValue<float>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static float IMultiplicativeIdentity<float, float>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static float INumberBase<float>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<float>.Radix
	{
		get
		{
			throw null;
		}
	}

	static float INumberBase<float>.Zero
	{
		get
		{
			throw null;
		}
	}

	static float ISignedNumber<float>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public static float Abs(float value)
	{
		throw null;
	}

	public static float Acos(float x)
	{
		throw null;
	}

	public static float Acosh(float x)
	{
		throw null;
	}

	public static float AcosPi(float x)
	{
		throw null;
	}

	public static float Asin(float x)
	{
		throw null;
	}

	public static float Asinh(float x)
	{
		throw null;
	}

	public static float AsinPi(float x)
	{
		throw null;
	}

	public static float Atan(float x)
	{
		throw null;
	}

	public static float Atan2(float y, float x)
	{
		throw null;
	}

	public static float Atan2Pi(float y, float x)
	{
		throw null;
	}

	public static float Atanh(float x)
	{
		throw null;
	}

	public static float AtanPi(float x)
	{
		throw null;
	}

	public static float BitDecrement(float x)
	{
		throw null;
	}

	public static float BitIncrement(float x)
	{
		throw null;
	}

	public static float Cbrt(float x)
	{
		throw null;
	}

	public static float Ceiling(float x)
	{
		throw null;
	}

	public static float Clamp(float value, float min, float max)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public int CompareTo(float value)
	{
		throw null;
	}

	public static float CopySign(float value, float sign)
	{
		throw null;
	}

	public static float Cos(float x)
	{
		throw null;
	}

	public static float Cosh(float x)
	{
		throw null;
	}

	public static float CosPi(float x)
	{
		throw null;
	}

	public static float CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static float CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static float CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static float DegreesToRadians(float degrees)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(float obj)
	{
		throw null;
	}

	public static float Exp(float x)
	{
		throw null;
	}

	public static float Exp10(float x)
	{
		throw null;
	}

	public static float Exp10M1(float x)
	{
		throw null;
	}

	public static float Exp2(float x)
	{
		throw null;
	}

	public static float Exp2M1(float x)
	{
		throw null;
	}

	public static float ExpM1(float x)
	{
		throw null;
	}

	public static float Floor(float x)
	{
		throw null;
	}

	public static float FusedMultiplyAdd(float left, float right, float addend)
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

	public static float Hypot(float x, float y)
	{
		throw null;
	}

	public static float Ieee754Remainder(float left, float right)
	{
		throw null;
	}

	public static int ILogB(float x)
	{
		throw null;
	}

	public static bool IsEvenInteger(float value)
	{
		throw null;
	}

	public static bool IsFinite(float f)
	{
		throw null;
	}

	public static bool IsInfinity(float f)
	{
		throw null;
	}

	public static bool IsInteger(float value)
	{
		throw null;
	}

	public static bool IsNaN(float f)
	{
		throw null;
	}

	public static bool IsNegative(float f)
	{
		throw null;
	}

	public static bool IsNegativeInfinity(float f)
	{
		throw null;
	}

	public static bool IsNormal(float f)
	{
		throw null;
	}

	public static bool IsOddInteger(float value)
	{
		throw null;
	}

	public static bool IsPositive(float value)
	{
		throw null;
	}

	public static bool IsPositiveInfinity(float f)
	{
		throw null;
	}

	public static bool IsPow2(float value)
	{
		throw null;
	}

	public static bool IsRealNumber(float value)
	{
		throw null;
	}

	public static bool IsSubnormal(float f)
	{
		throw null;
	}

	public static float Lerp(float value1, float value2, float amount)
	{
		throw null;
	}

	public static float Log(float x)
	{
		throw null;
	}

	public static float Log(float x, float newBase)
	{
		throw null;
	}

	public static float Log10(float x)
	{
		throw null;
	}

	public static float Log10P1(float x)
	{
		throw null;
	}

	public static float Log2(float value)
	{
		throw null;
	}

	public static float Log2P1(float x)
	{
		throw null;
	}

	public static float LogP1(float x)
	{
		throw null;
	}

	public static float Max(float x, float y)
	{
		throw null;
	}

	public static float MaxMagnitude(float x, float y)
	{
		throw null;
	}

	public static float MaxMagnitudeNumber(float x, float y)
	{
		throw null;
	}

	public static float MaxNumber(float x, float y)
	{
		throw null;
	}

	public static float Min(float x, float y)
	{
		throw null;
	}

	public static float MinMagnitude(float x, float y)
	{
		throw null;
	}

	public static float MinMagnitudeNumber(float x, float y)
	{
		throw null;
	}

	public static float MinNumber(float x, float y)
	{
		throw null;
	}

	public static bool operator ==(float left, float right)
	{
		throw null;
	}

	public static bool operator >(float left, float right)
	{
		throw null;
	}

	public static bool operator >=(float left, float right)
	{
		throw null;
	}

	public static bool operator !=(float left, float right)
	{
		throw null;
	}

	public static bool operator <(float left, float right)
	{
		throw null;
	}

	public static bool operator <=(float left, float right)
	{
		throw null;
	}

	public static float Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static float Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static float Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static float Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static float Parse(string s)
	{
		throw null;
	}

	public static float Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static float Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static float Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static float Pow(float x, float y)
	{
		throw null;
	}

	public static float RadiansToDegrees(float radians)
	{
		throw null;
	}

	public static float ReciprocalEstimate(float x)
	{
		throw null;
	}

	public static float ReciprocalSqrtEstimate(float x)
	{
		throw null;
	}

	public static float RootN(float x, int n)
	{
		throw null;
	}

	public static float Round(float x)
	{
		throw null;
	}

	public static float Round(float x, int digits)
	{
		throw null;
	}

	public static float Round(float x, int digits, MidpointRounding mode)
	{
		throw null;
	}

	public static float Round(float x, MidpointRounding mode)
	{
		throw null;
	}

	public static float ScaleB(float x, int n)
	{
		throw null;
	}

	public static int Sign(float value)
	{
		throw null;
	}

	public static float Sin(float x)
	{
		throw null;
	}

	public static (float Sin, float Cos) SinCos(float x)
	{
		throw null;
	}

	public static (float SinPi, float CosPi) SinCosPi(float x)
	{
		throw null;
	}

	public static float Sinh(float x)
	{
		throw null;
	}

	public static float SinPi(float x)
	{
		throw null;
	}

	public static float Sqrt(float x)
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

	static float IAdditionOperators<float, float, float>.operator +(float left, float right)
	{
		throw null;
	}

	static float IBitwiseOperators<float, float, float>.operator &(float left, float right)
	{
		throw null;
	}

	static float IBitwiseOperators<float, float, float>.operator |(float left, float right)
	{
		throw null;
	}

	static float IBitwiseOperators<float, float, float>.operator ^(float left, float right)
	{
		throw null;
	}

	static float IBitwiseOperators<float, float, float>.operator ~(float value)
	{
		throw null;
	}

	static float IDecrementOperators<float>.operator --(float value)
	{
		throw null;
	}

	static float IDivisionOperators<float, float, float>.operator /(float left, float right)
	{
		throw null;
	}

	int IFloatingPoint<float>.GetExponentByteCount()
	{
		throw null;
	}

	int IFloatingPoint<float>.GetExponentShortestBitLength()
	{
		throw null;
	}

	int IFloatingPoint<float>.GetSignificandBitLength()
	{
		throw null;
	}

	int IFloatingPoint<float>.GetSignificandByteCount()
	{
		throw null;
	}

	bool IFloatingPoint<float>.TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<float>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<float>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<float>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static float IIncrementOperators<float>.operator ++(float value)
	{
		throw null;
	}

	static float IModulusOperators<float, float, float>.operator %(float left, float right)
	{
		throw null;
	}

	static float IMultiplyOperators<float, float, float>.operator *(float left, float right)
	{
		throw null;
	}

	static bool INumberBase<float>.IsCanonical(float value)
	{
		throw null;
	}

	static bool INumberBase<float>.IsComplexNumber(float value)
	{
		throw null;
	}

	static bool INumberBase<float>.IsImaginaryNumber(float value)
	{
		throw null;
	}

	static bool INumberBase<float>.IsZero(float value)
	{
		throw null;
	}

	static bool INumberBase<float>.TryConvertFromChecked<TOther>(TOther value, out float result)
	{
		throw null;
	}

	static bool INumberBase<float>.TryConvertFromSaturating<TOther>(TOther value, out float result)
	{
		throw null;
	}

	static bool INumberBase<float>.TryConvertFromTruncating<TOther>(TOther value, out float result)
	{
		throw null;
	}

	static bool INumberBase<float>.TryConvertToChecked<TOther>(float value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<float>.TryConvertToSaturating<TOther>(float value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<float>.TryConvertToTruncating<TOther>(float value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static float ISubtractionOperators<float, float, float>.operator -(float left, float right)
	{
		throw null;
	}

	static float IUnaryNegationOperators<float, float>.operator -(float value)
	{
		throw null;
	}

	static float IUnaryPlusOperators<float, float>.operator +(float value)
	{
		throw null;
	}

	public static float Tan(float x)
	{
		throw null;
	}

	public static float Tanh(float x)
	{
		throw null;
	}

	public static float TanPi(float x)
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

	public static float Truncate(float x)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out float result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out float result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out float result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out float result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out float result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out float result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out float result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out float result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out float result)
	{
		throw null;
	}
}
