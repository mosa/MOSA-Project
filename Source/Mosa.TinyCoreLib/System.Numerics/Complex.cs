using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Numerics;

public readonly struct Complex : IEquatable<Complex>, IFormattable, IParsable<Complex>, ISpanFormattable, ISpanParsable<Complex>, IAdditionOperators<Complex, Complex, Complex>, IAdditiveIdentity<Complex, Complex>, IDecrementOperators<Complex>, IDivisionOperators<Complex, Complex, Complex>, IEqualityOperators<Complex, Complex, bool>, IIncrementOperators<Complex>, IMultiplicativeIdentity<Complex, Complex>, IMultiplyOperators<Complex, Complex, Complex>, INumberBase<Complex>, ISubtractionOperators<Complex, Complex, Complex>, IUnaryNegationOperators<Complex, Complex>, IUnaryPlusOperators<Complex, Complex>, IUtf8SpanFormattable, IUtf8SpanParsable<Complex>, ISignedNumber<Complex>
{
	private readonly int _dummyPrimitive;

	public static readonly Complex ImaginaryOne;

	public static readonly Complex Infinity;

	public static readonly Complex NaN;

	public static readonly Complex One;

	public static readonly Complex Zero;

	public double Imaginary
	{
		get
		{
			throw null;
		}
	}

	public double Magnitude
	{
		get
		{
			throw null;
		}
	}

	public double Phase
	{
		get
		{
			throw null;
		}
	}

	public double Real
	{
		get
		{
			throw null;
		}
	}

	static Complex IAdditiveIdentity<Complex, Complex>.AdditiveIdentity
	{
		get
		{
			throw null;
		}
	}

	static Complex IMultiplicativeIdentity<Complex, Complex>.MultiplicativeIdentity
	{
		get
		{
			throw null;
		}
	}

	static Complex INumberBase<Complex>.One
	{
		get
		{
			throw null;
		}
	}

	static int INumberBase<Complex>.Radix
	{
		get
		{
			throw null;
		}
	}

	static Complex INumberBase<Complex>.Zero
	{
		get
		{
			throw null;
		}
	}

	static Complex ISignedNumber<Complex>.NegativeOne
	{
		get
		{
			throw null;
		}
	}

	public Complex(double real, double imaginary)
	{
		throw null;
	}

	public static double Abs(Complex value)
	{
		throw null;
	}

	public static Complex Acos(Complex value)
	{
		throw null;
	}

	public static Complex Add(double left, Complex right)
	{
		throw null;
	}

	public static Complex Add(Complex left, double right)
	{
		throw null;
	}

	public static Complex Add(Complex left, Complex right)
	{
		throw null;
	}

	public static Complex Asin(Complex value)
	{
		throw null;
	}

	public static Complex Atan(Complex value)
	{
		throw null;
	}

	public static Complex Conjugate(Complex value)
	{
		throw null;
	}

	public static Complex Cos(Complex value)
	{
		throw null;
	}

	public static Complex Cosh(Complex value)
	{
		throw null;
	}

	public static Complex CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static Complex CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static Complex CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	public static Complex Divide(double dividend, Complex divisor)
	{
		throw null;
	}

	public static Complex Divide(Complex dividend, double divisor)
	{
		throw null;
	}

	public static Complex Divide(Complex dividend, Complex divisor)
	{
		throw null;
	}

	public bool Equals(Complex value)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static Complex Exp(Complex value)
	{
		throw null;
	}

	public static Complex FromPolarCoordinates(double magnitude, double phase)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool IsComplexNumber(Complex value)
	{
		throw null;
	}

	public static bool IsEvenInteger(Complex value)
	{
		throw null;
	}

	public static bool IsFinite(Complex value)
	{
		throw null;
	}

	public static bool IsImaginaryNumber(Complex value)
	{
		throw null;
	}

	public static bool IsInfinity(Complex value)
	{
		throw null;
	}

	public static bool IsInteger(Complex value)
	{
		throw null;
	}

	public static bool IsNaN(Complex value)
	{
		throw null;
	}

	public static bool IsNegative(Complex value)
	{
		throw null;
	}

	public static bool IsNegativeInfinity(Complex value)
	{
		throw null;
	}

	public static bool IsNormal(Complex value)
	{
		throw null;
	}

	public static bool IsOddInteger(Complex value)
	{
		throw null;
	}

	public static bool IsPositive(Complex value)
	{
		throw null;
	}

	public static bool IsPositiveInfinity(Complex value)
	{
		throw null;
	}

	public static bool IsRealNumber(Complex value)
	{
		throw null;
	}

	public static bool IsSubnormal(Complex value)
	{
		throw null;
	}

	public static Complex Log(Complex value)
	{
		throw null;
	}

	public static Complex Log(Complex value, double baseValue)
	{
		throw null;
	}

	public static Complex Log10(Complex value)
	{
		throw null;
	}

	public static Complex MaxMagnitude(Complex x, Complex y)
	{
		throw null;
	}

	public static Complex MinMagnitude(Complex x, Complex y)
	{
		throw null;
	}

	public static Complex Multiply(double left, Complex right)
	{
		throw null;
	}

	public static Complex Multiply(Complex left, double right)
	{
		throw null;
	}

	public static Complex Multiply(Complex left, Complex right)
	{
		throw null;
	}

	public static Complex Negate(Complex value)
	{
		throw null;
	}

	public static Complex operator +(double left, Complex right)
	{
		throw null;
	}

	public static Complex operator +(Complex left, double right)
	{
		throw null;
	}

	public static Complex operator +(Complex left, Complex right)
	{
		throw null;
	}

	public static Complex operator --(Complex value)
	{
		throw null;
	}

	public static Complex operator /(double left, Complex right)
	{
		throw null;
	}

	public static Complex operator /(Complex left, double right)
	{
		throw null;
	}

	public static Complex operator /(Complex left, Complex right)
	{
		throw null;
	}

	public static bool operator ==(Complex left, Complex right)
	{
		throw null;
	}

	public static explicit operator Complex(decimal value)
	{
		throw null;
	}

	public static explicit operator Complex(Int128 value)
	{
		throw null;
	}

	public static explicit operator Complex(BigInteger value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Complex(UInt128 value)
	{
		throw null;
	}

	public static implicit operator Complex(byte value)
	{
		throw null;
	}

	public static implicit operator Complex(char value)
	{
		throw null;
	}

	public static implicit operator Complex(double value)
	{
		throw null;
	}

	public static implicit operator Complex(Half value)
	{
		throw null;
	}

	public static implicit operator Complex(short value)
	{
		throw null;
	}

	public static implicit operator Complex(int value)
	{
		throw null;
	}

	public static implicit operator Complex(long value)
	{
		throw null;
	}

	public static implicit operator Complex(IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Complex(sbyte value)
	{
		throw null;
	}

	public static implicit operator Complex(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Complex(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Complex(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Complex(ulong value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator Complex(UIntPtr value)
	{
		throw null;
	}

	public static Complex operator ++(Complex value)
	{
		throw null;
	}

	public static bool operator !=(Complex left, Complex right)
	{
		throw null;
	}

	public static Complex operator *(double left, Complex right)
	{
		throw null;
	}

	public static Complex operator *(Complex left, double right)
	{
		throw null;
	}

	public static Complex operator *(Complex left, Complex right)
	{
		throw null;
	}

	public static Complex operator -(double left, Complex right)
	{
		throw null;
	}

	public static Complex operator -(Complex left, double right)
	{
		throw null;
	}

	public static Complex operator -(Complex left, Complex right)
	{
		throw null;
	}

	public static Complex operator -(Complex value)
	{
		throw null;
	}

	public static Complex operator +(Complex value)
	{
		throw null;
	}

	public static Complex Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static Complex Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static Complex Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	public static Complex Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static Complex Pow(Complex value, double power)
	{
		throw null;
	}

	public static Complex Pow(Complex value, Complex power)
	{
		throw null;
	}

	public static Complex Reciprocal(Complex value)
	{
		throw null;
	}

	public static Complex Sin(Complex value)
	{
		throw null;
	}

	public static Complex Sinh(Complex value)
	{
		throw null;
	}

	public static Complex Sqrt(Complex value)
	{
		throw null;
	}

	public static Complex Subtract(double left, Complex right)
	{
		throw null;
	}

	public static Complex Subtract(Complex left, double right)
	{
		throw null;
	}

	public static Complex Subtract(Complex left, Complex right)
	{
		throw null;
	}

	static Complex INumberBase<Complex>.Abs(Complex value)
	{
		throw null;
	}

	static bool INumberBase<Complex>.IsCanonical(Complex value)
	{
		throw null;
	}

	static bool INumberBase<Complex>.IsZero(Complex value)
	{
		throw null;
	}

	static Complex INumberBase<Complex>.MaxMagnitudeNumber(Complex x, Complex y)
	{
		throw null;
	}

	static Complex INumberBase<Complex>.MinMagnitudeNumber(Complex x, Complex y)
	{
		throw null;
	}

	static bool INumberBase<Complex>.TryConvertFromChecked<TOther>(TOther value, out Complex result)
	{
		throw null;
	}

	static bool INumberBase<Complex>.TryConvertFromSaturating<TOther>(TOther value, out Complex result)
	{
		throw null;
	}

	static bool INumberBase<Complex>.TryConvertFromTruncating<TOther>(TOther value, out Complex result)
	{
		throw null;
	}

	static bool INumberBase<Complex>.TryConvertToChecked<TOther>(Complex value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<Complex>.TryConvertToSaturating<TOther>(Complex value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	static bool INumberBase<Complex>.TryConvertToTruncating<TOther>(Complex value, [MaybeNullWhen(false)] out TOther result)
	{
		throw null;
	}

	public static Complex Tan(Complex value)
	{
		throw null;
	}

	public static Complex Tanh(Complex value)
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

	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax("NumericFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax("NumericFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Complex result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Complex result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Complex result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Complex result)
	{
		throw null;
	}
}
