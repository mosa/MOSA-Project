using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Collections;

public class Hashtable : ICollection, IEnumerable, IDictionary, ICloneable, IDeserializationCallback, ISerializable
{
	[Obsolete("Hashtable.comparer has been deprecated. Use the KeyComparer properties instead.")]
	protected IComparer? comparer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	protected IEqualityComparer? EqualityComparer
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Hashtable.hcp has been deprecated. Use the EqualityComparer property instead.")]
	protected IHashCodeProvider? hcp
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public virtual object? this[object key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual ICollection Keys
	{
		get
		{
			throw null;
		}
	}

	public virtual object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public virtual ICollection Values
	{
		get
		{
			throw null;
		}
	}

	public Hashtable()
	{
	}

	public Hashtable(IDictionary d)
	{
	}

	public Hashtable(IDictionary d, IEqualityComparer? equalityComparer)
	{
	}

	[Obsolete("This constructor has been deprecated. Use Hashtable(IDictionary, IEqualityComparer) instead.")]
	public Hashtable(IDictionary d, IHashCodeProvider? hcp, IComparer? comparer)
	{
	}

	public Hashtable(IDictionary d, float loadFactor)
	{
	}

	public Hashtable(IDictionary d, float loadFactor, IEqualityComparer? equalityComparer)
	{
	}

	[Obsolete("This constructor has been deprecated. Use Hashtable(IDictionary, float, IEqualityComparer) instead.")]
	public Hashtable(IDictionary d, float loadFactor, IHashCodeProvider? hcp, IComparer? comparer)
	{
	}

	public Hashtable(IEqualityComparer? equalityComparer)
	{
	}

	[Obsolete("This constructor has been deprecated. Use Hashtable(IEqualityComparer) instead.")]
	public Hashtable(IHashCodeProvider? hcp, IComparer? comparer)
	{
	}

	public Hashtable(int capacity)
	{
	}

	public Hashtable(int capacity, IEqualityComparer? equalityComparer)
	{
	}

	[Obsolete("This constructor has been deprecated. Use Hashtable(int, IEqualityComparer) instead.")]
	public Hashtable(int capacity, IHashCodeProvider? hcp, IComparer? comparer)
	{
	}

	public Hashtable(int capacity, float loadFactor)
	{
	}

	public Hashtable(int capacity, float loadFactor, IEqualityComparer? equalityComparer)
	{
	}

	[Obsolete("This constructor has been deprecated. Use Hashtable(int, float, IEqualityComparer) instead.")]
	public Hashtable(int capacity, float loadFactor, IHashCodeProvider? hcp, IComparer? comparer)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected Hashtable(SerializationInfo info, StreamingContext context)
	{
	}

	public virtual void Add(object key, object? value)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual object Clone()
	{
		throw null;
	}

	public virtual bool Contains(object key)
	{
		throw null;
	}

	public virtual bool ContainsKey(object key)
	{
		throw null;
	}

	public virtual bool ContainsValue(object? value)
	{
		throw null;
	}

	public virtual void CopyTo(Array array, int arrayIndex)
	{
	}

	public virtual IDictionaryEnumerator GetEnumerator()
	{
		throw null;
	}

	protected virtual int GetHash(object key)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	protected virtual bool KeyEquals(object? item, object key)
	{
		throw null;
	}

	public virtual void OnDeserialization(object? sender)
	{
	}

	public virtual void Remove(object key)
	{
	}

	public static Hashtable Synchronized(Hashtable table)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
