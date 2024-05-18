namespace System.Numerics;

public interface IUnaryPlusOperators<TSelf, TResult> where TSelf : IUnaryPlusOperators<TSelf, TResult>?
{
	static abstract TResult operator +(TSelf value);
}
