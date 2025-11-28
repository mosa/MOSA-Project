using System.Runtime.Versioning;

namespace System.Diagnostics.Tracing;

[UnsupportedOSPlatform("browser")]
public class PollingCounter : DiagnosticCounter
{
	public PollingCounter(string name, EventSource eventSource, Func<double> metricProvider)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
