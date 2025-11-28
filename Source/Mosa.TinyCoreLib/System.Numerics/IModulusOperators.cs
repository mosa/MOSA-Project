namespace System.Numerics;

public interface IModulusOperators<TSelf, TOther, TResult> where TSelf : IModulusOperators<TSelf, TOther, TResult>?
{
	static abstract TResult operator %(TSelf left, TOther right);
}
