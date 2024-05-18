using System.Collections.Generic;

namespace System.Runtime.InteropServices;

public static class CollectionsMarshal
{
	public static Span<T> AsSpan<T>(List<T>? list)
	{
		throw null;
	}

	public static ref TValue GetValueRefOrNullRef<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key) where TKey : notnull
	{
		throw null;
	}

	public static ref TValue? GetValueRefOrAddDefault<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, out bool exists) where TKey : notnull
	{
		throw null;
	}

	public static void SetCount<T>(List<T> list, int count)
	{
		throw null;
	}
}
