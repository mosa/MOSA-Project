using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Concurrent;

public class ConcurrentDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary where TKey : notnull
{
	public IEqualityComparer<TKey> Comparer
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

	public bool IsEmpty
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

	public ICollection<TKey> Keys
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

	public ICollection<TValue> Values
	{
		get
		{
			throw null;
		}
	}

	public ConcurrentDictionary()
	{
	}

	public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
	{
	}

	public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer)
	{
	}

	public ConcurrentDictionary(IEqualityComparer<TKey>? comparer)
	{
	}

	public ConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer)
	{
	}

	public ConcurrentDictionary(int concurrencyLevel, int capacity)
	{
	}

	public ConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey>? comparer)
	{
	}

	public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
	{
		throw null;
	}

	public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
	{
		throw null;
	}

	public TValue AddOrUpdate<TArg>(TKey key, Func<TKey, TArg, TValue> addValueFactory, Func<TKey, TValue, TArg, TValue> updateValueFactory, TArg factoryArgument)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool ContainsKey(TKey key)
	{
		throw null;
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		throw null;
	}

	public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
	{
		throw null;
	}

	public TValue GetOrAdd(TKey key, TValue value)
	{
		throw null;
	}

	public TValue GetOrAdd<TArg>(TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument)
	{
		throw null;
	}

	void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
	{
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
	{
		throw null;
	}

	void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
	{
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
	{
		throw null;
	}

	void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
	{
	}

	bool IDictionary<TKey, TValue>.Remove(TKey key)
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

	public KeyValuePair<TKey, TValue>[] ToArray()
	{
		throw null;
	}

	public bool TryAdd(TKey key, TValue value)
	{
		throw null;
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		throw null;
	}

	public bool TryRemove(KeyValuePair<TKey, TValue> item)
	{
		throw null;
	}

	public bool TryRemove(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		throw null;
	}

	public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
	{
		throw null;
	}
}
