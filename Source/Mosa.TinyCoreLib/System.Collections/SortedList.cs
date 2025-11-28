namespace System.Collections;

public class SortedList : ICollection, IEnumerable, IDictionary, ICloneable
{
	public virtual int Capacity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public virtual object? this[object key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual ICollection Keys
	{
		get
		{
			throw null;
		}
	}

	public virtual object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public virtual ICollection Values
	{
		get
		{
			throw null;
		}
	}

	public SortedList()
	{
	}

	public SortedList(IComparer? comparer)
	{
	}

	public SortedList(IComparer? comparer, int capacity)
	{
	}

	public SortedList(IDictionary d)
	{
	}

	public SortedList(IDictionary d, IComparer? comparer)
	{
	}

	public SortedList(int initialCapacity)
	{
	}

	public virtual void Add(object key, object? value)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual object Clone()
	{
		throw null;
	}

	public virtual bool Contains(object key)
	{
		throw null;
	}

	public virtual bool ContainsKey(object key)
	{
		throw null;
	}

	public virtual bool ContainsValue(object? value)
	{
		throw null;
	}

	public virtual void CopyTo(Array array, int arrayIndex)
	{
	}

	public virtual object? GetByIndex(int index)
	{
		throw null;
	}

	public virtual IDictionaryEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual object GetKey(int index)
	{
		throw null;
	}

	public virtual IList GetKeyList()
	{
		throw null;
	}

	public virtual IList GetValueList()
	{
		throw null;
	}

	public virtual int IndexOfKey(object key)
	{
		throw null;
	}

	public virtual int IndexOfValue(object? value)
	{
		throw null;
	}

	public virtual void Remove(object key)
	{
	}

	public virtual void RemoveAt(int index)
	{
	}

	public virtual void SetByIndex(int index, object? value)
	{
	}

	public static SortedList Synchronized(SortedList list)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public virtual void TrimToSize()
	{
	}
}
