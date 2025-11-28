namespace System.Numerics;

public interface ITrigonometricFunctions<TSelf> : IFloatingPointConstants<TSelf>, INumberBase<TSelf>, IEquatable<TSelf>, IFormattable, IParsable<TSelf>, ISpanFormattable, ISpanParsable<TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>, IAdditiveIdentity<TSelf, TSelf>, IDecrementOperators<TSelf>, IDivisionOperators<TSelf, TSelf, TSelf>, IEqualityOperators<TSelf, TSelf, bool>, IIncrementOperators<TSelf>, IMultiplicativeIdentity<TSelf, TSelf>, IMultiplyOperators<TSelf, TSelf, TSelf>, ISubtractionOperators<TSelf, TSelf, TSelf>, IUnaryNegationOperators<TSelf, TSelf>, IUnaryPlusOperators<TSelf, TSelf>, IUtf8SpanFormattable, IUtf8SpanParsable<TSelf> where TSelf : ITrigonometricFunctions<TSelf>?
{
	static abstract TSelf Acos(TSelf x);

	static abstract TSelf AcosPi(TSelf x);

	static abstract TSelf Asin(TSelf x);

	static abstract TSelf AsinPi(TSelf x);

	static abstract TSelf Atan(TSelf x);

	static abstract TSelf AtanPi(TSelf x);

	static abstract TSelf Cos(TSelf x);

	static abstract TSelf CosPi(TSelf x);

	static virtual TSelf DegreesToRadians(TSelf degrees)
	{
		throw null;
	}

	static virtual TSelf RadiansToDegrees(TSelf radians)
	{
		throw null;
	}

	static abstract TSelf Sin(TSelf x);

	static abstract (TSelf Sin, TSelf Cos) SinCos(TSelf x);

	static abstract (TSelf SinPi, TSelf CosPi) SinCosPi(TSelf x);

	static abstract TSelf SinPi(TSelf x);

	static abstract TSelf Tan(TSelf x);

	static abstract TSelf TanPi(TSelf x);
}
