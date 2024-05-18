using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace System.Runtime.InteropServices;

public readonly struct NFloat : IComparable, IComparable<NFloat>, IEquatable<NFloat>, IFormattable, IParsable<NFloat>, ISpanFormattable, ISpanParsable<NFloat>, IAdditionOperators<NFloat, NFloat, NFloat>, IAdditiveIdentity<NFloat, NFloat>, IBinaryFloatingPointIeee754<NFloat>, IBinaryNumber<NFloat>, IBitwiseOperators<NFloat, NFloat, NFloat>, IComparisonOperators<NFloat, NFloat, bool>, IEqualityOperators<NFloat, NFloat, bool>, IDecrementOperators<NFloat>, IDivisionOperators<NFloat, NFloat, NFloat>, IIncrementOperators<NFloat>, IModulusOperators<NFloat, NFloat, NFloat>, IMultiplicativeIdentity<NFloat, NFloat>, IMultiplyOperators<NFloat, NFloat, NFloat>, INumber<NFloat>, INumberBase<NFloat>, ISubtractionOperators<NFloat, NFloat, NFloat>, IUnaryNegationOperators<NFloat, NFloat>, IUnaryPlusOperators<NFloat, NFloat>, IUtf8SpanFormattable, IUtf8SpanParsable<NFloat>, IExponentialFunctions<NFloat>, IFloatingPointConstants<NFloat>, IFloatingPoint<NFloat>, ISignedNumber<NFloat>, IFloatingPointIeee754<NFloat>, IHyperbolicFunctions<NFloat>, ILogarithmicFunctions<NFloat>, IPowerFunctions<NFloat>, IRootFunctions<NFloat>, ITrigonometricFunctions<NFloat>, IMinMaxValue<NFloat>
{
	private readonly int _dummyPrimitive;

	public static NFloat E
	{
		get
		{
			throw null;
		}
	}

	public static NFloat Epsilon
	{
		get
		{
			throw null;
		}
	}

	public static NFloat MaxValue
	{
		get
		{
			throw null;
		}
	}

	public static NFloat MinValue
	{
		get
		{
			throw null;
		}
	}

	public static NFloat NaN
	{
		get
		{
			throw null;
		}
	}

	public static NFloat NegativeInfinity
	{
		get
		{
			throw null;
		}
	}

	public static NFloat NegativeZero
	{
		get
		{
			throw null;
		}
	}

	public static NFloat Pi
	{
		get
		{
			throw null;
		}
	}

	public static NFloat PositiveInfinity
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

	static NFloat IAdditiveIdentity<NFloat, NFloat>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static NFloat IBinaryNumber<NFloat>.AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static NFloat IMultiplicativeIdentity<NFloat, NFloat>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static NFloat INumberBase<NFloat>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<NFloat>.Radix
	{
		get
		{
			throw null;
		}
	}

	static NFloat INumberBase<NFloat>.Zero
	{
		get
		{
			throw null;
		}
	}

	static NFloat ISignedNumber<NFloat>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public static NFloat Tau
	{
		get
		{
			throw null;
		}
	}

	public double Value
	{
		get
		{
			throw null;
		}
	}

	public NFloat(double value)
	{
		throw null;
	}

	public NFloat(float value)
	{
		throw null;
	}

	public static NFloat Abs(NFloat value)
	{
		throw null;
	}

	public static NFloat Acos(NFloat x)
	{
		throw null;
	}

	public static NFloat Acosh(NFloat x)
	{
		throw null;
	}

	public static NFloat AcosPi(NFloat x)
	{
		throw null;
	}

	public static NFloat Asin(NFloat x)
	{
		throw null;
	}

	public static NFloat Asinh(NFloat x)
	{
		throw null;
	}

	public static NFloat AsinPi(NFloat x)
	{
		throw null;
	}

	public static NFloat Atan(NFloat x)
	{
		throw null;
	}

	public static NFloat Atan2(NFloat y, NFloat x)
	{
		throw null;
	}

	public static NFloat Atan2Pi(NFloat y, NFloat x)
	{
		throw null;
	}

	public static NFloat Atanh(NFloat x)
	{
		throw null;
	}

	public static NFloat AtanPi(NFloat x)
	{
		throw null;
	}

	public static NFloat BitDecrement(NFloat x)
	{
		throw null;
	}

	public static NFloat BitIncrement(NFloat x)
	{
		throw null;
	}

	public static NFloat Cbrt(NFloat x)
	{
		throw null;
	}

	public static NFloat Ceiling(NFloat x)
	{
		throw null;
	}

	public static NFloat Clamp(NFloat value, NFloat min, NFloat max)
	{
		throw null;
	}

	public int CompareTo(object? obj)
	{
		throw null;
	}

	public int CompareTo(NFloat other)
	{
		throw null;
	}

	public static NFloat CopySign(NFloat value, NFloat sign)
	{
		throw null;
	}

	public static NFloat Cos(NFloat x)
	{
		throw null;
	}

	public static NFloat Cosh(NFloat x)
	{
		throw null;
	}

	public static NFloat CosPi(NFloat x)
	{
		throw null;
	}

	public static NFloat CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static NFloat CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static NFloat CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static NFloat DegreesToRadians(NFloat degrees)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(NFloat other)
	{
		throw null;
	}

	public static NFloat Exp(NFloat x)
	{
		throw null;
	}

	public static NFloat Exp10(NFloat x)
	{
		throw null;
	}

	public static NFloat Exp10M1(NFloat x)
	{
		throw null;
	}

	public static NFloat Exp2(NFloat x)
	{
		throw null;
	}

	public static NFloat Exp2M1(NFloat x)
	{
		throw null;
	}

	public static NFloat ExpM1(NFloat x)
	{
		throw null;
	}

	public static NFloat Floor(NFloat x)
	{
		throw null;
	}

	public static NFloat FusedMultiplyAdd(NFloat left, NFloat right, NFloat addend)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static NFloat Hypot(NFloat x, NFloat y)
	{
		throw null;
	}

	public static NFloat Ieee754Remainder(NFloat left, NFloat right)
	{
		throw null;
	}

	public static int ILogB(NFloat x)
	{
		throw null;
	}

	public static bool IsEvenInteger(NFloat value)
	{
		throw null;
	}

	public static bool IsFinite(NFloat value)
	{
		throw null;
	}

	public static bool IsInfinity(NFloat value)
	{
		throw null;
	}

	public static bool IsInteger(NFloat value)
	{
		throw null;
	}

	public static bool IsNaN(NFloat value)
	{
		throw null;
	}

	public static bool IsNegative(NFloat value)
	{
		throw null;
	}

	public static bool IsNegativeInfinity(NFloat value)
	{
		throw null;
	}

	public static bool IsNormal(NFloat value)
	{
		throw null;
	}

	public static bool IsOddInteger(NFloat value)
	{
		throw null;
	}

	public static bool IsPositive(NFloat value)
	{
		throw null;
	}

	public static bool IsPositiveInfinity(NFloat value)
	{
		throw null;
	}

	public static bool IsPow2(NFloat value)
	{
		throw null;
	}

	public static bool IsRealNumber(NFloat value)
	{
		throw null;
	}

	public static bool IsSubnormal(NFloat value)
	{
		throw null;
	}

	public static NFloat Lerp(NFloat value1, NFloat value2, NFloat amount)
	{
		throw null;
	}

	public static NFloat Log(NFloat x)
	{
		throw null;
	}

	public static NFloat Log(NFloat x, NFloat newBase)
	{
		throw null;
	}

	public static NFloat Log10(NFloat x)
	{
		throw null;
	}

	public static NFloat Log10P1(NFloat x)
	{
		throw null;
	}

	public static NFloat Log2(NFloat value)
	{
		throw null;
	}

	public static NFloat Log2P1(NFloat x)
	{
		throw null;
	}

	public static NFloat LogP1(NFloat x)
	{
		throw null;
	}

	public static NFloat Max(NFloat x, NFloat y)
	{
		throw null;
	}

	public static NFloat MaxMagnitude(NFloat x, NFloat y)
	{
		throw null;
	}

	public static NFloat MaxMagnitudeNumber(NFloat x, NFloat y)
	{
		throw null;
	}

	public static NFloat MaxNumber(NFloat x, NFloat y)
	{
		throw null;
	}

	public static NFloat Min(NFloat x, NFloat y)
	{
		throw null;
	}

	public static NFloat MinMagnitude(NFloat x, NFloat y)
	{
		throw null;
	}

	public static NFloat MinMagnitudeNumber(NFloat x, NFloat y)
	{
		throw null;
	}

	public static NFloat MinNumber(NFloat x, NFloat y)
	{
		throw null;
	}

	public static NFloat operator +(NFloat left, NFloat right)
	{
		throw null;
	}

	public static explicit operator checked byte(NFloat value)
	{
		throw null;
	}

	public static explicit operator checked char(NFloat value)
	{
		throw null;
	}

	public static explicit operator checked short(NFloat value)
	{
		throw null;
	}

	public static explicit operator checked int(NFloat value)
	{
		throw null;
	}

	public static explicit operator checked long(NFloat value)
	{
		throw null;
	}

	public static explicit operator checked Int128(NFloat value)
	{
		throw null;
	}

	public static explicit operator checked IntPtr(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked sbyte(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked ushort(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked uint(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked ulong(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked UInt128(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator checked UIntPtr(NFloat value)
	{
		throw null;
	}

	public static NFloat operator --(NFloat value)
	{
		throw null;
	}

	public static NFloat operator /(NFloat left, NFloat right)
	{
		throw null;
	}

	public static bool operator ==(NFloat left, NFloat right)
	{
		throw null;
	}

	public static explicit operator NFloat(decimal value)
	{
		throw null;
	}

	public static explicit operator NFloat(double value)
	{
		throw null;
	}

	public static explicit operator NFloat(Int128 value)
	{
		throw null;
	}

	public static explicit operator byte(NFloat value)
	{
		throw null;
	}

	public static explicit operator char(NFloat value)
	{
		throw null;
	}

	public static explicit operator decimal(NFloat value)
	{
		throw null;
	}

	public static explicit operator Half(NFloat value)
	{
		throw null;
	}

	public static explicit operator Int128(NFloat value)
	{
		throw null;
	}

	public static explicit operator short(NFloat value)
	{
		throw null;
	}

	public static explicit operator int(NFloat value)
	{
		throw null;
	}

	public static explicit operator long(NFloat value)
	{
		throw null;
	}

	public static explicit operator IntPtr(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator sbyte(NFloat value)
	{
		throw null;
	}

	public static explicit operator float(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator UInt128(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ushort(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator uint(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ulong(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator UIntPtr(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator NFloat(UInt128 value)
	{
		throw null;
	}

	public static bool operator >(NFloat left, NFloat right)
	{
		throw null;
	}

	public static bool operator >=(NFloat left, NFloat right)
	{
		throw null;
	}

	public static implicit operator NFloat(byte value)
	{
		throw null;
	}

	public static implicit operator NFloat(char value)
	{
		throw null;
	}

	public static implicit operator NFloat(short value)
	{
		throw null;
	}

	public static implicit operator NFloat(int value)
	{
		throw null;
	}

	public static implicit operator NFloat(long value)
	{
		throw null;
	}

	public static implicit operator NFloat(IntPtr value)
	{
		throw null;
	}

	public static implicit operator NFloat(Half value)
	{
		throw null;
	}

	public static implicit operator double(NFloat value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator NFloat(sbyte value)
	{
		throw null;
	}

	public static implicit operator NFloat(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator NFloat(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator NFloat(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator NFloat(ulong value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator NFloat(UIntPtr value)
	{
		throw null;
	}

	public static NFloat operator ++(NFloat value)
	{
		throw null;
	}

	public static bool operator !=(NFloat left, NFloat right)
	{
		throw null;
	}

	public static bool operator <(NFloat left, NFloat right)
	{
		throw null;
	}

	public static bool operator <=(NFloat left, NFloat right)
	{
		throw null;
	}

	public static NFloat operator %(NFloat left, NFloat right)
	{
		throw null;
	}

	public static NFloat operator *(NFloat left, NFloat right)
	{
		throw null;
	}

	public static NFloat operator -(NFloat left, NFloat right)
	{
		throw null;
	}

	public static NFloat operator -(NFloat value)
	{
		throw null;
	}

	public static NFloat operator +(NFloat value)
	{
		throw null;
	}

	public static NFloat Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static NFloat Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	public static NFloat Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
	{
		throw null;
	}

	public static NFloat Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static NFloat Parse(string s)
	{
		throw null;
	}

	public static NFloat Parse(string s, NumberStyles style)
	{
		throw null;
	}

	public static NFloat Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static NFloat Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static NFloat Pow(NFloat x, NFloat y)
	{
		throw null;
	}

	public static NFloat RadiansToDegrees(NFloat radians)
	{
		throw null;
	}

	public static NFloat ReciprocalEstimate(NFloat x)
	{
		throw null;
	}

	public static NFloat ReciprocalSqrtEstimate(NFloat x)
	{
		throw null;
	}

	public static NFloat RootN(NFloat x, int n)
	{
		throw null;
	}

	public static NFloat Round(NFloat x)
	{
		throw null;
	}

	public static NFloat Round(NFloat x, int digits)
	{
		throw null;
	}

	public static NFloat Round(NFloat x, int digits, MidpointRounding mode)
	{
		throw null;
	}

	public static NFloat Round(NFloat x, MidpointRounding mode)
	{
		throw null;
	}

	public static NFloat ScaleB(NFloat x, int n)
	{
		throw null;
	}

	public static int Sign(NFloat value)
	{
		throw null;
	}

	public static NFloat Sin(NFloat x)
	{
		throw null;
	}

	public static (NFloat Sin, NFloat Cos) SinCos(NFloat x)
	{
		throw null;
	}

	public static (NFloat SinPi, NFloat CosPi) SinCosPi(NFloat x)
	{
		throw null;
	}

	public static NFloat Sinh(NFloat x)
	{
		throw null;
	}

	public static NFloat SinPi(NFloat x)
	{
		throw null;
	}

	public static NFloat Sqrt(NFloat x)
	{
		throw null;
	}

	static NFloat IAdditionOperators<NFloat, NFloat, NFloat>.operator checked +(NFloat left, NFloat right)
	{
		throw null;
	}

	static NFloat IBitwiseOperators<NFloat, NFloat, NFloat>.operator &(NFloat left, NFloat right)
	{
		throw null;
	}

	static NFloat IBitwiseOperators<NFloat, NFloat, NFloat>.operator |(NFloat left, NFloat right)
	{
		throw null;
	}

	static NFloat IBitwiseOperators<NFloat, NFloat, NFloat>.operator ^(NFloat left, NFloat right)
	{
		throw null;
	}

	static NFloat IBitwiseOperators<NFloat, NFloat, NFloat>.operator ~(NFloat value)
	{
		throw null;
	}

	static NFloat IDecrementOperators<NFloat>.operator checked --(NFloat value)
	{
		throw null;
	}

	static NFloat IDivisionOperators<NFloat, NFloat, NFloat>.operator checked /(NFloat left, NFloat right)
	{
		throw null;
	}

	int IFloatingPoint<NFloat>.GetExponentByteCount()
	{
		throw null;
	}

	int IFloatingPoint<NFloat>.GetExponentShortestBitLength()
	{
		throw null;
	}

	int IFloatingPoint<NFloat>.GetSignificandBitLength()
	{
		throw null;
	}

	int IFloatingPoint<NFloat>.GetSignificandByteCount()
	{
		throw null;
	}

	bool IFloatingPoint<NFloat>.TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<NFloat>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<NFloat>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	bool IFloatingPoint<NFloat>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	static NFloat IIncrementOperators<NFloat>.operator checked ++(NFloat value)
	{
		throw null;
	}

	static NFloat IMultiplyOperators<NFloat, NFloat, NFloat>.operator checked *(NFloat left, NFloat right)
	{
		throw null;
	}

	static bool INumberBase<NFloat>.IsCanonical(NFloat value)
	{
		throw null;
	}

	static bool INumberBase<NFloat>.IsComplexNumber(NFloat value)
	{
		throw null;
	}

	static bool INumberBase<NFloat>.IsImaginaryNumber(NFloat value)
	{
		throw null;
	}

	static bool INumberBase<NFloat>.IsZero(NFloat value)
	{
		throw null;
	}

	static bool INumberBase<NFloat>.TryConvertFromChecked<TOther>(TOther value, out NFloat result)
	{
		throw null;
	}

	static bool INumberBase<NFloat>.TryConvertFromSaturating<TOther>(TOther value, out NFloat result)
	{
		throw null;
	}

	static bool INumberBase<NFloat>.TryConvertFromTruncating<TOther>(TOther value, out NFloat result)
	{
		throw null;
	}

	static bool INumberBase<NFloat>.TryConvertToChecked<TOther>(NFloat value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<NFloat>.TryConvertToSaturating<TOther>(NFloat value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<NFloat>.TryConvertToTruncating<TOther>(NFloat value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static NFloat ISubtractionOperators<NFloat, NFloat, NFloat>.operator checked -(NFloat left, NFloat right)
	{
		throw null;
	}

	static NFloat IUnaryNegationOperators<NFloat, NFloat>.operator checked -(NFloat value)
	{
		throw null;
	}

	public static NFloat Tan(NFloat x)
	{
		throw null;
	}

	public static NFloat Tanh(NFloat x)
	{
		throw null;
	}

	public static NFloat TanPi(NFloat x)
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

	public static NFloat Truncate(NFloat x)
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

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out NFloat result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out NFloat result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<byte> utf8Text, out NFloat result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out NFloat result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out NFloat result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out NFloat result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out NFloat result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out NFloat result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out NFloat result)
	{
		throw null;
	}
}
