namespace System.Data;

public class DataColumnChangeEventArgs : EventArgs
{
	public DataColumn? Column
	{
		get
		{
			throw null;
		}
	}

	public object? ProposedValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DataRow Row
	{
		get
		{
			throw null;
		}
	}

	public DataColumnChangeEventArgs(DataRow row, DataColumn? column, object? value)
	{
	}
}
