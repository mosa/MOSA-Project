using System.ComponentModel;

namespace System.Collections.Generic;

public static class KeyValuePair
{
	public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) => new(key, value);
}

public readonly struct KeyValuePair<TKey, TValue>(TKey key, TValue value)
{
	private readonly TKey key = key;
	private readonly TValue value = value;

	public TKey Key => key;

	public TValue Value => value;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out TKey key, out TValue value)
	{
		key = this.key;
		value = this.value;
	}

	public override string ToString() => $"[{key}, {value}]";
}
