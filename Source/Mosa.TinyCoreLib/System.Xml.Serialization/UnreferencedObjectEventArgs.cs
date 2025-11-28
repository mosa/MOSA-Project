namespace System.Xml.Serialization;

public class UnreferencedObjectEventArgs : EventArgs
{
	public string? UnreferencedId
	{
		get
		{
			throw null;
		}
	}

	public object? UnreferencedObject
	{
		get
		{
			throw null;
		}
	}

	public UnreferencedObjectEventArgs(object? o, string? id)
	{
	}
}
