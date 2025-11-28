namespace System.Data;

public class DataRowChangeEventArgs : EventArgs
{
	public DataRowAction Action
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

	public DataRowChangeEventArgs(DataRow row, DataRowAction action)
	{
	}
}
