namespace System.Runtime.ExceptionServices;

public class FirstChanceExceptionEventArgs : EventArgs
{
	public Exception Exception
	{
		get
		{
			throw null;
		}
	}

	public FirstChanceExceptionEventArgs(Exception exception)
	{
	}
}
