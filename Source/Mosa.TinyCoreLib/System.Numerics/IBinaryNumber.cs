namespace System.Numerics;

public interface IBinaryNumber<TSelf> : IComparable, IComparable<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IBitwiseOperators<TSelf, TSelf, TSelf>, IComparisonOperators<TSelf, TSelf, bool>, IEqualityOperators<TSelf, TSelf, bool>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IIncrementOperators<TSelf>, IModulusOperators<TSelf, TSelf, TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, INumber<TSelf>, INumberBase<TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf> where TSelf : IBinaryNumber<TSelf>?
{
	static virtual TSelf AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	static abstract bool IsPow2(TSelf value);

	static abstract TSelf Log2(TSelf value);
}
