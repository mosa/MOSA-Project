using System.Collections.Generic;

namespace System.Collections.Immutable;

public interface IImmutableDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>
{
	IImmutableDictionary<TKey, TValue> Add(TKey key, TValue value);

	IImmutableDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs);

	IImmutableDictionary<TKey, TValue> Clear();

	bool Contains(KeyValuePair<TKey, TValue> pair);

	IImmutableDictionary<TKey, TValue> Remove(TKey key);

	IImmutableDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys);

	IImmutableDictionary<TKey, TValue> SetItem(TKey key, TValue value);

	IImmutableDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items);

	bool TryGetKey(TKey equalKey, out TKey actualKey);
}
