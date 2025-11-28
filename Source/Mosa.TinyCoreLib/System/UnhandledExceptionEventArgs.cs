namespace System;

public class UnhandledExceptionEventArgs : EventArgs
{
	public object ExceptionObject
	{
		get
		{
			throw null;
		}
	}

	public bool IsTerminating
	{
		get
		{
			throw null;
		}
	}

	public UnhandledExceptionEventArgs(object exception, bool isTerminating)
	{
	}
}
