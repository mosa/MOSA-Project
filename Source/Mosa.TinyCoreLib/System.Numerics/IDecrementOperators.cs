namespace System.Numerics;

public interface IDecrementOperators<TSelf> where TSelf : IDecrementOperators<TSelf>?
{
	static virtual TSelf operator checked --(TSelf value)
	{
		throw null;
	}

	static abstract TSelf operator --(TSelf value);
}
