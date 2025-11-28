using System.Collections.Generic;

namespace System.Data;

public static class EnumerableRowCollectionExtensions
{
	public static EnumerableRowCollection<TResult> Cast<TResult>(this EnumerableRowCollection source)
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> OrderByDescending<TRow, TKey>(this EnumerableRowCollection<TRow> source, Func<TRow, TKey> keySelector)
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> OrderByDescending<TRow, TKey>(this EnumerableRowCollection<TRow> source, Func<TRow, TKey> keySelector, IComparer<TKey> comparer)
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> OrderBy<TRow, TKey>(this EnumerableRowCollection<TRow> source, Func<TRow, TKey> keySelector)
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> OrderBy<TRow, TKey>(this EnumerableRowCollection<TRow> source, Func<TRow, TKey> keySelector, IComparer<TKey> comparer)
	{
		throw null;
	}

	public static EnumerableRowCollection<S> Select<TRow, S>(this EnumerableRowCollection<TRow> source, Func<TRow, S> selector)
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> ThenByDescending<TRow, TKey>(this OrderedEnumerableRowCollection<TRow> source, Func<TRow, TKey> keySelector)
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> ThenByDescending<TRow, TKey>(this OrderedEnumerableRowCollection<TRow> source, Func<TRow, TKey> keySelector, IComparer<TKey> comparer)
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> ThenBy<TRow, TKey>(this OrderedEnumerableRowCollection<TRow> source, Func<TRow, TKey> keySelector)
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> ThenBy<TRow, TKey>(this OrderedEnumerableRowCollection<TRow> source, Func<TRow, TKey> keySelector, IComparer<TKey> comparer)
	{
		throw null;
	}

	public static EnumerableRowCollection<TRow> Where<TRow>(this EnumerableRowCollection<TRow> source, Func<TRow, bool> predicate)
	{
		throw null;
	}
}
