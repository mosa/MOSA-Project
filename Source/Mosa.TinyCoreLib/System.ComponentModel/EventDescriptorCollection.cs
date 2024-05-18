using System.Collections;

namespace System.ComponentModel;

public class EventDescriptorCollection : ICollection, IEnumerable, IList
{
	public static readonly EventDescriptorCollection Empty;

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public virtual EventDescriptor? this[int index]
	{
		get
		{
			throw null;
		}
	}

	public virtual EventDescriptor? this[string name]
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

	public EventDescriptorCollection(EventDescriptor[]? events)
	{
	}

	public EventDescriptorCollection(EventDescriptor[]? events, bool readOnly)
	{
	}

	public int Add(EventDescriptor? value)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool Contains(EventDescriptor? value)
	{
		throw null;
	}

	public virtual EventDescriptor? Find(string name, bool ignoreCase)
	{
		throw null;
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(EventDescriptor? value)
	{
		throw null;
	}

	public void Insert(int index, EventDescriptor? value)
	{
	}

	protected void InternalSort(IComparer? sorter)
	{
	}

	protected void InternalSort(string[]? names)
	{
	}

	public void Remove(EventDescriptor? value)
	{
	}

	public void RemoveAt(int index)
	{
	}

	public virtual EventDescriptorCollection Sort()
	{
		throw null;
	}

	public virtual EventDescriptorCollection Sort(IComparer comparer)
	{
		throw null;
	}

	public virtual EventDescriptorCollection Sort(string[] names)
	{
		throw null;
	}

	public virtual EventDescriptorCollection Sort(string[] names, IComparer comparer)
	{
		throw null;
	}

	void ICollection.CopyTo(Array? array, int index)
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
