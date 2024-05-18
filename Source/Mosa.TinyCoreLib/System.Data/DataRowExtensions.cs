namespace System.Data;

public static class DataRowExtensions
{
	public static T? Field<T>(this DataRow row, DataColumn column)
	{
		throw null;
	}

	public static T? Field<T>(this DataRow row, DataColumn column, DataRowVersion version)
	{
		throw null;
	}

	public static T? Field<T>(this DataRow row, int columnIndex)
	{
		throw null;
	}

	public static T? Field<T>(this DataRow row, int columnIndex, DataRowVersion version)
	{
		throw null;
	}

	public static T? Field<T>(this DataRow row, string columnName)
	{
		throw null;
	}

	public static T? Field<T>(this DataRow row, string columnName, DataRowVersion version)
	{
		throw null;
	}

	public static void SetField<T>(this DataRow row, DataColumn column, T? value)
	{
	}

	public static void SetField<T>(this DataRow row, int columnIndex, T? value)
	{
	}

	public static void SetField<T>(this DataRow row, string columnName, T? value)
	{
	}
}
