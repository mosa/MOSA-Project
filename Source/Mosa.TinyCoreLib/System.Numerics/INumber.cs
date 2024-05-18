namespace System.Numerics;

public interface INumber<TSelf> : IComparable, IComparable<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IComparisonOperators<TSelf, TSelf, bool>, IEqualityOperators<TSelf, TSelf, bool>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IIncrementOperators<TSelf>, IModulusOperators<TSelf, TSelf, TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, INumberBase<TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf> where TSelf : INumber<TSelf>?
{
	static virtual TSelf Clamp(TSelf value, TSelf min, TSelf max)
	{
		throw null;
	}

	static virtual TSelf CopySign(TSelf value, TSelf sign)
	{
		throw null;
	}

	static virtual TSelf Max(TSelf x, TSelf y)
	{
		throw null;
	}

	static virtual TSelf MaxNumber(TSelf x, TSelf y)
	{
		throw null;
	}

	static virtual TSelf Min(TSelf x, TSelf y)
	{
		throw null;
	}

	static virtual TSelf MinNumber(TSelf x, TSelf y)
	{
		throw null;
	}

	static virtual int Sign(TSelf value)
	{
		throw null;
	}
}
