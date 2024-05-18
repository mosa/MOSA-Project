namespace System.Data;

public class MergeFailedEventArgs : EventArgs
{
	public string Conflict
	{
		get
		{
			throw null;
		}
	}

	public DataTable? Table
	{
		get
		{
			throw null;
		}
	}

	public MergeFailedEventArgs(DataTable? table, string conflict)
	{
	}
}
