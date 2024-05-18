using System.Collections.Generic;
using System.IO;

namespace System.Diagnostics.Eventing.Reader;

public class EventLogReader : IDisposable
{
	public int BatchSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IList<EventLogStatus> LogStatus
	{
		get
		{
			throw null;
		}
	}

	public EventLogReader(EventLogQuery eventQuery)
	{
	}

	public EventLogReader(EventLogQuery eventQuery, EventBookmark bookmark)
	{
	}

	public EventLogReader(string path)
	{
	}

	public EventLogReader(string path, PathType pathType)
	{
	}

	public void CancelReading()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public EventRecord ReadEvent()
	{
		throw null;
	}

	public EventRecord ReadEvent(TimeSpan timeout)
	{
		throw null;
	}

	public void Seek(EventBookmark bookmark)
	{
	}

	public void Seek(EventBookmark bookmark, long offset)
	{
	}

	public void Seek(SeekOrigin origin, long offset)
	{
	}
}
