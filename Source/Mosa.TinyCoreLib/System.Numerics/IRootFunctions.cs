namespace System.Numerics;

public interface IRootFunctions<TSelf> : IFloatingPointConstants<TSelf>, INumberBase<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IEqualityOperators<TSelf, TSelf, bool>, IIncrementOperators<TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf> where TSelf : IRootFunctions<TSelf>?
{
	static abstract TSelf Cbrt(TSelf x);

	static abstract TSelf Hypot(TSelf x, TSelf y);

	static abstract TSelf RootN(TSelf x, int n);

	static abstract TSelf Sqrt(TSelf x);
}
