using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public static class CollectionExtensions
{
	public static void AddRange<T>(this List<T> list, ReadOnlySpan<T> source)
	{
	}

	public static void CopyTo<T>(this List<T> list, Span<T> destination)
	{
	}

	public static TValue? GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
	{
		throw null;
	}

	public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
	{
		throw null;
	}

	public static void InsertRange<T>(this List<T> list, int index, ReadOnlySpan<T> source)
	{
	}

	public static bool Remove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		throw null;
	}

	public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
	{
		throw null;
	}

	public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
	{
		throw null;
	}

	public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) where TKey : notnull
	{
		throw null;
	}
}
