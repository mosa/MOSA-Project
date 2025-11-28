namespace System.Numerics;

public interface IExponentialFunctions<TSelf> : IFloatingPointConstants<TSelf>, INumberBase<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IEqualityOperators<TSelf, TSelf, bool>, IIncrementOperators<TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf> where TSelf : IExponentialFunctions<TSelf>?
{
	static abstract TSelf Exp(TSelf x);

	static abstract TSelf Exp10(TSelf x);

	static virtual TSelf Exp10M1(TSelf x)
	{
		throw null;
	}

	static abstract TSelf Exp2(TSelf x);

	static virtual TSelf Exp2M1(TSelf x)
	{
		throw null;
	}

	static virtual TSelf ExpM1(TSelf x)
	{
		throw null;
	}
}
