namespace System.ComponentModel.Design.Serialization;

public class ResolveNameEventArgs : EventArgs
{
	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ResolveNameEventArgs(string? name)
	{
	}
}
