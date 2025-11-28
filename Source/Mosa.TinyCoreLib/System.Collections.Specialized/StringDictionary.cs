using System.ComponentModel.Design.Serialization;

namespace System.Collections.Specialized;

[DesignerSerializer("System.Diagnostics.Design.StringDictionaryCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public class StringDictionary : IEnumerable
{
	public virtual int Count
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

	public virtual string? this[string key]
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

	public virtual void Add(string key, string? value)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual bool ContainsKey(string key)
	{
		throw null;
	}

	public virtual bool ContainsValue(string? value)
	{
		throw null;
	}

	public virtual void CopyTo(Array array, int index)
	{
	}

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual void Remove(string key)
	{
	}
}
