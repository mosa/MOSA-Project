using System.Collections;

namespace System.DirectoryServices;

public class SearchResultCollection : MarshalByRefObject, ICollection, IEnumerable, IDisposable
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public IntPtr Handle
	{
		get
		{
			throw null;
		}
	}

	public SearchResult this[int index]
	{
		get
		{
			throw null;
		}
	}

	public string[] PropertiesLoaded
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

	internal SearchResultCollection()
	{
	}

	public bool Contains(SearchResult result)
	{
		throw null;
	}

	public void CopyTo(SearchResult[] results, int index)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~SearchResultCollection()
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(SearchResult result)
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}
}
