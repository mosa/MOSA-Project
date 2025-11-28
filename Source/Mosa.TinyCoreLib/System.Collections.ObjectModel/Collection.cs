using System.Collections.Generic;

namespace System.Collections.ObjectModel;

public class Collection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public T this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected IList<T> Items
	{
		get
		{
			throw null;
		}
	}

	bool ICollection<T>.IsReadOnly
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

	public Collection()
	{
	}

	public Collection(IList<T> list)
	{
	}

	public void Add(T item)
	{
	}

	public void Clear()
	{
	}

	protected virtual void ClearItems()
	{
	}

	public bool Contains(T item)
	{
		throw null;
	}

	public void CopyTo(T[] array, int index)
	{
	}

	public IEnumerator<T> GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(T item)
	{
		throw null;
	}

	public void Insert(int index, T item)
	{
	}

	protected virtual void InsertItem(int index, T item)
	{
	}

	public bool Remove(T item)
	{
		throw null;
	}

	public void RemoveAt(int index)
	{
	}

	protected virtual void RemoveItem(int index)
	{
	}

	protected virtual void SetItem(int index, T item)
	{
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
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
