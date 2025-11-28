namespace System.Diagnostics.Eventing.Reader;

public sealed class EventRecordWrittenEventArgs : EventArgs
{
	public Exception EventException
	{
		get
		{
			throw null;
		}
	}

	public EventRecord EventRecord
	{
		get
		{
			throw null;
		}
	}

	internal EventRecordWrittenEventArgs()
	{
	}
}
