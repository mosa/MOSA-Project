using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Diagnostics;

public class XmlWriterTraceListener : TextWriterTraceListener
{
	public XmlWriterTraceListener(Stream stream)
	{
	}

	public XmlWriterTraceListener(Stream stream, string? name)
	{
	}

	public XmlWriterTraceListener(TextWriter writer)
	{
	}

	public XmlWriterTraceListener(TextWriter writer, string? name)
	{
	}

	public XmlWriterTraceListener(string? filename)
	{
	}

	public XmlWriterTraceListener(string? filename, string? name)
	{
	}

	public override void Close()
	{
	}

	public override void Fail(string? message, string? detailMessage)
	{
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

	public override void TraceTransfer(TraceEventCache? eventCache, string source, int id, string? message, Guid relatedActivityId)
	{
	}

	public override void Write(string? message)
	{
	}

	public override void WriteLine(string? message)
	{
	}
}
