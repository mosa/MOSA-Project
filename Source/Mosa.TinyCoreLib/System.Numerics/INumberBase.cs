using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Numerics;

public interface INumberBase<TSelf> : IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IEqualityOperators<TSelf, TSelf, bool>, IIncrementOperators<TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf> where TSelf : INumberBase<TSelf>?
{
	static abstract TSelf One { get; }

	static abstract int Radix { get; }

	static abstract TSelf Zero { get; }

	static abstract TSelf Abs(TSelf value);

	static virtual TSelf CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	static virtual TSelf CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	static virtual TSelf CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
	{
		throw null;
	}

	static abstract bool IsCanonical(TSelf value);

	static abstract bool IsComplexNumber(TSelf value);

	static abstract bool IsEvenInteger(TSelf value);

	static abstract bool IsFinite(TSelf value);

	static abstract bool IsImaginaryNumber(TSelf value);

	static abstract bool IsInfinity(TSelf value);

	static abstract bool IsInteger(TSelf value);

	static abstract bool IsNaN(TSelf value);

	static abstract bool IsNegative(TSelf value);

	static abstract bool IsNegativeInfinity(TSelf value);

	static abstract bool IsNormal(TSelf value);

	static abstract bool IsOddInteger(TSelf value);

	static abstract bool IsPositive(TSelf value);

	static abstract bool IsPositiveInfinity(TSelf value);

	static abstract bool IsRealNumber(TSelf value);

	static abstract bool IsSubnormal(TSelf value);

	static abstract bool IsZero(TSelf value);

	static abstract TSelf MaxMagnitude(TSelf x, TSelf y);

	static abstract TSelf MaxMagnitudeNumber(TSelf x, TSelf y);

	static abstract TSelf MinMagnitude(TSelf x, TSelf y);

	static abstract TSelf MinMagnitudeNumber(TSelf x, TSelf y);

	static virtual TSelf Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
	{
		throw null;
	}

	static abstract TSelf Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider);

	static abstract TSelf Parse(string s, NumberStyles style, IFormatProvider? provider);

	bool IUtf8SpanFormattable.TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	static TSelf IUtf8SpanParsable<TSelf>.Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		throw null;
	}

	static bool IUtf8SpanParsable<TSelf>.TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result)
	{
		throw null;
	}

	protected static abstract bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TOther : INumberBase<TOther>;

	protected static abstract bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TOther : INumberBase<TOther>;

	protected static abstract bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TOther : INumberBase<TOther>;

	protected static abstract bool TryConvertToChecked<TOther>(TSelf value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>;

	protected static abstract bool TryConvertToSaturating<TOther>(TSelf value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>;

	protected static abstract bool TryConvertToTruncating<TOther>(TSelf value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>;

	static virtual bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result)
	{
		throw null;
	}

	static abstract bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result);

	static abstract bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result);
}
