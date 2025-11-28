using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Collections.Specialized;

public class NameValueCollection : NameObjectCollectionBase
{
	public virtual string?[] AllKeys
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

	public string? this[string? name]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public NameValueCollection()
	{
	}

	public NameValueCollection(IEqualityComparer? equalityComparer)
	{
	}

	[Obsolete("This constructor has been deprecated. Use NameValueCollection(IEqualityComparer) instead.")]
	public NameValueCollection(IHashCodeProvider? hashProvider, IComparer? comparer)
	{
	}

	public NameValueCollection(NameValueCollection col)
	{
	}

	public NameValueCollection(int capacity)
	{
	}

	public NameValueCollection(int capacity, IEqualityComparer? equalityComparer)
	{
	}

	[Obsolete("This constructor has been deprecated. Use NameValueCollection(Int32, IEqualityComparer) instead.")]
	public NameValueCollection(int capacity, IHashCodeProvider? hashProvider, IComparer? comparer)
	{
	}

	public NameValueCollection(int capacity, NameValueCollection col)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected NameValueCollection(SerializationInfo info, StreamingContext context)
	{
	}

	public void Add(NameValueCollection c)
	{
	}

	public virtual void Add(string? name, string? value)
	{
	}

	public virtual void Clear()
	{
	}

	public void CopyTo(Array dest, int index)
	{
	}

	public virtual string? Get(int index)
	{
		throw null;
	}

	public virtual string? Get(string? name)
	{
		throw null;
	}

	public virtual string? GetKey(int index)
	{
		throw null;
	}

	public virtual string[]? GetValues(int index)
	{
		throw null;
	}

	public virtual string[]? GetValues(string? name)
	{
		throw null;
	}

	public bool HasKeys()
	{
		throw null;
	}

	protected void InvalidateCachedArrays()
	{
	}

	public virtual void Remove(string? name)
	{
	}

	public virtual void Set(string? name, string? value)
	{
	}
}
