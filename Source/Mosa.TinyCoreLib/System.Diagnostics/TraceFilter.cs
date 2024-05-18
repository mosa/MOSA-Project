using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public abstract class TraceFilter
{
	public abstract bool ShouldTrace(TraceEventCache? cache, string source, TraceEventType eventType, int id, [StringSyntax("CompositeFormat")] string? formatOrMessage, object?[]? args, object? data1, object?[]? data);
}
