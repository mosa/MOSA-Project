namespace System.ComponentModel;

public class HandledEventArgs : EventArgs
{
	public bool Handled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HandledEventArgs()
	{
	}

	public HandledEventArgs(bool defaultHandledValue)
	{
	}
}
