using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public class SourceFilter : TraceFilter
{
	public string Source
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SourceFilter(string source)
	{
	}

	public override bool ShouldTrace(TraceEventCache? cache, string source, TraceEventType eventType, int id, [StringSyntax("CompositeFormat")] string? formatOrMessage, object?[]? args, object? data1, object?[]? data)
	{
		throw null;
	}
}
