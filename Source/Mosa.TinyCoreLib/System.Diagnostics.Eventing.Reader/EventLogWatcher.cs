namespace System.Diagnostics.Eventing.Reader;

public class EventLogWatcher : IDisposable
{
	public bool Enabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event EventHandler<EventRecordWrittenEventArgs> EventRecordWritten
	{
		add
		{
		}
		remove
		{
		}
	}

	public EventLogWatcher(EventLogQuery eventQuery)
	{
	}

	public EventLogWatcher(EventLogQuery eventQuery, EventBookmark bookmark)
	{
	}

	public EventLogWatcher(EventLogQuery eventQuery, EventBookmark bookmark, bool readExistingEvents)
	{
	}

	public EventLogWatcher(string path)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}
}
