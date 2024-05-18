using System.Collections;
using System.Collections.Generic;

namespace System.DirectoryServices.AccountManagement;

public class PrincipalValueCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, ICollection, IList
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public bool IsSynchronized
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

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	int ICollection.Count
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

	object IList.this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal PrincipalValueCollection()
	{
	}

	public void Add(T value)
	{
	}

	public void Clear()
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

	public void Insert(int index, T value)
	{
	}

	public bool Remove(T value)
	{
		throw null;
	}

	public void RemoveAt(int index)
	{
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	int IList.Add(object value)
	{
		throw null;
	}

	void IList.Clear()
	{
	}

	bool IList.Contains(object value)
	{
		throw null;
	}

	int IList.IndexOf(object value)
	{
		throw null;
	}

	void IList.Insert(int index, object value)
	{
	}

	void IList.Remove(object value)
	{
	}

	void IList.RemoveAt(int index)
	{
	}
}
