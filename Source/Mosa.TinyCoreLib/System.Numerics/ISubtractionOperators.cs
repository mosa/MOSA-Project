namespace System.Numerics;

public interface ISubtractionOperators<TSelf, TOther, TResult> where TSelf : ISubtractionOperators<TSelf, TOther, TResult>?
{
	static virtual TResult operator checked -(TSelf left, TOther right)
	{
		throw null;
	}

	static abstract TResult operator -(TSelf left, TOther right);
}
