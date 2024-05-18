using System.Collections.Generic;

namespace System.Collections.ObjectModel;

public class ReadOnlyCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public static ReadOnlyCollection<T> Empty
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

	T IList<T>.this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
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

	public ReadOnlyCollection(IList<T> list)
	{
	}

	public bool Contains(T value)
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

	public int IndexOf(T value)
	{
		throw null;
	}

	void ICollection<T>.Add(T value)
	{
	}

	void ICollection<T>.Clear()
	{
	}

	bool ICollection<T>.Remove(T value)
	{
		throw null;
	}

	void IList<T>.Insert(int index, T value)
	{
	}

	void IList<T>.RemoveAt(int index)
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

	void IList.Clear()
	{
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

	void IList.RemoveAt(int index)
	{
	}
}
