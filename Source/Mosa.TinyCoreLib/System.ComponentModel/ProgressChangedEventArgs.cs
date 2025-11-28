namespace System.ComponentModel;

public class ProgressChangedEventArgs : EventArgs
{
	public int ProgressPercentage
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

	public ProgressChangedEventArgs(int progressPercentage, object? userState)
	{
	}
}
