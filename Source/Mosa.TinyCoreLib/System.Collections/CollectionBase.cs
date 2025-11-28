namespace System.Collections;

public abstract class CollectionBase : ICollection, IEnumerable, IList
{
	public int Capacity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	protected ArrayList InnerList
	{
		get
		{
			throw null;
		}
	}

	protected IList List
	{
		get
		{
			throw null;
		}
	}

	bool ICollection.IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	object ICollection.SyncRoot
	{
		get
		{
			throw null;
		}
	}

	bool IList.IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	bool IList.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	object? IList.this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected CollectionBase()
	{
	}

	protected CollectionBase(int capacity)
	{
	}

	public void Clear()
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	protected virtual void OnClear()
	{
	}

	protected virtual void OnClearComplete()
	{
	}

	protected virtual void OnInsert(int index, object? value)
	{
	}

	protected virtual void OnInsertComplete(int index, object? value)
	{
	}

	protected virtual void OnRemove(int index, object? value)
	{
	}

	protected virtual void OnRemoveComplete(int index, object? value)
	{
	}

	protected virtual void OnSet(int index, object? oldValue, object? newValue)
	{
	}

	protected virtual void OnSetComplete(int index, object? oldValue, object? newValue)
	{
	}

	protected virtual void OnValidate(object value)
	{
	}

	public void RemoveAt(int index)
	{
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	int IList.Add(object? value)
	{
		throw null;
	}

	bool IList.Contains(object? value)
	{
		throw null;
	}

	int IList.IndexOf(object? value)
	{
		throw null;
	}

	void IList.Insert(int index, object? value)
	{
	}

	void IList.Remove(object? value)
	{
	}
}
