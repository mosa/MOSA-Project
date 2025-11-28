namespace System.Diagnostics.PerformanceData;

public class CounterSet : IDisposable
{
	public CounterSet(Guid providerGuid, Guid counterSetGuid, CounterSetInstanceType instanceType)
	{
	}

	public void AddCounter(int counterId, CounterType counterType)
	{
	}

	public void AddCounter(int counterId, CounterType counterType, string counterName)
	{
	}

	public CounterSetInstance CreateCounterSetInstance(string instanceName)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~CounterSet()
	{
	}
}
