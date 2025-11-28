namespace System.Numerics;

public interface IAdditiveIdentity<TSelf, TResult> where TSelf : IAdditiveIdentity<TSelf, TResult>?
{
	static abstract TResult AdditiveIdentity { get; }
}
