using System.ComponentModel;

namespace System.Collections.Generic;

public static class KeyValuePair
{
	public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
	{
		throw null;
	}
}
public readonly struct KeyValuePair<TKey, TValue>
{
	private readonly TKey key;

	private readonly TValue value;

	private readonly int _dummyPrimitive;

	public TKey Key
	{
		get
		{
			throw null;
		}
	}

	public TValue Value
	{
		get
		{
			throw null;
		}
	}

	public KeyValuePair(TKey key, TValue value)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out TKey key, out TValue value)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
