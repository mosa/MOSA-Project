using System.Collections.Generic;
using System.Collections.Immutable;

namespace System.Linq;

public static class ImmutableArrayExtensions
{
	public static T? Aggregate<T>(this ImmutableArray<T> immutableArray, Func<T, T, T> func)
	{
		throw null;
	}

	public static TAccumulate Aggregate<TAccumulate, T>(this ImmutableArray<T> immutableArray, TAccumulate seed, Func<TAccumulate, T, TAccumulate> func)
	{
		throw null;
	}

	public static TResult Aggregate<TAccumulate, TResult, T>(this ImmutableArray<T> immutableArray, TAccumulate seed, Func<TAccumulate, T, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
	{
		throw null;
	}

	public static bool All<T>(this ImmutableArray<T> immutableArray, Func<T, bool> predicate)
	{
		throw null;
	}

	public static bool Any<T>(this ImmutableArray<T> immutableArray)
	{
		throw null;
	}

	public static bool Any<T>(this ImmutableArray<T> immutableArray, Func<T, bool> predicate)
	{
		throw null;
	}

	public static bool Any<T>(this ImmutableArray<T>.Builder builder)
	{
		throw null;
	}

	public static T? ElementAtOrDefault<T>(this ImmutableArray<T> immutableArray, int index)
	{
		throw null;
	}

	public static T ElementAt<T>(this ImmutableArray<T> immutableArray, int index)
	{
		throw null;
	}

	public static T? FirstOrDefault<T>(this ImmutableArray<T> immutableArray)
	{
		throw null;
	}

	public static T? FirstOrDefault<T>(this ImmutableArray<T> immutableArray, Func<T, bool> predicate)
	{
		throw null;
	}

	public static T? FirstOrDefault<T>(this ImmutableArray<T>.Builder builder)
	{
		throw null;
	}

	public static T First<T>(this ImmutableArray<T> immutableArray)
	{
		throw null;
	}

	public static T First<T>(this ImmutableArray<T> immutableArray, Func<T, bool> predicate)
	{
		throw null;
	}

	public static T First<T>(this ImmutableArray<T>.Builder builder)
	{
		throw null;
	}

	public static T? LastOrDefault<T>(this ImmutableArray<T> immutableArray)
	{
		throw null;
	}

	public static T? LastOrDefault<T>(this ImmutableArray<T> immutableArray, Func<T, bool> predicate)
	{
		throw null;
	}

	public static T? LastOrDefault<T>(this ImmutableArray<T>.Builder builder)
	{
		throw null;
	}

	public static T Last<T>(this ImmutableArray<T> immutableArray)
	{
		throw null;
	}

	public static T Last<T>(this ImmutableArray<T> immutableArray, Func<T, bool> predicate)
	{
		throw null;
	}

	public static T Last<T>(this ImmutableArray<T>.Builder builder)
	{
		throw null;
	}

	public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this ImmutableArray<TSource> immutableArray, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
	{
		throw null;
	}

	public static IEnumerable<TResult> Select<T, TResult>(this ImmutableArray<T> immutableArray, Func<T, TResult> selector)
	{
		throw null;
	}

	public static bool SequenceEqual<TDerived, TBase>(this ImmutableArray<TBase> immutableArray, IEnumerable<TDerived> items, IEqualityComparer<TBase>? comparer = null) where TDerived : TBase
	{
		throw null;
	}

	public static bool SequenceEqual<TDerived, TBase>(this ImmutableArray<TBase> immutableArray, ImmutableArray<TDerived> items, IEqualityComparer<TBase>? comparer = null) where TDerived : TBase
	{
		throw null;
	}

	public static bool SequenceEqual<TDerived, TBase>(this ImmutableArray<TBase> immutableArray, ImmutableArray<TDerived> items, Func<TBase, TBase, bool> predicate) where TDerived : TBase
	{
		throw null;
	}

	public static T? SingleOrDefault<T>(this ImmutableArray<T> immutableArray)
	{
		throw null;
	}

	public static T? SingleOrDefault<T>(this ImmutableArray<T> immutableArray, Func<T, bool> predicate)
	{
		throw null;
	}

	public static T Single<T>(this ImmutableArray<T> immutableArray)
	{
		throw null;
	}

	public static T Single<T>(this ImmutableArray<T> immutableArray, Func<T, bool> predicate)
	{
		throw null;
	}

	public static T[] ToArray<T>(this ImmutableArray<T> immutableArray)
	{
		throw null;
	}

	public static Dictionary<TKey, T> ToDictionary<TKey, T>(this ImmutableArray<T> immutableArray, Func<T, TKey> keySelector) where TKey : notnull
	{
		throw null;
	}

	public static Dictionary<TKey, T> ToDictionary<TKey, T>(this ImmutableArray<T> immutableArray, Func<T, TKey> keySelector, IEqualityComparer<TKey>? comparer) where TKey : notnull
	{
		throw null;
	}

	public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement, T>(this ImmutableArray<T> immutableArray, Func<T, TKey> keySelector, Func<T, TElement> elementSelector) where TKey : notnull
	{
		throw null;
	}

	public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement, T>(this ImmutableArray<T> immutableArray, Func<T, TKey> keySelector, Func<T, TElement> elementSelector, IEqualityComparer<TKey>? comparer) where TKey : notnull
	{
		throw null;
	}

	public static IEnumerable<T> Where<T>(this ImmutableArray<T> immutableArray, Func<T, bool> predicate)
	{
		throw null;
	}
}
