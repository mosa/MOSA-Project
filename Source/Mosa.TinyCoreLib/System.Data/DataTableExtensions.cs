using System.Collections.Generic;

namespace System.Data;

public static class DataTableExtensions
{
	public static DataView AsDataView(this DataTable table)
	{
		throw null;
	}

	public static DataView AsDataView<T>(this EnumerableRowCollection<T> source) where T : DataRow
	{
		throw null;
	}

	public static EnumerableRowCollection<DataRow> AsEnumerable(this DataTable source)
	{
		throw null;
	}

	public static DataTable CopyToDataTable<T>(this IEnumerable<T> source) where T : DataRow
	{
		throw null;
	}

	public static void CopyToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption options) where T : DataRow
	{
	}

	public static void CopyToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption options, FillErrorEventHandler? errorHandler) where T : DataRow
	{
	}
}
