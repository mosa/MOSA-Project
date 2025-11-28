using System.Collections;
using System.Collections.Generic;

namespace System.Net;

public class HttpListenerPrefixCollection : ICollection<string>, IEnumerable<string>, IEnumerable
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

	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	internal HttpListenerPrefixCollection()
	{
	}

	public void Add(string uriPrefix)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(string uriPrefix)
	{
		throw null;
	}

	public void CopyTo(Array array, int offset)
	{
	}

	public void CopyTo(string[] array, int offset)
	{
	}

	public IEnumerator<string> GetEnumerator()
	{
		throw null;
	}

	public bool Remove(string uriPrefix)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
