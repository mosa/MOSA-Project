using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http;

public sealed class HttpRequestOptions : IDictionary<string, object?>, ICollection<KeyValuePair<string, object?>>, IEnumerable<KeyValuePair<string, object?>>, IEnumerable, IReadOnlyDictionary<string, object?>, IReadOnlyCollection<KeyValuePair<string, object?>>
{
	int IReadOnlyCollection<KeyValuePair<string, object>>.Count
	{
		get
		{
			throw null;
		}
	}

	ICollection<string> IDictionary<string, object>.Keys
	{
		get
		{
			throw null;
		}
	}

	ICollection<object?> IDictionary<string, object>.Values
	{
		get
		{
			throw null;
		}
	}

	object? IDictionary<string, object>.this[string key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	int ICollection<KeyValuePair<string, object>>.Count
	{
		get
		{
			throw null;
		}
	}

	bool ICollection<KeyValuePair<string, object>>.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	object? IReadOnlyDictionary<string, object>.this[string key]
	{
		get
		{
			throw null;
		}
	}

	IEnumerable<string> IReadOnlyDictionary<string, object>.Keys
	{
		get
		{
			throw null;
		}
	}

	IEnumerable<object?> IReadOnlyDictionary<string, object>.Values
	{
		get
		{
			throw null;
		}
	}

	bool IReadOnlyDictionary<string, object>.ContainsKey(string key)
	{
		throw null;
	}

	void IDictionary<string, object>.Add(string key, object? value)
	{
		throw null;
	}

	bool IDictionary<string, object>.Remove(string key)
	{
		throw null;
	}

	bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object?> item)
	{
		throw null;
	}

	bool IDictionary<string, object>.TryGetValue(string key, out object? value)
	{
		throw null;
	}

	void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object?> item)
	{
		throw null;
	}

	void ICollection<KeyValuePair<string, object>>.Clear()
	{
		throw null;
	}

	bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object?> item)
	{
		throw null;
	}

	bool IDictionary<string, object>.ContainsKey(string key)
	{
		throw null;
	}

	void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
	{
		throw null;
	}

	IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public bool TryGetValue<TValue>(HttpRequestOptionsKey<TValue> key, [MaybeNullWhen(false)] out TValue value)
	{
		throw null;
	}

	public void Set<TValue>(HttpRequestOptionsKey<TValue> key, TValue value)
	{
		throw null;
	}

	bool IReadOnlyDictionary<string, object>.TryGetValue(string key, out object? value)
	{
		throw null;
	}
}
