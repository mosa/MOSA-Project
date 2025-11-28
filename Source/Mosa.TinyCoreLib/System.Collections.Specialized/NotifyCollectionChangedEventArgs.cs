namespace System.Collections.Specialized;

public class NotifyCollectionChangedEventArgs : EventArgs
{
	public NotifyCollectionChangedAction Action
	{
		get
		{
			throw null;
		}
	}

	public IList? NewItems
	{
		get
		{
			throw null;
		}
	}

	public int NewStartingIndex
	{
		get
		{
			throw null;
		}
	}

	public IList? OldItems
	{
		get
		{
			throw null;
		}
	}

	public int OldStartingIndex
	{
		get
		{
			throw null;
		}
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
	{
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList? changedItems)
	{
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
	{
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
	{
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList? changedItems, int startingIndex)
	{
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList? changedItems, int index, int oldIndex)
	{
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object? changedItem)
	{
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object? changedItem, int index)
	{
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object? changedItem, int index, int oldIndex)
	{
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object? newItem, object? oldItem)
	{
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object? newItem, object? oldItem, int index)
	{
	}
}
