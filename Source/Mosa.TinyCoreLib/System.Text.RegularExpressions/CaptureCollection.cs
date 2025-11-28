using System.Collections;
using System.Collections.Generic;

namespace System.Text.RegularExpressions;

public class CaptureCollection : ICollection<Capture>, IEnumerable<Capture>, IEnumerable, IList<Capture>, IReadOnlyCollection<Capture>, IReadOnlyList<Capture>, ICollection, IList
{
	public int Count
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

	public Capture this[int i]
	{
		get
		{
			throw null;
		}
	}

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	Capture IList<Capture>.this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	bool IList.IsFixedSize
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

	internal CaptureCollection()
	{
	}

	public void CopyTo(Array array, int arrayIndex)
	{
	}

	public void CopyTo(Capture[] array, int arrayIndex)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	void ICollection<Capture>.Add(Capture item)
	{
	}

	void ICollection<Capture>.Clear()
	{
	}

	bool ICollection<Capture>.Contains(Capture item)
	{
		throw null;
	}

	bool ICollection<Capture>.Remove(Capture item)
	{
		throw null;
	}

	IEnumerator<Capture> IEnumerable<Capture>.GetEnumerator()
	{
		throw null;
	}

	int IList<Capture>.IndexOf(Capture item)
	{
		throw null;
	}

	void IList<Capture>.Insert(int index, Capture item)
	{
	}

	void IList<Capture>.RemoveAt(int index)
	{
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
