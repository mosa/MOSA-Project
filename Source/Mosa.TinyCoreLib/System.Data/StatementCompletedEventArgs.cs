namespace System.Data;

public sealed class StatementCompletedEventArgs : EventArgs
{
	public int RecordCount
	{
		get
		{
			throw null;
		}
	}

	public StatementCompletedEventArgs(int recordCount)
	{
	}
}
