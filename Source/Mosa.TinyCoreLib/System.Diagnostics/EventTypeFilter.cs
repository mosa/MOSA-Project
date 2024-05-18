using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public class EventTypeFilter : TraceFilter
{
	public SourceLevels EventType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventTypeFilter(SourceLevels level)
	{
	}

	public override bool ShouldTrace(TraceEventCache? cache, string source, TraceEventType eventType, int id, [StringSyntax("CompositeFormat")] string? formatOrMessage, object?[]? args, object? data1, object?[]? data)
	{
		throw null;
	}
}
