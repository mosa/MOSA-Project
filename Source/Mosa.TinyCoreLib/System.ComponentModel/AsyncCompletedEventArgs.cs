namespace System.ComponentModel;

public class AsyncCompletedEventArgs : EventArgs
{
	public bool Cancelled
	{
		get
		{
			throw null;
		}
	}

	public Exception? Error
	{
		get
		{
			throw null;
		}
	}

	public object? UserState
	{
		get
		{
			throw null;
		}
	}

	public AsyncCompletedEventArgs(Exception? error, bool cancelled, object? userState)
	{
	}

	protected void RaiseExceptionIfNecessary()
	{
	}
}
