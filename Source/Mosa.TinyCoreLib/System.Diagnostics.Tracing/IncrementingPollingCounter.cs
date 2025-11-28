using System.Runtime.Versioning;

namespace System.Diagnostics.Tracing;

[UnsupportedOSPlatform("browser")]
public class IncrementingPollingCounter : DiagnosticCounter
{
	public TimeSpan DisplayRateTimeScale
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IncrementingPollingCounter(string name, EventSource eventSource, Func<double> totalValueProvider)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
