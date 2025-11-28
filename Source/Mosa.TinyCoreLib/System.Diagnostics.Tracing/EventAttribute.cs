namespace System.Diagnostics.Tracing;

[AttributeUsage(AttributeTargets.Method)]
public sealed class EventAttribute : Attribute
{
	public EventActivityOptions ActivityOptions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventChannel Channel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int EventId
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
		set
		{
		}
	}

	public EventLevel Level
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Message
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventOpcode Opcode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventTags Tags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventTask Task
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte Version
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventAttribute(int eventId)
	{
	}
}
