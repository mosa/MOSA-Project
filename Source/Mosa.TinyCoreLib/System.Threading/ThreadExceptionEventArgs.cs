namespace System.Threading;

public class ThreadExceptionEventArgs : EventArgs
{
	public Exception Exception
	{
		get
		{
			throw null;
		}
	}

	public ThreadExceptionEventArgs(Exception t)
	{
	}
}
