using System.Collections.ObjectModel;

namespace System.Diagnostics.Tracing;

public class EventWrittenEventArgs : EventArgs
{
	public Guid ActivityId
	{
		get
		{
			throw null;
		}
	}

	public EventChannel Channel
	{
		get
		{
			throw null;
		}
	}

	public int EventId
	{
		get
		{
			throw null;
		}
	}

	public string? EventName
	{
		get
		{
			throw null;
		}
	}

	public EventSource EventSource
	{
		get
		{
			throw null;
		}
	}

	public EventKeywords Keywords
	{
		get
		{
			throw null;
		}
	}

	public EventLevel Level
	{
		get
		{
			throw null;
		}
	}

	public string? Message
	{
		get
		{
			throw null;
		}
	}

	public EventOpcode Opcode
	{
		get
		{
			throw null;
		}
	}

	public long OSThreadId
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<object?>? Payload
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<string>? PayloadNames
	{
		get
		{
			throw null;
		}
	}

	public Guid RelatedActivityId
	{
		get
		{
			throw null;
		}
	}

	public EventTags Tags
	{
		get
		{
			throw null;
		}
	}

	public EventTask Task
	{
		get
		{
			throw null;
		}
	}

	public DateTime TimeStamp
	{
		get
		{
			throw null;
		}
	}

	public byte Version
	{
		get
		{
			throw null;
		}
	}

	internal EventWrittenEventArgs()
	{
	}
}
