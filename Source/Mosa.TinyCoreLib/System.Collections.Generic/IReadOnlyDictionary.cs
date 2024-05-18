using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public interface IReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
{
	TValue this[TKey key] { get; }

	IEnumerable<TKey> Keys { get; }

	IEnumerable<TValue> Values { get; }

	bool ContainsKey(TKey key);

	bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);
}
