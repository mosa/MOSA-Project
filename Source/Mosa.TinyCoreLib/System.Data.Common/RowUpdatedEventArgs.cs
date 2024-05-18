namespace System.Data.Common;

public class RowUpdatedEventArgs : EventArgs
{
	public IDbCommand? Command
	{
		get
		{
			throw null;
		}
	}

	public Exception? Errors
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int RecordsAffected
	{
		get
		{
			throw null;
		}
	}

	public DataRow Row
	{
		get
		{
			throw null;
		}
	}

	public int RowCount
	{
		get
		{
			throw null;
		}
	}

	public StatementType StatementType
	{
		get
		{
			throw null;
		}
	}

	public UpdateStatus Status
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DataTableMapping TableMapping
	{
		get
		{
			throw null;
		}
	}

	public RowUpdatedEventArgs(DataRow dataRow, IDbCommand? command, StatementType statementType, DataTableMapping tableMapping)
	{
	}

	public void CopyToRows(DataRow[] array)
	{
	}

	public void CopyToRows(DataRow[] array, int arrayIndex)
	{
	}
}
