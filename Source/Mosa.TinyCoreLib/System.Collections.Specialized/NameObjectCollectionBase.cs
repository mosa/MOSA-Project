using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Collections.Specialized;

public abstract class NameObjectCollectionBase : ICollection, IEnumerable, IDeserializationCallback, ISerializable
{
	public class KeysCollection : ICollection, IEnumerable
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

		internal KeysCollection()
		{
		}

		public virtual string? Get(int index)
		{
			throw null;
		}

		public IEnumerator GetEnumerator()
		{
			throw null;
		}

		void ICollection.CopyTo(Array array, int index)
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

	protected bool IsReadOnly
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual KeysCollection Keys
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

	protected NameObjectCollectionBase()
	{
	}

	protected NameObjectCollectionBase(IEqualityComparer? equalityComparer)
	{
	}

	[Obsolete("This constructor has been deprecated. Use NameObjectCollectionBase(IEqualityComparer) instead.")]
	protected NameObjectCollectionBase(IHashCodeProvider? hashProvider, IComparer? comparer)
	{
	}

	protected NameObjectCollectionBase(int capacity)
	{
	}

	protected NameObjectCollectionBase(int capacity, IEqualityComparer? equalityComparer)
	{
	}

	[Obsolete("This constructor has been deprecated. Use NameObjectCollectionBase(Int32, IEqualityComparer) instead.")]
	protected NameObjectCollectionBase(int capacity, IHashCodeProvider? hashProvider, IComparer? comparer)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected NameObjectCollectionBase(SerializationInfo info, StreamingContext context)
	{
	}

	protected void BaseAdd(string? name, object? value)
	{
	}

	protected void BaseClear()
	{
	}

	protected object? BaseGet(int index)
	{
		throw null;
	}

	protected object? BaseGet(string? name)
	{
		throw null;
	}

	protected string?[] BaseGetAllKeys()
	{
		throw null;
	}

	protected object?[] BaseGetAllValues()
	{
		throw null;
	}

	protected object?[] BaseGetAllValues(Type type)
	{
		throw null;
	}

	protected string? BaseGetKey(int index)
	{
		throw null;
	}

	protected bool BaseHasKeys()
	{
		throw null;
	}

	protected void BaseRemove(string? name)
	{
	}

	protected void BaseRemoveAt(int index)
	{
	}

	protected void BaseSet(int index, object? value)
	{
	}

	protected void BaseSet(string? name, object? value)
	{
	}

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public virtual void OnDeserialization(object? sender)
	{
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}
}
