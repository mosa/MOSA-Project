using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Runtime.CompilerServices;

public sealed class ReadOnlyCollectionBuilder<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, ICollection, IList
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

	public ReadOnlyCollectionBuilder()
	{
	}

	public ReadOnlyCollectionBuilder(IEnumerable<T> collection)
	{
	}

	public ReadOnlyCollectionBuilder(int capacity)
	{
	}

	public void Add(T item)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(T item)
	{
		throw null;
	}

	public void CopyTo(T[] array, int arrayIndex)
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

	public bool Remove(T item)
	{
		throw null;
	}

	public void RemoveAt(int index)
	{
	}

	public void Reverse()
	{
	}

	public void Reverse(int index, int count)
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

	public T[] ToArray()
	{
		throw null;
	}

	public ReadOnlyCollection<T> ToReadOnlyCollection()
	{
		throw null;
	}
}
