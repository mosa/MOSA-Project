namespace System.Numerics;

public interface IIncrementOperators<TSelf> where TSelf : IIncrementOperators<TSelf>?
{
	static virtual TSelf operator checked ++(TSelf value)
	{
		throw null;
	}

	static abstract TSelf operator ++(TSelf value);
}
