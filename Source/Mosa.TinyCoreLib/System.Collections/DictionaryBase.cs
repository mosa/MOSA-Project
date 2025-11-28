namespace System.Collections;

public abstract class DictionaryBase : ICollection, IEnumerable, IDictionary
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	protected IDictionary Dictionary
	{
		get
		{
			throw null;
		}
	}

	protected Hashtable InnerHashtable
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

	bool IDictionary.IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	bool IDictionary.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	object? IDictionary.this[object key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	ICollection IDictionary.Keys
	{
		get
		{
			throw null;
		}
	}

	ICollection IDictionary.Values
	{
		get
		{
			throw null;
		}
	}

	public void Clear()
	{
	}

	public void CopyTo(Array array, int index)
	{
	}

	public IDictionaryEnumerator GetEnumerator()
	{
		throw null;
	}

	protected virtual void OnClear()
	{
	}

	protected virtual void OnClearComplete()
	{
	}

	protected virtual object? OnGet(object key, object? currentValue)
	{
		throw null;
	}

	protected virtual void OnInsert(object key, object? value)
	{
	}

	protected virtual void OnInsertComplete(object key, object? value)
	{
	}

	protected virtual void OnRemove(object key, object? value)
	{
	}

	protected virtual void OnRemoveComplete(object key, object? value)
	{
	}

	protected virtual void OnSet(object key, object? oldValue, object? newValue)
	{
	}

	protected virtual void OnSetComplete(object key, object? oldValue, object? newValue)
	{
	}

	protected virtual void OnValidate(object key, object? value)
	{
	}

	void IDictionary.Add(object key, object? value)
	{
	}

	bool IDictionary.Contains(object key)
	{
		throw null;
	}

	void IDictionary.Remove(object key)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
