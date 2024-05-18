using System.Collections;
using System.Collections.Generic;

namespace System.Net.Http.Headers;

public sealed class HttpHeaderValueCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T : class
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

	internal HttpHeaderValueCollection()
	{
	}

	public void Add(T item)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(T item)
	{
		throw null;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
	}

	public IEnumerator<T> GetEnumerator()
	{
		throw null;
	}

	public void ParseAdd(string? input)
	{
	}

	public bool Remove(T item)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public bool TryParseAdd(string? input)
	{
		throw null;
	}
}
