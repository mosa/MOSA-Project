namespace System.ComponentModel;

public class CancelEventArgs : EventArgs
{
	public bool Cancel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CancelEventArgs()
	{
	}

	public CancelEventArgs(bool cancel)
	{
	}
}
