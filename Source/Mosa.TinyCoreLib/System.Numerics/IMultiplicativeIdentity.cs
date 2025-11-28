namespace System.Numerics;

public interface IMultiplicativeIdentity<TSelf, TResult> where TSelf : IMultiplicativeIdentity<TSelf, TResult>?
{
	static abstract TResult MultiplicativeIdentity { get; }
}
