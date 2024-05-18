namespace System.Diagnostics.Metrics;

public interface IMeterFactory : IDisposable
{
	Meter Create(MeterOptions options);
}
