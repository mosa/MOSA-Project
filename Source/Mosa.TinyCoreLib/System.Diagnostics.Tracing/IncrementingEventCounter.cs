using System.Runtime.Versioning;

namespace System.Diagnostics.Tracing;

[UnsupportedOSPlatform("browser")]
public class IncrementingEventCounter : DiagnosticCounter
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

	public IncrementingEventCounter(string name, EventSource eventSource)
	{
	}

	public void Increment(double increment = 1.0)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
