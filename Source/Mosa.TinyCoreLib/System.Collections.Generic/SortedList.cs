using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public class SortedList<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary where TKey : notnull
{
	public int Capacity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IComparer<TKey> Comparer
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

	public TValue this[TKey key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IList<TKey> Keys
	{
		get
		{
			throw null;
		}
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	ICollection<TKey> IDictionary<TKey, TValue>.Keys
	{
		get
		{
			throw null;
		}
	}

	ICollection<TValue> IDictionary<TKey, TValue>.Values
	{
		get
		{
			throw null;
		}
	}

	IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
	{
		get
		{
			throw null;
		}
	}

	IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
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

	bool IDictionary.IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	bool IDictionary.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	object? IDictionary.this[object key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	ICollection IDictionary.Keys
	{
		get
		{
			throw null;
		}
	}

	ICollection IDictionary.Values
	{
		get
		{
			throw null;
		}
	}

	public IList<TValue> Values
	{
		get
		{
			throw null;
		}
	}

	public SortedList()
	{
	}

	public SortedList(IComparer<TKey>? comparer)
	{
	}

	public SortedList(IDictionary<TKey, TValue> dictionary)
	{
	}

	public SortedList(IDictionary<TKey, TValue> dictionary, IComparer<TKey>? comparer)
	{
	}

	public SortedList(int capacity)
	{
	}

	public SortedList(int capacity, IComparer<TKey>? comparer)
	{
	}

	public void Add(TKey key, TValue value)
	{
	}

	public void Clear()
	{
	}

	public bool ContainsKey(TKey key)
	{
		throw null;
	}

	public bool ContainsValue(TValue value)
	{
		throw null;
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		throw null;
	}

	public TKey GetKeyAtIndex(int index)
	{
		throw null;
	}

	public TValue GetValueAtIndex(int index)
	{
		throw null;
	}

	public int IndexOfKey(TKey key)
	{
		throw null;
	}

	public int IndexOfValue(TValue value)
	{
		throw null;
	}

	public bool Remove(TKey key)
	{
		throw null;
	}

	public void RemoveAt(int index)
	{
	}

	public void SetValueAtIndex(int index, TValue value)
	{
	}

	void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
	{
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
	{
		throw null;
	}

	void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
	{
		throw null;
	}

	IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	void IDictionary.Add(object key, object? value)
	{
	}

	bool IDictionary.Contains(object key)
	{
		throw null;
	}

	IDictionaryEnumerator IDictionary.GetEnumerator()
	{
		throw null;
	}

	void IDictionary.Remove(object key)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public void TrimExcess()
	{
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		throw null;
	}
}
