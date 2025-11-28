using System;
using System.Collections;
using System.ComponentModel;

namespace Microsoft.VisualBasic;

public sealed class Collection : ICollection, IEnumerable, IList
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public object? this[int Index]
	{
		get
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public object? this[object Index]
	{
		get
		{
			throw null;
		}
	}

	public object? this[string Key]
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

	public void Add(object? Item, string? Key = null, object? Before = null, object? After = null)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(string Key)
	{
		throw null;
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public void Remove(int Index)
	{
	}

	public void Remove(string Key)
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
