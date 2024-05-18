using System.Collections.Generic;

namespace System.Data;

public static class TypedTableBaseExtensions
{
	public static EnumerableRowCollection<TRow> AsEnumerable<TRow>(this TypedTableBase<TRow> source) where TRow : DataRow
	{
		throw null;
	}

	public static TRow? ElementAtOrDefault<TRow>(this TypedTableBase<TRow> source, int index) where TRow : DataRow
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> OrderByDescending<TRow, TKey>(this TypedTableBase<TRow> source, Func<TRow, TKey> keySelector) where TRow : DataRow
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> OrderByDescending<TRow, TKey>(this TypedTableBase<TRow> source, Func<TRow, TKey> keySelector, IComparer<TKey> comparer) where TRow : DataRow
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> OrderBy<TRow, TKey>(this TypedTableBase<TRow> source, Func<TRow, TKey> keySelector) where TRow : DataRow
	{
		throw null;
	}

	public static OrderedEnumerableRowCollection<TRow> OrderBy<TRow, TKey>(this TypedTableBase<TRow> source, Func<TRow, TKey> keySelector, IComparer<TKey> comparer) where TRow : DataRow
	{
		throw null;
	}

	public static EnumerableRowCollection<S> Select<TRow, S>(this TypedTableBase<TRow> source, Func<TRow, S> selector) where TRow : DataRow
	{
		throw null;
	}

	public static EnumerableRowCollection<TRow> Where<TRow>(this TypedTableBase<TRow> source, Func<TRow, bool> predicate) where TRow : DataRow
	{
		throw null;
	}
}
