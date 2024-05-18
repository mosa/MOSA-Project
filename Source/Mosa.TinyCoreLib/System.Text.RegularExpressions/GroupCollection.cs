using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Text.RegularExpressions;

public class GroupCollection : ICollection<Group>, IEnumerable<Group>, IEnumerable, IEnumerable<KeyValuePair<string, Group>>, IList<Group>, IReadOnlyCollection<KeyValuePair<string, Group>>, IReadOnlyCollection<Group>, IReadOnlyDictionary<string, Group>, IReadOnlyList<Group>, ICollection, IList
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

	public Group this[int groupnum]
	{
		get
		{
			throw null;
		}
	}

	public Group this[string groupname]
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<string> Keys
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

	Group IList<Group>.this[int index]
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

	public IEnumerable<Group> Values
	{
		get
		{
			throw null;
		}
	}

	internal GroupCollection()
	{
	}

	public bool ContainsKey(string key)
	{
		throw null;
	}

	public void CopyTo(Array array, int arrayIndex)
	{
	}

	public void CopyTo(Group[] array, int arrayIndex)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	void ICollection<Group>.Add(Group item)
	{
	}

	void ICollection<Group>.Clear()
	{
	}

	bool ICollection<Group>.Contains(Group item)
	{
		throw null;
	}

	bool ICollection<Group>.Remove(Group item)
	{
		throw null;
	}

	IEnumerator<KeyValuePair<string, Group>> IEnumerable<KeyValuePair<string, Group>>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator<Group> IEnumerable<Group>.GetEnumerator()
	{
		throw null;
	}

	int IList<Group>.IndexOf(Group item)
	{
		throw null;
	}

	void IList<Group>.Insert(int index, Group item)
	{
	}

	void IList<Group>.RemoveAt(int index)
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

	public bool TryGetValue(string key, [NotNullWhen(true)] out Group? value)
	{
		throw null;
	}
}
