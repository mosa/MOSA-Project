using System.Collections.Generic;

namespace System.Diagnostics.Tracing;

public class EventCommandEventArgs : EventArgs
{
	public IDictionary<string, string?>? Arguments
	{
		get
		{
			throw null;
		}
	}

	public EventCommand Command
	{
		get
		{
			throw null;
		}
	}

	internal EventCommandEventArgs()
	{
	}

	public bool DisableEvent(int eventId)
	{
		throw null;
	}

	public bool EnableEvent(int eventId)
	{
		throw null;
	}
}
