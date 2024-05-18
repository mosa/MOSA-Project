namespace System.Numerics;

public interface IFloatingPointIeee754<TSelf> : IComparable, IComparable<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IComparisonOperators<TSelf, TSelf, bool>, IEqualityOperators<TSelf, TSelf, bool>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IExponentialFunctions<TSelf>, IFloatingPointConstants<TSelf>, INumberBase<TSelf>, IIncrementOperators<TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf>, IFloatingPoint<TSelf>, IModulusOperators<TSelf, TSelf, TSelf>, INumber<TSelf>, ISignedNumber<TSelf>, IHyperbolicFunctions<TSelf>, ILogarithmicFunctions<TSelf>, IPowerFunctions<TSelf>, IRootFunctions<TSelf>, ITrigonometricFunctions<TSelf> where TSelf : IFloatingPointIeee754<TSelf>?
{
	static abstract TSelf Epsilon { get; }

	static abstract TSelf NaN { get; }

	static abstract TSelf NegativeInfinity { get; }

	static abstract TSelf NegativeZero { get; }

	static abstract TSelf PositiveInfinity { get; }

	static abstract TSelf Atan2(TSelf y, TSelf x);

	static abstract TSelf Atan2Pi(TSelf y, TSelf x);

	static abstract TSelf BitDecrement(TSelf x);

	static abstract TSelf BitIncrement(TSelf x);

	static abstract TSelf FusedMultiplyAdd(TSelf left, TSelf right, TSelf addend);

	static abstract TSelf Ieee754Remainder(TSelf left, TSelf right);

	static abstract int ILogB(TSelf x);

	static virtual TSelf Lerp(TSelf value1, TSelf value2, TSelf amount)
	{
		throw null;
	}

	static virtual TSelf ReciprocalEstimate(TSelf x)
	{
		throw null;
	}

	static virtual TSelf ReciprocalSqrtEstimate(TSelf x)
	{
		throw null;
	}

	static abstract TSelf ScaleB(TSelf x, int n);
}
