namespace System.Diagnostics.PerformanceData;

public sealed class CounterSetInstanceCounterDataSet : IDisposable
{
	public CounterData this[int counterId]
	{
		get
		{
			throw null;
		}
	}

	public CounterData this[string counterName]
	{
		get
		{
			throw null;
		}
	}

	internal CounterSetInstanceCounterDataSet()
	{
	}

	public void Dispose()
	{
	}

	~CounterSetInstanceCounterDataSet()
	{
	}
}
