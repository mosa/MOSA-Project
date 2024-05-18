namespace System.Numerics;

public interface IAdditionOperators<TSelf, TOther, TResult> where TSelf : IAdditionOperators<TSelf, TOther, TResult>?
{
	static abstract TResult operator +(TSelf left, TOther right);

	static virtual TResult operator checked +(TSelf left, TOther right)
	{
		throw null;
	}
}
