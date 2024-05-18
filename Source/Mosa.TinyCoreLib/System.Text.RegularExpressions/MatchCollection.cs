using System.Collections;
using System.Collections.Generic;

namespace System.Text.RegularExpressions;

public class MatchCollection : ICollection<Match>, IEnumerable<Match>, IEnumerable, IList<Match>, IReadOnlyCollection<Match>, IReadOnlyList<Match>, ICollection, IList
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

	public virtual Match this[int i]
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

	Match IList<Match>.this[int index]
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

	internal MatchCollection()
	{
	}

	public void CopyTo(Array array, int arrayIndex)
	{
	}

	public void CopyTo(Match[] array, int arrayIndex)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	void ICollection<Match>.Add(Match item)
	{
	}

	void ICollection<Match>.Clear()
	{
	}

	bool ICollection<Match>.Contains(Match item)
	{
		throw null;
	}

	bool ICollection<Match>.Remove(Match item)
	{
		throw null;
	}

	IEnumerator<Match> IEnumerable<Match>.GetEnumerator()
	{
		throw null;
	}

	int IList<Match>.IndexOf(Match item)
	{
		throw null;
	}

	void IList<Match>.Insert(int index, Match item)
	{
	}

	void IList<Match>.RemoveAt(int index)
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
