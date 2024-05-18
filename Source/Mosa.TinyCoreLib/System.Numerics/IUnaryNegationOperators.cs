namespace System.Numerics;

public interface IUnaryNegationOperators<TSelf, TResult> where TSelf : IUnaryNegationOperators<TSelf, TResult>?
{
	static virtual TResult operator checked -(TSelf value)
	{
		throw null;
	}

	static abstract TResult operator -(TSelf value);
}
