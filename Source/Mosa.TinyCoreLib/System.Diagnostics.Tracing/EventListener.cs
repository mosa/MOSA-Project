using System.Collections.Generic;

namespace System.Diagnostics.Tracing;

public abstract class EventListener : IDisposable
{
	public event EventHandler<EventSourceCreatedEventArgs>? EventSourceCreated
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<EventWrittenEventArgs>? EventWritten
	{
		add
		{
		}
		remove
		{
		}
	}

	public void DisableEvents(EventSource eventSource)
	{
	}

	public virtual void Dispose()
	{
	}

	public void EnableEvents(EventSource eventSource, EventLevel level)
	{
	}

	public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword)
	{
	}

	public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword, IDictionary<string, string?>? arguments)
	{
	}

	protected static int EventSourceIndex(EventSource eventSource)
	{
		throw null;
	}

	protected internal virtual void OnEventSourceCreated(EventSource eventSource)
	{
	}

	protected internal virtual void OnEventWritten(EventWrittenEventArgs eventData)
	{
	}
}
