namespace System.Threading;

public sealed class ThreadAbortException : SystemException
{
	public object? ExceptionState
	{
		get
		{
			throw null;
		}
	}

	internal ThreadAbortException()
	{
	}
}
