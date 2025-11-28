namespace System.ComponentModel;

public class DoWorkEventArgs : CancelEventArgs
{
	public object? Argument
	{
		get
		{
			throw null;
		}
	}

	public object? Result
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DoWorkEventArgs(object? argument)
	{
	}
}
