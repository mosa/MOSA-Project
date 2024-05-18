using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Collections.Specialized;

public class OrderedDictionary : ICollection, IEnumerable, IDictionary, IOrderedDictionary, IDeserializationCallback, ISerializable
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

	public object? this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object? this[object key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ICollection Keys
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

	public ICollection Values
	{
		get
		{
			throw null;
		}
	}

	public OrderedDictionary()
	{
	}

	public OrderedDictionary(IEqualityComparer? comparer)
	{
	}

	public OrderedDictionary(int capacity)
	{
	}

	public OrderedDictionary(int capacity, IEqualityComparer? comparer)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected OrderedDictionary(SerializationInfo info, StreamingContext context)
	{
	}

	public void Add(object key, object? value)
	{
	}

	public OrderedDictionary AsReadOnly()
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool Contains(object key)
	{
		throw null;
	}

	public void CopyTo(Array array, int index)
	{
	}

	public virtual IDictionaryEnumerator GetEnumerator()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public void Insert(int index, object key, object? value)
	{
	}

	protected virtual void OnDeserialization(object? sender)
	{
	}

	public void Remove(object key)
	{
	}

	public void RemoveAt(int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	void IDeserializationCallback.OnDeserialization(object? sender)
	{
	}
}
