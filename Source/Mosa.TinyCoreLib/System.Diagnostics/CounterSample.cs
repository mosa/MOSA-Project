namespace System.Diagnostics;

public readonly struct CounterSample : IEquatable<CounterSample>
{
	private readonly int _dummyPrimitive;

	public static CounterSample Empty;

	public long BaseValue
	{
		get
		{
			throw null;
		}
	}

	public long CounterFrequency
	{
		get
		{
			throw null;
		}
	}

	public long CounterTimeStamp
	{
		get
		{
			throw null;
		}
	}

	public PerformanceCounterType CounterType
	{
		get
		{
			throw null;
		}
	}

	public long RawValue
	{
		get
		{
			throw null;
		}
	}

	public long SystemFrequency
	{
		get
		{
			throw null;
		}
	}

	public long TimeStamp
	{
		get
		{
			throw null;
		}
	}

	public long TimeStamp100nSec
	{
		get
		{
			throw null;
		}
	}

	public CounterSample(long rawValue, long baseValue, long counterFrequency, long systemFrequency, long timeStamp, long timeStamp100nSec, PerformanceCounterType counterType)
	{
		throw null;
	}

	public CounterSample(long rawValue, long baseValue, long counterFrequency, long systemFrequency, long timeStamp, long timeStamp100nSec, PerformanceCounterType counterType, long counterTimeStamp)
	{
		throw null;
	}

	public static float Calculate(CounterSample counterSample)
	{
		throw null;
	}

	public static float Calculate(CounterSample counterSample, CounterSample nextCounterSample)
	{
		throw null;
	}

	public bool Equals(CounterSample sample)
	{
		throw null;
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CounterSample a, CounterSample b)
	{
		throw null;
	}

	public static bool operator !=(CounterSample a, CounterSample b)
	{
		throw null;
	}
}
