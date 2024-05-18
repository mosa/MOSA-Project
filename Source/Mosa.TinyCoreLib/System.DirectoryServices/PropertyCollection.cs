using System.Collections;

namespace System.DirectoryServices;

public class PropertyCollection : ICollection, IEnumerable, IDictionary
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public PropertyValueCollection this[string propertyName]
	{
		get
		{
			throw null;
		}
	}

	public ICollection PropertyNames
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

	bool IDictionary.IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	bool IDictionary.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	object? IDictionary.this[object key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	ICollection IDictionary.Keys
	{
		get
		{
			throw null;
		}
	}

	public ICollection Values
	{
		get
		{
			throw null;
		}
	}

	internal PropertyCollection()
	{
	}

	public bool Contains(string propertyName)
	{
		throw null;
	}

	public void CopyTo(PropertyValueCollection[] array, int index)
	{
	}

	public IDictionaryEnumerator GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	void IDictionary.Add(object key, object? value)
	{
	}

	void IDictionary.Clear()
	{
	}

	bool IDictionary.Contains(object value)
	{
		throw null;
	}

	void IDictionary.Remove(object key)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
