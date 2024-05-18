using System.Collections;
using System.Collections.Generic;

namespace System.Speech.Recognition;

public sealed class SemanticValue : ICollection<KeyValuePair<string, SemanticValue>>, IEnumerable<KeyValuePair<string, SemanticValue>>, IEnumerable, IDictionary<string, SemanticValue>
{
	public float Confidence
	{
		get
		{
			throw null;
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public SemanticValue this[string key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	bool ICollection<KeyValuePair<string, SemanticValue>>.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	ICollection<string> IDictionary<string, SemanticValue>.Keys
	{
		get
		{
			throw null;
		}
	}

	ICollection<SemanticValue> IDictionary<string, SemanticValue>.Values
	{
		get
		{
			throw null;
		}
	}

	public object Value
	{
		get
		{
			throw null;
		}
	}

	public SemanticValue(object value)
	{
	}

	public SemanticValue(string keyName, object value, float confidence)
	{
	}

	public bool Contains(KeyValuePair<string, SemanticValue> item)
	{
		throw null;
	}

	public bool ContainsKey(string key)
	{
		throw null;
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	void ICollection<KeyValuePair<string, SemanticValue>>.Add(KeyValuePair<string, SemanticValue> key)
	{
	}

	void ICollection<KeyValuePair<string, SemanticValue>>.Clear()
	{
	}

	void ICollection<KeyValuePair<string, SemanticValue>>.CopyTo(KeyValuePair<string, SemanticValue>[] array, int index)
	{
	}

	bool ICollection<KeyValuePair<string, SemanticValue>>.Remove(KeyValuePair<string, SemanticValue> key)
	{
		throw null;
	}

	void IDictionary<string, SemanticValue>.Add(string key, SemanticValue value)
	{
	}

	bool IDictionary<string, SemanticValue>.Remove(string key)
	{
		throw null;
	}

	bool IDictionary<string, SemanticValue>.TryGetValue(string key, out SemanticValue value)
	{
		throw null;
	}

	IEnumerator<KeyValuePair<string, SemanticValue>> IEnumerable<KeyValuePair<string, SemanticValue>>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
