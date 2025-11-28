namespace System.Numerics;

public interface IPowerFunctions<TSelf> : INumberBase<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IEqualityOperators<TSelf, TSelf, bool>, IIncrementOperators<TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf> where TSelf : IPowerFunctions<TSelf>?
{
	static abstract TSelf Pow(TSelf x, TSelf y);
}
