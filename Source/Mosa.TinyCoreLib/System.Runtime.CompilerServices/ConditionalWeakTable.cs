using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

public sealed class ConditionalWeakTable<TKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable where TKey : class where TValue : class?
{
	public delegate TValue CreateValueCallback(TKey key);

	public void Add(TKey key, TValue value)
	{
	}

	public void AddOrUpdate(TKey key, TValue value)
	{
	}

	public void Clear()
	{
	}

	public TValue GetOrCreateValue(TKey key)
	{
		throw null;
	}

	public TValue GetValue(TKey key, CreateValueCallback createValueCallback)
	{
		throw null;
	}

	public bool Remove(TKey key)
	{
		throw null;
	}

	IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
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
}
