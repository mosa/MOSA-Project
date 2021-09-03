﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Collections.Generic
{
	// Provides a read-only view of a generic dictionary.
	public interface IReadOnlyDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		bool ContainsKey(TKey key);

		bool TryGetValue(TKey key, out TValue value);

		TValue this[TKey key] { get; }
		IEnumerable<TKey> Keys { get; }
		IEnumerable<TValue> Values { get; }
	}
}
