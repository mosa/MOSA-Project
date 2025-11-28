using System.Collections;

namespace System.DirectoryServices;

public class SchemaNameCollection : ICollection, IEnumerable, IList
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public string? this[int index]
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

	internal SchemaNameCollection()
	{
	}

	public int Add(string? value)
	{
		throw null;
	}

	public void AddRange(SchemaNameCollection value)
	{
	}

	public void AddRange(string?[] value)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(string? value)
	{
		throw null;
	}

	public void CopyTo(string?[] stringArray, int index)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(string? value)
	{
		throw null;
	}

	public void Insert(int index, string? value)
	{
	}

	public void Remove(string? value)
	{
	}

	public void RemoveAt(int index)
	{
	}

	void ICollection.CopyTo(Array? array, int index)
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
