namespace System.Numerics;

public interface IMinMaxValue<TSelf> where TSelf : IMinMaxValue<TSelf>?
{
	static abstract TSelf MaxValue { get; }

	static abstract TSelf MinValue { get; }
}
