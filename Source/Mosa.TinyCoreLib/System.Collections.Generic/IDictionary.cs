using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	TValue this[TKey key] { get; set; }

	ICollection<TKey> Keys { get; }

	ICollection<TValue> Values { get; }

	void Add(TKey key, TValue value);

	bool ContainsKey(TKey key);

	bool Remove(TKey key);

	bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);
}
