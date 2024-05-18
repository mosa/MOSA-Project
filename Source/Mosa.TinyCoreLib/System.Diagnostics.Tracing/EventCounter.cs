using System.Runtime.Versioning;

namespace System.Diagnostics.Tracing;

[UnsupportedOSPlatform("browser")]
public class EventCounter : DiagnosticCounter
{
	public EventCounter(string name, EventSource eventSource)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public void WriteMetric(double value)
	{
	}

	public void WriteMetric(float value)
	{
	}
}
