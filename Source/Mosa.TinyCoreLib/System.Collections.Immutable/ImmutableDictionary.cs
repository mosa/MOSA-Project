using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable;

public static class ImmutableDictionary
{
	public static bool Contains<TKey, TValue>(this IImmutableDictionary<TKey, TValue> map, TKey key, TValue value) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>() where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(IEqualityComparer<TKey>? keyComparer) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> items) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEqualityComparer<TKey>? keyComparer, IEnumerable<KeyValuePair<TKey, TValue>> items) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer, IEnumerable<KeyValuePair<TKey, TValue>> items) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> Create<TKey, TValue>() where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey>? keyComparer) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer) where TKey : notnull
	{
		throw null;
	}

	public static TValue? GetValueOrDefault<TKey, TValue>(this IImmutableDictionary<TKey, TValue> dictionary, TKey key) where TKey : notnull
	{
		throw null;
	}

	public static TValue GetValueOrDefault<TKey, TValue>(this IImmutableDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey>? keyComparer) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TSource> ToImmutableDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TSource> ToImmutableDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? keyComparer) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(this ImmutableDictionary<TKey, TValue>.Builder builder) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IEqualityComparer<TKey>? keyComparer) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer) where TKey : notnull
	{
		throw null;
	}
}
public sealed class ImmutableDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary, IImmutableDictionary<TKey, TValue> where TKey : notnull
{
	public sealed class Builder : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary
	{
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

		public IEqualityComparer<TKey> KeyComparer
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public IEnumerable<TKey> Keys
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

		public IEqualityComparer<TValue> ValueComparer
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public IEnumerable<TValue> Values
		{
			get
			{
				throw null;
			}
		}

		internal Builder()
		{
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
		}

		public void Add(TKey key, TValue value)
		{
		}

		public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
		{
		}

		public void Clear()
		{
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			throw null;
		}

		public bool ContainsKey(TKey key)
		{
			throw null;
		}

		public bool ContainsValue(TValue value)
		{
			throw null;
		}

		public Enumerator GetEnumerator()
		{
			throw null;
		}

		public TValue? GetValueOrDefault(TKey key)
		{
			throw null;
		}

		public TValue GetValueOrDefault(TKey key, TValue defaultValue)
		{
			throw null;
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			throw null;
		}

		public bool Remove(TKey key)
		{
			throw null;
		}

		public void RemoveRange(IEnumerable<TKey> keys)
		{
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			throw null;
		}

		void ICollection.CopyTo(Array array, int arrayIndex)
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

		public ImmutableDictionary<TKey, TValue> ToImmutable()
		{
			throw null;
		}

		public bool TryGetKey(TKey equalKey, out TKey actualKey)
		{
			throw null;
		}

		public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
		{
			throw null;
		}
	}

	public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public KeyValuePair<TKey, TValue> Current
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

		public void Reset()
		{
		}
	}

	public static readonly ImmutableDictionary<TKey, TValue> Empty;

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
	}

	public IEqualityComparer<TKey> KeyComparer
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<TKey> Keys
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

	public IEqualityComparer<TValue> ValueComparer
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<TValue> Values
	{
		get
		{
			throw null;
		}
	}

	internal ImmutableDictionary()
	{
	}

	public ImmutableDictionary<TKey, TValue> Add(TKey key, TValue value)
	{
		throw null;
	}

	public ImmutableDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
	{
		throw null;
	}

	public ImmutableDictionary<TKey, TValue> Clear()
	{
		throw null;
	}

	public bool Contains(KeyValuePair<TKey, TValue> pair)
	{
		throw null;
	}

	public bool ContainsKey(TKey key)
	{
		throw null;
	}

	public bool ContainsValue(TValue value)
	{
		throw null;
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	public ImmutableDictionary<TKey, TValue> Remove(TKey key)
	{
		throw null;
	}

	public ImmutableDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys)
	{
		throw null;
	}

	public ImmutableDictionary<TKey, TValue> SetItem(TKey key, TValue value)
	{
		throw null;
	}

	public ImmutableDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
	{
		throw null;
	}

	void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
	{
	}

	void ICollection<KeyValuePair<TKey, TValue>>.Clear()
	{
	}

	void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
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

	void ICollection.CopyTo(Array array, int arrayIndex)
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

	IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Add(TKey key, TValue value)
	{
		throw null;
	}

	IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
	{
		throw null;
	}

	IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Clear()
	{
		throw null;
	}

	IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Remove(TKey key)
	{
		throw null;
	}

	IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys)
	{
		throw null;
	}

	IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItem(TKey key, TValue value)
	{
		throw null;
	}

	IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
	{
		throw null;
	}

	public Builder ToBuilder()
	{
		throw null;
	}

	public bool TryGetKey(TKey equalKey, out TKey actualKey)
	{
		throw null;
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		throw null;
	}

	public ImmutableDictionary<TKey, TValue> WithComparers(IEqualityComparer<TKey>? keyComparer)
	{
		throw null;
	}

	public ImmutableDictionary<TKey, TValue> WithComparers(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
	{
		throw null;
	}
}
