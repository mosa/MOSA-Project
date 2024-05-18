using System.Collections;

namespace System.Configuration;

public class ConfigurationPropertyCollection : ICollection, IEnumerable
{
	public int Count
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

	public ConfigurationProperty this[string name]
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

	public void Add(ConfigurationProperty property)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(string name)
	{
		throw null;
	}

	public void CopyTo(ConfigurationProperty[] array, int index)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public bool Remove(string name)
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}
}
