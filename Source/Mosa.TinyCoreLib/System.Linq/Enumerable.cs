using System.Collections;
using System.Collections.Generic;

namespace System.Linq;

public static class Enumerable
{
	public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
	{
		throw null;
	}

	public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
	{
		throw null;
	}

	public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
	{
		throw null;
	}

	public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static bool Any<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource element)
	{
		throw null;
	}

	public static IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static decimal Average(this IEnumerable<decimal> source)
	{
		throw null;
	}

	public static double Average(this IEnumerable<double> source)
	{
		throw null;
	}

	public static double Average(this IEnumerable<int> source)
	{
		throw null;
	}

	public static double Average(this IEnumerable<long> source)
	{
		throw null;
	}

	public static decimal? Average(this IEnumerable<decimal?> source)
	{
		throw null;
	}

	public static double? Average(this IEnumerable<double?> source)
	{
		throw null;
	}

	public static double? Average(this IEnumerable<int?> source)
	{
		throw null;
	}

	public static double? Average(this IEnumerable<long?> source)
	{
		throw null;
	}

	public static float? Average(this IEnumerable<float?> source)
	{
		throw null;
	}

	public static float Average(this IEnumerable<float> source)
	{
		throw null;
	}

	public static decimal Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
	{
		throw null;
	}

	public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
	{
		throw null;
	}

	public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
	{
		throw null;
	}

	public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
	{
		throw null;
	}

	public static decimal? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
	{
		throw null;
	}

	public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
	{
		throw null;
	}

	public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
	{
		throw null;
	}

	public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
	{
		throw null;
	}

	public static float? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
	{
		throw null;
	}

	public static float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
	{
		throw null;
	}

	public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
	{
		throw null;
	}

	public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> source, int size)
	{
		throw null;
	}

	public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
	{
		throw null;
	}

	public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
	{
		throw null;
	}

	public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static int Count<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static IEnumerable<TSource?> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
	{
		throw null;
	}

	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static TSource? ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, Index index)
	{
		throw null;
	}

	public static TSource? ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index)
	{
		throw null;
	}

	public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, Index index)
	{
		throw null;
	}

	public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
	{
		throw null;
	}

	public static IEnumerable<TResult> Empty<TResult>()
	{
		throw null;
	}

	public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
	{
		throw null;
	}

	public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
	{
		throw null;
	}

	public static TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue)
	{
		throw null;
	}

	public static TSource First<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
	{
		throw null;
	}

	public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
	{
		throw null;
	}

	public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
	{
		throw null;
	}

	public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
	{
		throw null;
	}

	public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
	{
		throw null;
	}

	public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
	{
		throw null;
	}

	public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static TSource? LastOrDefault<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
	{
		throw null;
	}

	public static TSource? LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue)
	{
		throw null;
	}

	public static TSource Last<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static long LongCount<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static decimal Max(this IEnumerable<decimal> source)
	{
		throw null;
	}

	public static double Max(this IEnumerable<double> source)
	{
		throw null;
	}

	public static int Max(this IEnumerable<int> source)
	{
		throw null;
	}

	public static long Max(this IEnumerable<long> source)
	{
		throw null;
	}

	public static decimal? Max(this IEnumerable<decimal?> source)
	{
		throw null;
	}

	public static double? Max(this IEnumerable<double?> source)
	{
		throw null;
	}

	public static int? Max(this IEnumerable<int?> source)
	{
		throw null;
	}

	public static long? Max(this IEnumerable<long?> source)
	{
		throw null;
	}

	public static float? Max(this IEnumerable<float?> source)
	{
		throw null;
	}

	public static float Max(this IEnumerable<float> source)
	{
		throw null;
	}

	public static TSource? MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static TSource? MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
	{
		throw null;
	}

	public static TSource? Max<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static TSource? Max<TSource>(this IEnumerable<TSource> source, IComparer<TSource>? comparer)
	{
		throw null;
	}

	public static decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
	{
		throw null;
	}

	public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
	{
		throw null;
	}

	public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
	{
		throw null;
	}

	public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
	{
		throw null;
	}

	public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
	{
		throw null;
	}

	public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
	{
		throw null;
	}

	public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
	{
		throw null;
	}

	public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
	{
		throw null;
	}

	public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
	{
		throw null;
	}

	public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
	{
		throw null;
	}

	public static TResult? Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
	{
		throw null;
	}

	public static decimal Min(this IEnumerable<decimal> source)
	{
		throw null;
	}

	public static double Min(this IEnumerable<double> source)
	{
		throw null;
	}

	public static int Min(this IEnumerable<int> source)
	{
		throw null;
	}

	public static long Min(this IEnumerable<long> source)
	{
		throw null;
	}

	public static decimal? Min(this IEnumerable<decimal?> source)
	{
		throw null;
	}

	public static double? Min(this IEnumerable<double?> source)
	{
		throw null;
	}

	public static int? Min(this IEnumerable<int?> source)
	{
		throw null;
	}

	public static long? Min(this IEnumerable<long?> source)
	{
		throw null;
	}

	public static float? Min(this IEnumerable<float?> source)
	{
		throw null;
	}

	public static float Min(this IEnumerable<float> source)
	{
		throw null;
	}

	public static TSource? MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static TSource? MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
	{
		throw null;
	}

	public static TSource? Min<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static TSource? Min<TSource>(this IEnumerable<TSource> source, IComparer<TSource>? comparer)
	{
		throw null;
	}

	public static decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
	{
		throw null;
	}

	public static double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
	{
		throw null;
	}

	public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
	{
		throw null;
	}

	public static long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
	{
		throw null;
	}

	public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
	{
		throw null;
	}

	public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
	{
		throw null;
	}

	public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
	{
		throw null;
	}

	public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
	{
		throw null;
	}

	public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
	{
		throw null;
	}

	public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
	{
		throw null;
	}

	public static TResult? Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
	{
		throw null;
	}

	public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
	{
		throw null;
	}

	public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IOrderedEnumerable<T> OrderDescending<T>(this IEnumerable<T> source)
	{
		throw null;
	}

	public static IOrderedEnumerable<T> OrderDescending<T>(this IEnumerable<T> source, IComparer<T>? comparer)
	{
		throw null;
	}

	public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source)
	{
		throw null;
	}

	public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source, IComparer<T>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource element)
	{
		throw null;
	}

	public static IEnumerable<int> Range(int start, int count)
	{
		throw null;
	}

	public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
	{
		throw null;
	}

	public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
	{
		throw null;
	}

	public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
	{
		throw null;
	}

	public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
	{
		throw null;
	}

	public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
	{
		throw null;
	}

	public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
	{
		throw null;
	}

	public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
	{
		throw null;
	}

	public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
	{
		throw null;
	}

	public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
	{
		throw null;
	}

	public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue)
	{
		throw null;
	}

	public static TSource Single<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count)
	{
		throw null;
	}

	public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
	{
		throw null;
	}

	public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
	{
		throw null;
	}

	public static decimal Sum(this IEnumerable<decimal> source)
	{
		throw null;
	}

	public static double Sum(this IEnumerable<double> source)
	{
		throw null;
	}

	public static int Sum(this IEnumerable<int> source)
	{
		throw null;
	}

	public static long Sum(this IEnumerable<long> source)
	{
		throw null;
	}

	public static decimal? Sum(this IEnumerable<decimal?> source)
	{
		throw null;
	}

	public static double? Sum(this IEnumerable<double?> source)
	{
		throw null;
	}

	public static int? Sum(this IEnumerable<int?> source)
	{
		throw null;
	}

	public static long? Sum(this IEnumerable<long?> source)
	{
		throw null;
	}

	public static float? Sum(this IEnumerable<float?> source)
	{
		throw null;
	}

	public static float Sum(this IEnumerable<float> source)
	{
		throw null;
	}

	public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
	{
		throw null;
	}

	public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
	{
		throw null;
	}

	public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
	{
		throw null;
	}

	public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
	{
		throw null;
	}

	public static decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
	{
		throw null;
	}

	public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
	{
		throw null;
	}

	public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
	{
		throw null;
	}

	public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
	{
		throw null;
	}

	public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
	{
		throw null;
	}

	public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
	{
		throw null;
	}

	public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
	{
		throw null;
	}

	public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
	{
		throw null;
	}

	public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
	{
		throw null;
	}

	public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, Range range)
	{
		throw null;
	}

	public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
	{
		throw null;
	}

	public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source) where TKey : notnull
	{
		throw null;
	}

	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey>? comparer) where TKey : notnull
	{
		throw null;
	}

	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source) where TKey : notnull
	{
		throw null;
	}

	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source, IEqualityComparer<TKey>? comparer) where TKey : notnull
	{
		throw null;
	}

	public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : notnull
	{
		throw null;
	}

	public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) where TKey : notnull
	{
		throw null;
	}

	public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) where TKey : notnull
	{
		throw null;
	}

	public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer) where TKey : notnull
	{
		throw null;
	}

	public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
	{
		throw null;
	}

	public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
	{
		throw null;
	}

	public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static bool TryGetNonEnumeratedCount<TSource>(this IEnumerable<TSource> source, out int count)
	{
		throw null;
	}

	public static IEnumerable<TSource> UnionBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector)
	{
		throw null;
	}

	public static IEnumerable<TSource> UnionBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
	{
		throw null;
	}

	public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		throw null;
	}

	public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
	{
		throw null;
	}

	public static IEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
	{
		throw null;
	}

	public static IEnumerable<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third)
	{
		throw null;
	}

	public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
	{
		throw null;
	}
}
