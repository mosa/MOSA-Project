using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public class TraceSource
{
	public StringDictionary Attributes
	{
		get
		{
			throw null;
		}
	}

	public SourceLevels DefaultLevel
	{
		get
		{
			throw null;
		}
	}

	public TraceListenerCollection Listeners
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public SourceSwitch Switch
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static event EventHandler<InitializingTraceSourceEventArgs>? Initializing
	{
		add
		{
		}
		remove
		{
		}
	}

	public TraceSource(string name)
	{
	}

	public TraceSource(string name, SourceLevels defaultLevel)
	{
	}

	public void Close()
	{
	}

	public void Flush()
	{
	}

	protected virtual string[]? GetSupportedAttributes()
	{
		throw null;
	}

	[Conditional("TRACE")]
	public void TraceData(TraceEventType eventType, int id, object? data)
	{
	}

	[Conditional("TRACE")]
	public void TraceData(TraceEventType eventType, int id, params object?[]? data)
	{
	}

	[Conditional("TRACE")]
	public void TraceEvent(TraceEventType eventType, int id)
	{
	}

	[Conditional("TRACE")]
	public void TraceEvent(TraceEventType eventType, int id, string? message)
	{
	}

	[Conditional("TRACE")]
	public void TraceEvent(TraceEventType eventType, int id, [StringSyntax("CompositeFormat")] string? format, params object?[]? args)
	{
	}

	[Conditional("TRACE")]
	public void TraceInformation(string? message)
	{
	}

	[Conditional("TRACE")]
	public void TraceInformation([StringSyntax("CompositeFormat")] string? format, params object?[]? args)
	{
	}

	[Conditional("TRACE")]
	public void TraceTransfer(int id, string? message, Guid relatedActivityId)
	{
	}
}
