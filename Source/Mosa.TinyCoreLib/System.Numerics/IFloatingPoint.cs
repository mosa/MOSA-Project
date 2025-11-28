namespace System.Numerics;

public interface IFloatingPoint<TSelf> : IComparable, IComparable<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IComparisonOperators<TSelf, TSelf, bool>, IEqualityOperators<TSelf, TSelf, bool>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IFloatingPointConstants<TSelf>, INumberBase<TSelf>, IIncrementOperators<TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf>, IModulusOperators<TSelf, TSelf, TSelf>, INumber<TSelf>, ISignedNumber<TSelf> where TSelf : IFloatingPoint<TSelf>?
{
	static virtual TSelf Ceiling(TSelf x)
	{
		throw null;
	}

	static virtual TSelf Floor(TSelf x)
	{
		throw null;
	}

	int GetExponentByteCount();

	int GetExponentShortestBitLength();

	int GetSignificandBitLength();

	int GetSignificandByteCount();

	static virtual TSelf Round(TSelf x)
	{
		throw null;
	}

	static virtual TSelf Round(TSelf x, int digits)
	{
		throw null;
	}

	static abstract TSelf Round(TSelf x, int digits, MidpointRounding mode);

	static virtual TSelf Round(TSelf x, MidpointRounding mode)
	{
		throw null;
	}

	static virtual TSelf Truncate(TSelf x)
	{
		throw null;
	}

	bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten);

	bool TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten);

	bool TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten);

	bool TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten);

	int WriteExponentBigEndian(byte[] destination)
	{
		throw null;
	}

	int WriteExponentBigEndian(byte[] destination, int startIndex)
	{
		throw null;
	}

	int WriteExponentBigEndian(Span<byte> destination)
	{
		throw null;
	}

	int WriteExponentLittleEndian(byte[] destination)
	{
		throw null;
	}

	int WriteExponentLittleEndian(byte[] destination, int startIndex)
	{
		throw null;
	}

	int WriteExponentLittleEndian(Span<byte> destination)
	{
		throw null;
	}

	int WriteSignificandBigEndian(byte[] destination)
	{
		throw null;
	}

	int WriteSignificandBigEndian(byte[] destination, int startIndex)
	{
		throw null;
	}

	int WriteSignificandBigEndian(Span<byte> destination)
	{
		throw null;
	}

	int WriteSignificandLittleEndian(byte[] destination)
	{
		throw null;
	}

	int WriteSignificandLittleEndian(byte[] destination, int startIndex)
	{
		throw null;
	}

	int WriteSignificandLittleEndian(Span<byte> destination)
	{
		throw null;
	}
}
