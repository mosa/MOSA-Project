namespace System.Numerics;

public interface IBinaryInteger<TSelf> : IComparable, IComparable<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IBinaryNumber<TSelf>, IBitwiseOperators<TSelf, TSelf, TSelf>, IComparisonOperators<TSelf, TSelf, bool>, IEqualityOperators<TSelf, TSelf, bool>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IIncrementOperators<TSelf>, IModulusOperators<TSelf, TSelf, TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, INumber<TSelf>, INumberBase<TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf>, IShiftOperators<TSelf, int, TSelf> where TSelf : IBinaryInteger<TSelf>?
{
	static virtual (TSelf Quotient, TSelf Remainder) DivRem(TSelf left, TSelf right)
	{
		throw null;
	}

	int GetByteCount();

	int GetShortestBitLength();

	static virtual TSelf LeadingZeroCount(TSelf value)
	{
		throw null;
	}

	static abstract TSelf PopCount(TSelf value);

	static virtual TSelf ReadBigEndian(byte[] source, bool isUnsigned)
	{
		throw null;
	}

	static virtual TSelf ReadBigEndian(byte[] source, int startIndex, bool isUnsigned)
	{
		throw null;
	}

	static virtual TSelf ReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned)
	{
		throw null;
	}

	static virtual TSelf ReadLittleEndian(byte[] source, bool isUnsigned)
	{
		throw null;
	}

	static virtual TSelf ReadLittleEndian(byte[] source, int startIndex, bool isUnsigned)
	{
		throw null;
	}

	static virtual TSelf ReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned)
	{
		throw null;
	}

	static virtual TSelf RotateLeft(TSelf value, int rotateAmount)
	{
		throw null;
	}

	static virtual TSelf RotateRight(TSelf value, int rotateAmount)
	{
		throw null;
	}

	static abstract TSelf TrailingZeroCount(TSelf value);

	static abstract bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out TSelf value);

	static abstract bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out TSelf value);

	bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten);

	bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten);

	int WriteBigEndian(byte[] destination)
	{
		throw null;
	}

	int WriteBigEndian(byte[] destination, int startIndex)
	{
		throw null;
	}

	int WriteBigEndian(Span<byte> destination)
	{
		throw null;
	}

	int WriteLittleEndian(byte[] destination)
	{
		throw null;
	}

	int WriteLittleEndian(byte[] destination, int startIndex)
	{
		throw null;
	}

	int WriteLittleEndian(Span<byte> destination)
	{
		throw null;
	}
}
