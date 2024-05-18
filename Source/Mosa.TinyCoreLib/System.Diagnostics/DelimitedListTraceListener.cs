using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Diagnostics;

public class DelimitedListTraceListener : TextWriterTraceListener
{
	public string Delimiter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DelimitedListTraceListener(Stream stream)
	{
	}

	public DelimitedListTraceListener(Stream stream, string? name)
	{
	}

	public DelimitedListTraceListener(TextWriter writer)
	{
	}

	public DelimitedListTraceListener(TextWriter writer, string? name)
	{
	}

	public DelimitedListTraceListener(string? fileName)
	{
	}

	public DelimitedListTraceListener(string? fileName, string? name)
	{
	}

	protected override string[] GetSupportedAttributes()
	{
		throw null;
	}

	public override void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, object? data)
	{
	}

	public override void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, params object?[]? data)
	{
	}

	public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
	{
	}

	public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, [StringSyntax("CompositeFormat")] string? format, params object?[]? args)
	{
	}
}
