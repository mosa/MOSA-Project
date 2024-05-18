using System.Runtime.InteropServices;

namespace System.Diagnostics;

public sealed class EventLogTraceListener : TraceListener
{
	public EventLog EventLog
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventLogTraceListener()
	{
	}

	public EventLogTraceListener(EventLog eventLog)
	{
	}

	public EventLogTraceListener(string source)
	{
	}

	public override void Close()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	[ComVisible(false)]
	public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, object data)
	{
	}

	[ComVisible(false)]
	public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, params object[] data)
	{
	}

	[ComVisible(false)]
	public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string message)
	{
	}

	[ComVisible(false)]
	public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args)
	{
	}

	public override void Write(string message)
	{
	}

	public override void WriteLine(string message)
	{
	}
}
