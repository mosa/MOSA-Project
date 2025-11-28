using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System;

public readonly struct Double : IComparable, IComparable<double>, IConvertible, IEquatable<double>, IFormattable, IParsable<double>, ISpanFormattable, ISpanParsable<double>, IAdditionOperators<double, double, double>, IAdditiveIdentity<double, double>, IBinaryFloatingPointIeee754<double>, IBinaryNumber<double>, IBitwiseOperators<double, double, double>, IComparisonOperators<double, double, bool>, IEqualityOperators<double, double, bool>, IDecrementOperators<double>, IDivisionOperators<double, double, double>, IIncrementOperators<double>, IModulusOperators<double, double, double>, IMultiplicativeIdentity<double, double>, IMultiplyOperators<double, double, double>, INumber<double>, INumberBase<double>, ISubtractionOperators<double, double, double>, IUnaryNegationOperators<double, double>, IUnaryPlusOperators<double, double>, IUtf8SpanFormattable, IUtf8SpanParsable<double>, IExponentialFunctions<double>, IFloatingPointConstants<double>, IFloatingPoint<double>, ISignedNumber<double>, IFloatingPointIeee754<double>, IHyperbolicFunctions<double>, ILogarithmicFunctions<double>, IPowerFunctions<double>, IRootFunctions<double>, ITrigonometricFunctions<double>, IMinMaxValue<double>
{
	private readonly double _dummyPrimitive;

	public const double E = 2.718281828459045;

	public const double Epsilon = 5E-324;

	public const double MaxValue = 1.7976931348623157E+308;

	public const double MinValue = -1.7976931348623157E+308;

	public const double NaN = 0.0 / 0.0;

	public const double NegativeInfinity = -1.0 / 0.0;

	public const double NegativeZero = -0.0;

	public const double Pi = 3.141592653589793;

	public const double PositiveInfinity = 1.0 / 0.0;

	public const double Tau = 6.283185307179586;

	static double IAdditiveIdentity<double, double>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static double IBinaryNumber<double>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static double IFloatingPointConstants<double>.E
	{
		get
		{
			throw null;
		}
	}

	static double IFloatingPointConstants<double>.Pi
	{
		get
		{
			throw null;
		}
	}

	static double IFloatingPointConstants<double>.Tau
	{
		get
		{
			throw null;
		}
	}

	static double IFloatingPointIeee754<double>.Epsilon
	{
		get
		{
			throw null;
		}
	}

	static double IFloatingPointIeee754<double>.NaN
	{
		get
		{
			throw null;
		}
	}

	static double IFloatingPointIeee754<double>.NegativeInfinity
	{
		get
		{
			throw null;
		}
	}

	static double IFloatingPointIeee754<double>.NegativeZero
	{
		get
		{
			throw null;
		}
	}

	static double IFloatingPointIeee754<double>.PositiveInfinity
	{
		get
		{
			throw null;
		}
	}

	static double IMinMaxValue<double>.MaxValue
	{
		get
		{
			throw null;
		}
	}

	static double IMinMaxValue<double>.MinValue
	{
		get
		{
			throw null;
		}
	}

	static double IMultiplicativeIdentity<double, double>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static double INumberBase<double>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<double>.Radix
	{
		get
		{
			throw null;
		}
	}

	static double INumberBase<double>.Zero
	{
		get
		{
			throw null;
		}
	}

	static double ISignedNumber<double>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public static double Abs(double value)
	{
		throw null;
	}

	public static double Acos(double x)
	{
		throw null;
	}

	public static double Acosh(double x)
	{
		throw null;
	}

	public static double AcosPi(double x)
	{
		throw null;
	}

	public static double Asin(double x)
	{
		throw null;
	}

	public static double Asinh(double x)
	{
		throw null;
	}

	public static double AsinPi(double x)
	{
		throw null;
	}

	public static double Atan(double x)
	{
		throw null;
	}

	public static double Atan2(double y, double x)
	{
		throw null;
	}

	public static double Atan2Pi(double y, double x)
	{
		throw null;
	}

	public static double Atanh(double x)
	{
		throw null;
	}

	public static double AtanPi(double x)
	{
		throw null;
	}

	public static double BitDecrement(double x)
	{
		throw null;
	}

	public static double BitIncrement(double x)
	{
		throw null;
	}

	public static double Cbrt(double x)
	{
		throw null;
	}

	public static double Ceiling(double x)
	{
		throw null;
	}

	public static double Clamp(double value, double min, double max)
	{
		throw null;
	}

	public int CompareTo(double value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static double CopySign(double value, double sign)
	{
		throw null;
	}

	public static double Cos(double x)
	{
		throw null;
	}

	public static double Cosh(double x)
	{
		throw null;
	}

	public static double CosPi(double x)
	{
		throw null;
	}

	public static double CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static double CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static double CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static double DegreesToRadians(double degrees)
	{
		throw null;
	}

	public bool Equals(double obj)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static double Exp(double x)
	{
		throw null;
	}

	public static double Exp10(double x)
	{
		throw null;
	}

	public static double Exp10M1(double x)
	{
		throw null;
	}

	public static double Exp2(double x)
	{
		throw null;
	}

	public static double Exp2M1(double x)
	{
		throw null;
	}

	public static double ExpM1(double x)
	{
		throw null;
	}

	public static double Floor(double x)
	{
		throw null;
	}

	public static double FusedMultiplyAdd(double left, double right, double addend)
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

	public static double Hypot(double x, double y)
	{
		throw null;
	}

	public static double Ieee754Remainder(double left, double right)
	{
		throw null;
	}

	public static int ILogB(double x)
	{
		throw null;
	}

	public static bool IsEvenInteger(double value)
	{
		throw null;
	}

	public static bool IsFinite(double d)
	{
		throw null;
	}

	public static bool IsInfinity(double d)
	{
		throw null;
	}

	public static bool IsInteger(double value)
	{
		throw null;
	}

	public static bool IsNaN(double d)
	{
		throw null;
	}

	public static bool IsNegative(double d)
	{
		throw null;
	}

	public static bool IsNegativeInfinity(double d)
	{
		throw null;
	}

	public static bool IsNormal(double d)
	{
		throw null;
	}

	public static bool IsOddInteger(double value)
	{
		throw null;
	}

	public static bool IsPositive(double value)
	{
		throw null;
	}

	public static bool IsPositiveInfinity(double d)
	{
		throw null;
	}

	public static bool IsPow2(double value)
	{
		throw null;
	}

	public static bool IsRealNumber(double value)
	{
		throw null;
	}

	public static bool IsSubnormal(double d)
	{
		throw null;
	}

	public static double Lerp(double value1, double value2, double amount)
	{
		throw null;
	}

	public static double Log(double x)
	{
		throw null;
	}

	public static double Log(double x, double newBase)
	{
		throw null;
	}

	public static double Log10(double x)
	{
		throw null;
	}

	public static double Log10P1(double x)
	{
		throw null;
	}

	public static double Log2(double value)
	{
		throw null;
	}

	public static double Log2P1(double x)
	{
		throw null;
	}

	public static double LogP1(double x)
	{
		throw null;
	}

	public static double Max(double x, double y)
	{
		throw null;
	}

	public static double MaxMagnitude(double x, double y)
	{
		throw null;
	}

	public static double MaxMagnitudeNumber(double x, double y)
	{
		throw null;
	}

	public static double MaxNumber(double x, double y)
	{
		throw null;
	}

	public static double Min(double x, double y)
	{
		throw null;
	}

	public static double MinMagnitude(double x, double y)
	{
		throw null;
	}

	public static double MinMagnitudeNumber(double x, double y)
	{
		throw null;
	}

	public static double MinNumber(double x, double y)
	{
		throw null;
	}

	public static bool operator ==(double left, double right)
	{
		throw null;
	}

	public static bool operator >(double left, double right)
	{
		throw null;
	}

	public static bool operator >=(double left, double right)
	{
		throw null;
	}

	public static bool operator !=(double left, double right)
	{
		throw null;
	}

	public static bool operator <(double left, double right)
	{
		throw null;
	}

	public static bool operator <=(double left, double right)
	{
		throw null;
	}

	public static double Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static double Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static double Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static double Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static double Parse(string s)
	{
		throw null;
	}

	public static double Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static double Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static double Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static double Pow(double x, double y)
	{
		throw null;
	}

	public static double RadiansToDegrees(double radians)
	{
		throw null;
	}

	public static double ReciprocalEstimate(double x)
	{
		throw null;
	}

	public static double ReciprocalSqrtEstimate(double x)
	{
		throw null;
	}

	public static double RootN(double x, int n)
	{
		throw null;
	}

	public static double Round(double x)
	{
		throw null;
	}

	public static double Round(double x, int digits)
	{
		throw null;
	}

	public static double Round(double x, int digits, MidpointRounding mode)
	{
		throw null;
	}

	public static double Round(double x, MidpointRounding mode)
	{
		throw null;
	}

	public static double ScaleB(double x, int n)
	{
		throw null;
	}

	public static int Sign(double value)
	{
		throw null;
	}

	public static double Sin(double x)
	{
		throw null;
	}

	public static (double Sin, double Cos) SinCos(double x)
	{
		throw null;
	}

	public static (double SinPi, double CosPi) SinCosPi(double x)
	{
		throw null;
	}

	public static double Sinh(double x)
	{
		throw null;
	}

	public static double SinPi(double x)
	{
		throw null;
	}

	public static double Sqrt(double x)
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

	static double IAdditionOperators<double, double, double>.operator +(double left, double right)
	{
		throw null;
	}

	static double IBitwiseOperators<double, double, double>.operator &(double left, double right)
	{
		throw null;
	}

	static double IBitwiseOperators<double, double, double>.operator |(double left, double right)
	{
		throw null;
	}

	static double IBitwiseOperators<double, double, double>.operator ^(double left, double right)
	{
		throw null;
	}

	static double IBitwiseOperators<double, double, double>.operator ~(double value)
	{
		throw null;
	}

	static double IDecrementOperators<double>.operator --(double value)
	{
		throw null;
	}

	static double IDivisionOperators<double, double, double>.operator /(double left, double right)
	{
		throw null;
	}

	int IFloatingPoint<double>.GetExponentByteCount()
	{
		throw null;
	}

	int IFloatingPoint<double>.GetExponentShortestBitLength()
	{
		throw null;
	}

	int IFloatingPoint<double>.GetSignificandBitLength()
	{
		throw null;
	}

	int IFloatingPoint<double>.GetSignificandByteCount()
	{
		throw null;
	}

	bool IFloatingPoint<double>.TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<double>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<double>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<double>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static double IIncrementOperators<double>.operator ++(double value)
	{
		throw null;
	}

	static double IModulusOperators<double, double, double>.operator %(double left, double right)
	{
		throw null;
	}

	static double IMultiplyOperators<double, double, double>.operator *(double left, double right)
	{
		throw null;
	}

	static bool INumberBase<double>.IsCanonical(double value)
	{
		throw null;
	}

	static bool INumberBase<double>.IsComplexNumber(double value)
	{
		throw null;
	}

	static bool INumberBase<double>.IsImaginaryNumber(double value)
	{
		throw null;
	}

	static bool INumberBase<double>.IsZero(double value)
	{
		throw null;
	}

	static bool INumberBase<double>.TryConvertFromChecked<TOther>(TOther value, out double result)
	{
		throw null;
	}

	static bool INumberBase<double>.TryConvertFromSaturating<TOther>(TOther value, out double result)
	{
		throw null;
	}

	static bool INumberBase<double>.TryConvertFromTruncating<TOther>(TOther value, out double result)
	{
		throw null;
	}

	static bool INumberBase<double>.TryConvertToChecked<TOther>(double value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<double>.TryConvertToSaturating<TOther>(double value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<double>.TryConvertToTruncating<TOther>(double value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static double ISubtractionOperators<double, double, double>.operator -(double left, double right)
	{
		throw null;
	}

	static double IUnaryNegationOperators<double, double>.operator -(double value)
	{
		throw null;
	}

	static double IUnaryPlusOperators<double, double>.operator +(double value)
	{
		throw null;
	}

	public static double Tan(double x)
	{
		throw null;
	}

	public static double Tanh(double x)
	{
		throw null;
	}

	public static double TanPi(double x)
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

	public static double Truncate(double x)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out double result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out double result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out double result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out double result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out double result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out double result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out double result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out double result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out double result)
	{
		throw null;
	}
}
