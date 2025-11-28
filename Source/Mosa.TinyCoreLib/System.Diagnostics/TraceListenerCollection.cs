using System.Collections;

namespace System.Diagnostics;

public class TraceListenerCollection : ICollection, IEnumerable, IList
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public TraceListener this[int i]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TraceListener? this[string name]
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

	internal TraceListenerCollection()
	{
	}

	public int Add(TraceListener listener)
	{
		throw null;
	}

	public void AddRange(TraceListenerCollection value)
	{
	}

	public void AddRange(TraceListener[] value)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(TraceListener? listener)
	{
		throw null;
	}

	public void CopyTo(TraceListener[] listeners, int index)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(TraceListener? listener)
	{
		throw null;
	}

	public void Insert(int index, TraceListener listener)
	{
	}

	public void Remove(TraceListener? listener)
	{
	}

	public void Remove(string name)
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
