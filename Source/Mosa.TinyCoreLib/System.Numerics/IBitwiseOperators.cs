namespace System.Numerics;

public interface IBitwiseOperators<TSelf, TOther, TResult> where TSelf : IBitwiseOperators<TSelf, TOther, TResult>?
{
	static abstract TResult operator &(TSelf left, TOther right);

	static abstract TResult operator |(TSelf left, TOther right);

	static abstract TResult operator ^(TSelf left, TOther right);

	static abstract TResult operator ~(TSelf value);
}
