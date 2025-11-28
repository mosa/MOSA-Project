using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

public class DataRow
{
	public bool HasErrors
	{
		get
		{
			throw null;
		}
	}

	public object this[DataColumn column]
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public object this[DataColumn column, DataRowVersion version]
	{
		get
		{
			throw null;
		}
	}

	public object this[int columnIndex]
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public object this[int columnIndex, DataRowVersion version]
	{
		get
		{
			throw null;
		}
	}

	public object this[string columnName]
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public object this[string columnName, DataRowVersion version]
	{
		get
		{
			throw null;
		}
	}

	public object?[] ItemArray
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string RowError
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public DataRowState RowState
	{
		get
		{
			throw null;
		}
	}

	public DataTable Table
	{
		get
		{
			throw null;
		}
	}

	protected internal DataRow(DataRowBuilder builder)
	{
	}

	public void AcceptChanges()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void BeginEdit()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void CancelEdit()
	{
	}

	public void ClearErrors()
	{
	}

	public void Delete()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndEdit()
	{
	}

	public DataRow[] GetChildRows(DataRelation? relation)
	{
		throw null;
	}

	public DataRow[] GetChildRows(DataRelation? relation, DataRowVersion version)
	{
		throw null;
	}

	public DataRow[] GetChildRows(string? relationName)
	{
		throw null;
	}

	public DataRow[] GetChildRows(string? relationName, DataRowVersion version)
	{
		throw null;
	}

	public string GetColumnError(DataColumn column)
	{
		throw null;
	}

	public string GetColumnError(int columnIndex)
	{
		throw null;
	}

	public string GetColumnError(string columnName)
	{
		throw null;
	}

	public DataColumn[] GetColumnsInError()
	{
		throw null;
	}

	public DataRow? GetParentRow(DataRelation? relation)
	{
		throw null;
	}

	public DataRow? GetParentRow(DataRelation? relation, DataRowVersion version)
	{
		throw null;
	}

	public DataRow? GetParentRow(string? relationName)
	{
		throw null;
	}

	public DataRow? GetParentRow(string? relationName, DataRowVersion version)
	{
		throw null;
	}

	public DataRow[] GetParentRows(DataRelation? relation)
	{
		throw null;
	}

	public DataRow[] GetParentRows(DataRelation? relation, DataRowVersion version)
	{
		throw null;
	}

	public DataRow[] GetParentRows(string? relationName)
	{
		throw null;
	}

	public DataRow[] GetParentRows(string? relationName, DataRowVersion version)
	{
		throw null;
	}

	public bool HasVersion(DataRowVersion version)
	{
		throw null;
	}

	public bool IsNull(DataColumn column)
	{
		throw null;
	}

	public bool IsNull(DataColumn column, DataRowVersion version)
	{
		throw null;
	}

	public bool IsNull(int columnIndex)
	{
		throw null;
	}

	public bool IsNull(string columnName)
	{
		throw null;
	}

	public void RejectChanges()
	{
	}

	public void SetAdded()
	{
	}

	public void SetColumnError(DataColumn column, string? error)
	{
	}

	public void SetColumnError(int columnIndex, string? error)
	{
	}

	public void SetColumnError(string columnName, string? error)
	{
	}

	public void SetModified()
	{
	}

	protected void SetNull(DataColumn column)
	{
	}

	public void SetParentRow(DataRow? parentRow)
	{
	}

	public void SetParentRow(DataRow? parentRow, DataRelation? relation)
	{
	}
}
