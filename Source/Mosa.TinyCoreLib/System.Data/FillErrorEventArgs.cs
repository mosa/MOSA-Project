namespace System.Data;

public class FillErrorEventArgs : EventArgs
{
	public bool Continue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DataTable? DataTable
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

	public object?[] Values
	{
		get
		{
			throw null;
		}
	}

	public FillErrorEventArgs(DataTable? dataTable, object?[]? values)
	{
	}
}
