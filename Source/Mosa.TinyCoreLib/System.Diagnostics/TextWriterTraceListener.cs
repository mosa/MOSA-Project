using System.IO;

namespace System.Diagnostics;

public class TextWriterTraceListener : TraceListener
{
	public TextWriter? Writer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TextWriterTraceListener()
	{
	}

	public TextWriterTraceListener(Stream stream)
	{
	}

	public TextWriterTraceListener(Stream stream, string? name)
	{
	}

	public TextWriterTraceListener(TextWriter writer)
	{
	}

	public TextWriterTraceListener(TextWriter writer, string? name)
	{
	}

	public TextWriterTraceListener(string? fileName)
	{
	}

	public TextWriterTraceListener(string? fileName, string? name)
	{
	}

	public override void Close()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override void Flush()
	{
	}

	public override void Write(string? message)
	{
	}

	public override void WriteLine(string? message)
	{
	}
}
