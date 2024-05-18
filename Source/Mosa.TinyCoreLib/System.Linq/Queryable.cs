using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace System.Linq;

public static class Queryable
{
	public static TSource Aggregate<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> func)
	{
		throw null;
	}

	public static TAccumulate Aggregate<TSource, TAccumulate>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func)
	{
		throw null;
	}

	public static TResult Aggregate<TSource, TAccumulate, TResult>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector)
	{
		throw null;
	}

	public static bool All<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static bool Any<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static bool Any<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static IQueryable<TSource> Append<TSource>(this IQueryable<TSource> source, TSource element)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Enumerating in-memory collections as IQueryable can require unreferenced code because expressions referencing IQueryable extension methods can get rebound to IEnumerable extension methods. The IEnumerable extension methods could be trimmed causing the application to fail at runtime.")]
	[RequiresDynamicCode("Enumerating in-memory collections as IQueryable can require creating new generic types or methods, which requires creating code at runtime. This may not work when AOT compiling.")]
	public static IQueryable AsQueryable(this IEnumerable source)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Enumerating in-memory collections as IQueryable can require unreferenced code because expressions referencing IQueryable extension methods can get rebound to IEnumerable extension methods. The IEnumerable extension methods could be trimmed causing the application to fail at runtime.")]
	[RequiresDynamicCode("Enumerating in-memory collections as IQueryable can require creating new generic types or methods, which requires creating code at runtime. This may not work when AOT compiling.")]
	public static IQueryable<TElement> AsQueryable<TElement>(this IEnumerable<TElement> source)
	{
		throw null;
	}

	public static decimal Average(this IQueryable<decimal> source)
	{
		throw null;
	}

	public static double Average(this IQueryable<double> source)
	{
		throw null;
	}

	public static double Average(this IQueryable<int> source)
	{
		throw null;
	}

	public static double Average(this IQueryable<long> source)
	{
		throw null;
	}

	public static decimal? Average(this IQueryable<decimal?> source)
	{
		throw null;
	}

	public static double? Average(this IQueryable<double?> source)
	{
		throw null;
	}

	public static double? Average(this IQueryable<int?> source)
	{
		throw null;
	}

	public static double? Average(this IQueryable<long?> source)
	{
		throw null;
	}

	public static float? Average(this IQueryable<float?> source)
	{
		throw null;
	}

	public static float Average(this IQueryable<float> source)
	{
		throw null;
	}

	public static decimal Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
	{
		throw null;
	}

	public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
	{
		throw null;
	}

	public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
	{
		throw null;
	}

	public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
	{
		throw null;
	}

	public static decimal? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
	{
		throw null;
	}

	public static double? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
	{
		throw null;
	}

	public static double? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
	{
		throw null;
	}

	public static double? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
	{
		throw null;
	}

	public static float? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
	{
		throw null;
	}

	public static float Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
	{
		throw null;
	}

	public static IQueryable<TResult> Cast<TResult>(this IQueryable source)
	{
		throw null;
	}

	public static IQueryable<TSource[]> Chunk<TSource>(this IQueryable<TSource> source, int size)
	{
		throw null;
	}

	public static IQueryable<TSource> Concat<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2)
	{
		throw null;
	}

	public static bool Contains<TSource>(this IQueryable<TSource> source, TSource item)
	{
		throw null;
	}

	public static bool Contains<TSource>(this IQueryable<TSource> source, TSource item, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static int Count<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static int Count<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static IQueryable<TSource?> DefaultIfEmpty<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static IQueryable<TSource> DefaultIfEmpty<TSource>(this IQueryable<TSource> source, TSource defaultValue)
	{
		throw null;
	}

	public static IQueryable<TSource> DistinctBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static IQueryable<TSource> DistinctBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IQueryable<TSource> Distinct<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static IQueryable<TSource> Distinct<TSource>(this IQueryable<TSource> source, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static TSource? ElementAtOrDefault<TSource>(this IQueryable<TSource> source, Index index)
	{
		throw null;
	}

	public static TSource? ElementAtOrDefault<TSource>(this IQueryable<TSource> source, int index)
	{
		throw null;
	}

	public static TSource ElementAt<TSource>(this IQueryable<TSource> source, Index index)
	{
		throw null;
	}

	public static TSource ElementAt<TSource>(this IQueryable<TSource> source, int index)
	{
		throw null;
	}

	public static IQueryable<TSource> ExceptBy<TSource, TKey>(this IQueryable<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static IQueryable<TSource> ExceptBy<TSource, TKey>(this IQueryable<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IQueryable<TSource> Except<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2)
	{
		throw null;
	}

	public static IQueryable<TSource> Except<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static TSource? FirstOrDefault<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static TSource? FirstOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static TSource FirstOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue)
	{
		throw null;
	}

	public static TSource FirstOrDefault<TSource>(this IQueryable<TSource> source, TSource defaultValue)
	{
		throw null;
	}

	public static TSource First<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static TSource First<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static IQueryable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static IQueryable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IQueryable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
	{
		throw null;
	}

	public static IQueryable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector)
	{
		throw null;
	}

	public static IQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
	{
		throw null;
	}

	public static IQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
	{
		throw null;
	}

	public static IQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IQueryable<TSource> IntersectBy<TSource, TKey>(this IQueryable<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static IQueryable<TSource> IntersectBy<TSource, TKey>(this IQueryable<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IQueryable<TSource> Intersect<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2)
	{
		throw null;
	}

	public static IQueryable<TSource> Intersect<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static IQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
	{
		throw null;
	}

	public static IQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static TSource? LastOrDefault<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static TSource? LastOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static TSource LastOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue)
	{
		throw null;
	}

	public static TSource LastOrDefault<TSource>(this IQueryable<TSource> source, TSource defaultValue)
	{
		throw null;
	}

	public static TSource Last<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static TSource Last<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static long LongCount<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static long LongCount<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static TSource? MaxBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static TSource? MaxBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TSource>? comparer)
	{
		throw null;
	}

	public static TSource? Max<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static TSource? Max<TSource>(this IQueryable<TSource> source, IComparer<TSource>? comparer)
	{
		throw null;
	}

	public static TResult? Max<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
	{
		throw null;
	}

	public static TSource? MinBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static TSource? MinBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TSource>? comparer)
	{
		throw null;
	}

	public static TSource? Min<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static TSource? Min<TSource>(this IQueryable<TSource> source, IComparer<TSource>? comparer)
	{
		throw null;
	}

	public static TResult? Min<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
	{
		throw null;
	}

	public static IQueryable<TResult> OfType<TResult>(this IQueryable source)
	{
		throw null;
	}

	public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IOrderedQueryable<T> OrderDescending<T>(this IQueryable<T> source)
	{
		throw null;
	}

	public static IOrderedQueryable<T> OrderDescending<T>(this IQueryable<T> source, IComparer<T> comparer)
	{
		throw null;
	}

	public static IOrderedQueryable<T> Order<T>(this IQueryable<T> source)
	{
		throw null;
	}

	public static IOrderedQueryable<T> Order<T>(this IQueryable<T> source, IComparer<T> comparer)
	{
		throw null;
	}

	public static IQueryable<TSource> Prepend<TSource>(this IQueryable<TSource> source, TSource element)
	{
		throw null;
	}

	public static IQueryable<TSource> Reverse<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static IQueryable<TResult> SelectMany<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
	{
		throw null;
	}

	public static IQueryable<TResult> SelectMany<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector)
	{
		throw null;
	}

	public static IQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
	{
		throw null;
	}

	public static IQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
	{
		throw null;
	}

	public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, int, TResult>> selector)
	{
		throw null;
	}

	public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
	{
		throw null;
	}

	public static bool SequenceEqual<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2)
	{
		throw null;
	}

	public static bool SequenceEqual<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static TSource? SingleOrDefault<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static TSource? SingleOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static TSource SingleOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue)
	{
		throw null;
	}

	public static TSource SingleOrDefault<TSource>(this IQueryable<TSource> source, TSource defaultValue)
	{
		throw null;
	}

	public static TSource Single<TSource>(this IQueryable<TSource> source)
	{
		throw null;
	}

	public static TSource Single<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static IQueryable<TSource> SkipLast<TSource>(this IQueryable<TSource> source, int count)
	{
		throw null;
	}

	public static IQueryable<TSource> SkipWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static IQueryable<TSource> SkipWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
	{
		throw null;
	}

	public static IQueryable<TSource> Skip<TSource>(this IQueryable<TSource> source, int count)
	{
		throw null;
	}

	public static decimal Sum(this IQueryable<decimal> source)
	{
		throw null;
	}

	public static double Sum(this IQueryable<double> source)
	{
		throw null;
	}

	public static int Sum(this IQueryable<int> source)
	{
		throw null;
	}

	public static long Sum(this IQueryable<long> source)
	{
		throw null;
	}

	public static decimal? Sum(this IQueryable<decimal?> source)
	{
		throw null;
	}

	public static double? Sum(this IQueryable<double?> source)
	{
		throw null;
	}

	public static int? Sum(this IQueryable<int?> source)
	{
		throw null;
	}

	public static long? Sum(this IQueryable<long?> source)
	{
		throw null;
	}

	public static float? Sum(this IQueryable<float?> source)
	{
		throw null;
	}

	public static float Sum(this IQueryable<float> source)
	{
		throw null;
	}

	public static decimal Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
	{
		throw null;
	}

	public static double Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
	{
		throw null;
	}

	public static int Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
	{
		throw null;
	}

	public static long Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
	{
		throw null;
	}

	public static decimal? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
	{
		throw null;
	}

	public static double? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
	{
		throw null;
	}

	public static int? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
	{
		throw null;
	}

	public static long? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
	{
		throw null;
	}

	public static float? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
	{
		throw null;
	}

	public static float Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
	{
		throw null;
	}

	public static IQueryable<TSource> TakeLast<TSource>(this IQueryable<TSource> source, int count)
	{
		throw null;
	}

	public static IQueryable<TSource> TakeWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static IQueryable<TSource> TakeWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
	{
		throw null;
	}

	public static IQueryable<TSource> Take<TSource>(this IQueryable<TSource> source, int count)
	{
		throw null;
	}

	public static IQueryable<TSource> Take<TSource>(this IQueryable<TSource> source, Range range)
	{
		throw null;
	}

	public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IQueryable<TSource> UnionBy<TSource, TKey>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Expression<Func<TSource, TKey>> keySelector)
	{
		throw null;
	}

	public static IQueryable<TSource> UnionBy<TSource, TKey>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
	{
		throw null;
	}

	public static IQueryable<TSource> Union<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2)
	{
		throw null;
	}

	public static IQueryable<TSource> Union<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer)
	{
		throw null;
	}

	public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
	{
		throw null;
	}

	public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
	{
		throw null;
	}

	public static IQueryable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IQueryable<TFirst> source1, IEnumerable<TSecond> source2)
	{
		throw null;
	}

	public static IQueryable<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(this IQueryable<TFirst> source1, IEnumerable<TSecond> source2, IEnumerable<TThird> source3)
	{
		throw null;
	}

	public static IQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IQueryable<TFirst> source1, IEnumerable<TSecond> source2, Expression<Func<TFirst, TSecond, TResult>> resultSelector)
	{
		throw null;
	}
}
