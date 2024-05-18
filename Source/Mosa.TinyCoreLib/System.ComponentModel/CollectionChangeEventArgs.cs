namespace System.ComponentModel;

public class CollectionChangeEventArgs : EventArgs
{
	public virtual CollectionChangeAction Action
	{
		get
		{
			throw null;
		}
	}

	public virtual object? Element
	{
		get
		{
			throw null;
		}
	}

	public CollectionChangeEventArgs(CollectionChangeAction action, object? element)
	{
	}
}
