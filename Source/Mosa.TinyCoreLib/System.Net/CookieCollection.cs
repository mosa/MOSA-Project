using System.Collections;
using System.Collections.Generic;

namespace System.Net;

public class CookieCollection : ICollection<Cookie>, IEnumerable<Cookie>, IEnumerable, IReadOnlyCollection<Cookie>, ICollection
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

	public Cookie this[int index]
	{
		get
		{
			throw null;
		}
	}

	public Cookie? this[string name]
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

	public void Add(Cookie cookie)
	{
	}

	public void Add(CookieCollection cookies)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(Cookie cookie)
	{
		throw null;
	}

	public void CopyTo(Array array, int index)
	{
	}

	public void CopyTo(Cookie[] array, int index)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public bool Remove(Cookie cookie)
	{
		throw null;
	}

	IEnumerator<Cookie> IEnumerable<Cookie>.GetEnumerator()
	{
		throw null;
	}
}
