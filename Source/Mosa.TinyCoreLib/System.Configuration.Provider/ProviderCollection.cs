using System.Collections;

namespace System.Configuration.Provider;

public class ProviderCollection : ICollection, IEnumerable
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

	public ProviderBase this[string name]
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

	public virtual void Add(ProviderBase provider)
	{
	}

	public void Clear()
	{
	}

	public void CopyTo(ProviderBase[] array, int index)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public void Remove(string name)
	{
	}

	public void SetReadOnly()
	{
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}
}
