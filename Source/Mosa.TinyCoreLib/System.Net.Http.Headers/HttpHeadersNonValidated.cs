using System.Collections;
using System.Collections.Generic;

namespace System.Net.Http.Headers;

public readonly struct HttpHeadersNonValidated : IEnumerable<KeyValuePair<string, HeaderStringValues>>, IEnumerable, IReadOnlyCollection<KeyValuePair<string, HeaderStringValues>>, IReadOnlyDictionary<string, HeaderStringValues>
{
	public struct Enumerator : IEnumerator<KeyValuePair<string, HeaderStringValues>>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public KeyValuePair<string, HeaderStringValues> Current
		{
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				throw null;
			}
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			throw null;
		}

		void IEnumerator.Reset()
		{
		}
	}

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public HeaderStringValues this[string headerName]
	{
		get
		{
			throw null;
		}
	}

	IEnumerable<string> IReadOnlyDictionary<string, HeaderStringValues>.Keys
	{
		get
		{
			throw null;
		}
	}

	IEnumerable<HeaderStringValues> IReadOnlyDictionary<string, HeaderStringValues>.Values
	{
		get
		{
			throw null;
		}
	}

	public bool Contains(string headerName)
	{
		throw null;
	}

	bool IReadOnlyDictionary<string, HeaderStringValues>.ContainsKey(string key)
	{
		throw null;
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	IEnumerator<KeyValuePair<string, HeaderStringValues>> IEnumerable<KeyValuePair<string, HeaderStringValues>>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public bool TryGetValues(string headerName, out HeaderStringValues values)
	{
		throw null;
	}

	bool IReadOnlyDictionary<string, HeaderStringValues>.TryGetValue(string key, out HeaderStringValues value)
	{
		throw null;
	}
}
