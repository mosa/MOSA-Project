namespace System.ComponentModel;

public class AddingNewEventArgs : EventArgs
{
	public object? NewObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AddingNewEventArgs()
	{
	}

	public AddingNewEventArgs(object? newObject)
	{
	}
}
