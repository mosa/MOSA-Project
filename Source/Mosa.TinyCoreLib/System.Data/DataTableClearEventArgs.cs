namespace System.Data;

public sealed class DataTableClearEventArgs : EventArgs
{
	public DataTable Table
	{
		get
		{
			throw null;
		}
	}

	public string TableName
	{
		get
		{
			throw null;
		}
	}

	public string TableNamespace
	{
		get
		{
			throw null;
		}
	}

	public DataTableClearEventArgs(DataTable dataTable)
	{
	}
}
