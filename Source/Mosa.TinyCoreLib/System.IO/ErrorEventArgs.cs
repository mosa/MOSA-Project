namespace System.IO;

public class ErrorEventArgs : EventArgs
{
	public ErrorEventArgs(Exception exception)
	{
	}

	public virtual Exception GetException()
	{
		throw null;
	}
}
