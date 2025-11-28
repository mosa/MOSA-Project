namespace System.Numerics;

public interface IHyperbolicFunctions<TSelf> : IFloatingPointConstants<TSelf>, INumberBase<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IEqualityOperators<TSelf, TSelf, bool>, IIncrementOperators<TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf> where TSelf : IHyperbolicFunctions<TSelf>?
{
	static abstract TSelf Acosh(TSelf x);

	static abstract TSelf Asinh(TSelf x);

	static abstract TSelf Atanh(TSelf x);

	static abstract TSelf Cosh(TSelf x);

	static abstract TSelf Sinh(TSelf x);

	static abstract TSelf Tanh(TSelf x);
}
