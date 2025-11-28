namespace System.Diagnostics;

public class EntryWrittenEventArgs : EventArgs
{
	public EventLogEntry Entry
	{
		get
		{
			throw null;
		}
	}

	public EntryWrittenEventArgs()
	{
	}

	public EntryWrittenEventArgs(EventLogEntry entry)
	{
	}
}
