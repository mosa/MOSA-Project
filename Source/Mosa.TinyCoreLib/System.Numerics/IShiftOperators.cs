namespace System.Numerics;

public interface IShiftOperators<TSelf, TOther, TResult> where TSelf : IShiftOperators<TSelf, TOther, TResult>?
{
	static abstract TResult operator <<(TSelf value, TOther shiftAmount);

	static abstract TResult operator >>(TSelf value, TOther shiftAmount);

	static abstract TResult operator >>>(TSelf value, TOther shiftAmount);
}
