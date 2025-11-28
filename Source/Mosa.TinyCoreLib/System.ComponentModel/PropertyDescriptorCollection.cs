using System.Collections;

namespace System.ComponentModel;

public class PropertyDescriptorCollection : ICollection, IEnumerable, IDictionary, IList
{
	public static readonly PropertyDescriptorCollection Empty;

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public virtual PropertyDescriptor this[int index]
	{
		get
		{
			throw null;
		}
	}

	public virtual PropertyDescriptor? this[string name]
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

	ICollection IDictionary.Values
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

	public PropertyDescriptorCollection(PropertyDescriptor[]? properties)
	{
	}

	public PropertyDescriptorCollection(PropertyDescriptor[]? properties, bool readOnly)
	{
	}

	public int Add(PropertyDescriptor value)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool Contains(PropertyDescriptor value)
	{
		throw null;
	}

	public void CopyTo(Array array, int index)
	{
	}

	public virtual PropertyDescriptor? Find(string name, bool ignoreCase)
	{
		throw null;
	}

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(PropertyDescriptor? value)
	{
		throw null;
	}

	public void Insert(int index, PropertyDescriptor value)
	{
	}

	protected void InternalSort(IComparer? sorter)
	{
	}

	protected void InternalSort(string[]? names)
	{
	}

	public void Remove(PropertyDescriptor? value)
	{
	}

	public void RemoveAt(int index)
	{
	}

	public virtual PropertyDescriptorCollection Sort()
	{
		throw null;
	}

	public virtual PropertyDescriptorCollection Sort(IComparer? comparer)
	{
		throw null;
	}

	public virtual PropertyDescriptorCollection Sort(string[]? names)
	{
		throw null;
	}

	public virtual PropertyDescriptorCollection Sort(string[]? names, IComparer? comparer)
	{
		throw null;
	}

	void IDictionary.Add(object key, object? value)
	{
	}

	void IDictionary.Clear()
	{
	}

	bool IDictionary.Contains(object key)
	{
		throw null;
	}

	IDictionaryEnumerator IDictionary.GetEnumerator()
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
