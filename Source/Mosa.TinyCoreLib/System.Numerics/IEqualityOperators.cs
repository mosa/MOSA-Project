namespace System.Numerics;

public interface IEqualityOperators<TSelf, TOther, TResult> where TSelf : IEqualityOperators<TSelf, TOther, TResult>?
{
	static abstract TResult operator ==(TSelf? left, TOther? right);

	static abstract TResult operator !=(TSelf? left, TOther? right);
}
