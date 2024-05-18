using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Frozen;

public static class FrozenDictionary
{
	public static FrozenDictionary<TKey, TValue> ToFrozenDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
	{
		throw null;
	}

	public static FrozenDictionary<TKey, TSource> ToFrozenDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
	{
		throw null;
	}

	public static FrozenDictionary<TKey, TElement> ToFrozenDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
	{
		throw null;
	}
}
public abstract class FrozenDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary where TKey : notnull
{
	public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
	{
		private readonly TKey[] _keys;

		private readonly TValue[] _values;

		private object _dummy;

		private int _dummyPrimitive;

		public readonly KeyValuePair<TKey, TValue> Current
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

		public bool MoveNext()
		{
			throw null;
		}

		void IEnumerator.Reset()
		{
		}

		void IDisposable.Dispose()
		{
		}
	}

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

	public static FrozenDictionary<TKey, TValue> Empty
	{
		get
		{
			throw null;
		}
	}

	public ref readonly TValue this[TKey key]
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<TKey> Keys
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

	TValue IDictionary<TKey, TValue>.this[TKey key]
	{
		get
		{
			throw null;
		}
		set
		{
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

	TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
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

	public ImmutableArray<TValue> Values
	{
		get
		{
			throw null;
		}
	}

	internal FrozenDictionary()
	{
	}

	public bool ContainsKey(TKey key)
	{
		throw null;
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] destination, int destinationIndex)
	{
	}

	public void CopyTo(Span<KeyValuePair<TKey, TValue>> destination)
	{
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	public ref readonly TValue GetValueRefOrNullRef(TKey key)
	{
		throw null;
	}

	void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
	{
	}

	void ICollection<KeyValuePair<TKey, TValue>>.Clear()
	{
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
	{
		throw null;
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
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

	void IDictionary.Clear()
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

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		throw null;
	}
}
