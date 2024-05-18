namespace System.Data;

public sealed class DataTableNewRowEventArgs : EventArgs
{
	public DataRow Row
	{
		get
		{
			throw null;
		}
	}

	public DataTableNewRowEventArgs(DataRow dataRow)
	{
	}
}
