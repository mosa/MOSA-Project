namespace System.ComponentModel;

public class RefreshEventArgs : EventArgs
{
	public object? ComponentChanged
	{
		get
		{
			throw null;
		}
	}

	public Type? TypeChanged
	{
		get
		{
			throw null;
		}
	}

	public RefreshEventArgs(object? componentChanged)
	{
	}

	public RefreshEventArgs(Type? typeChanged)
	{
	}
}
