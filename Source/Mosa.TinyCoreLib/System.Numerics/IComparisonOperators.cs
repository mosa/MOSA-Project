namespace System.Numerics;

public interface IComparisonOperators<TSelf, TOther, TResult> : IEqualityOperators<TSelf, TOther, TResult> where TSelf : IComparisonOperators<TSelf, TOther, TResult>?
{
	static abstract TResult operator >(TSelf left, TOther right);

	static abstract TResult operator >=(TSelf left, TOther right);

	static abstract TResult operator <(TSelf left, TOther right);

	static abstract TResult operator <=(TSelf left, TOther right);
}
