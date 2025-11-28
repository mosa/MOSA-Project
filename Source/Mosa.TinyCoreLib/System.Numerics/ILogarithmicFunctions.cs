namespace System.Numerics;

public interface ILogarithmicFunctions<TSelf> : IFloatingPointConstants<TSelf>, INumberBase<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IEqualityOperators<TSelf, TSelf, bool>, IIncrementOperators<TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf> where TSelf : ILogarithmicFunctions<TSelf>?
{
	static abstract TSelf Log(TSelf x);

	static abstract TSelf Log(TSelf x, TSelf newBase);

	static abstract TSelf Log10(TSelf x);

	static virtual TSelf Log10P1(TSelf x)
	{
		throw null;
	}

	static abstract TSelf Log2(TSelf x);

	static virtual TSelf Log2P1(TSelf x)
	{
		throw null;
	}

	static virtual TSelf LogP1(TSelf x)
	{
		throw null;
	}
}
