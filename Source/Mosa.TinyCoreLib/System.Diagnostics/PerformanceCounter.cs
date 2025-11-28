using System.ComponentModel;

namespace System.Diagnostics;

public sealed class PerformanceCounter : Component, ISupportInitialize
{
	[Obsolete("PerformanceCounter.DefaultFileMappingSize has been deprecated and is not used. Use machine.config or an application configuration file to set the size of the PerformanceCounter file mapping instead.")]
	public static int DefaultFileMappingSize;

	public string CategoryName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string CounterHelp
	{
		get
		{
			throw null;
		}
	}

	public string CounterName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PerformanceCounterType CounterType
	{
		get
		{
			throw null;
		}
	}

	public PerformanceCounterInstanceLifetime InstanceLifetime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string InstanceName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string MachineName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long RawValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ReadOnly
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PerformanceCounter()
	{
	}

	public PerformanceCounter(string categoryName, string counterName)
	{
	}

	public PerformanceCounter(string categoryName, string counterName, bool readOnly)
	{
	}

	public PerformanceCounter(string categoryName, string counterName, string instanceName)
	{
	}

	public PerformanceCounter(string categoryName, string counterName, string instanceName, bool readOnly)
	{
	}

	public PerformanceCounter(string categoryName, string counterName, string instanceName, string machineName)
	{
	}

	public void BeginInit()
	{
	}

	public void Close()
	{
	}

	public static void CloseSharedResources()
	{
	}

	public long Decrement()
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public void EndInit()
	{
	}

	public long Increment()
	{
		throw null;
	}

	public long IncrementBy(long value)
	{
		throw null;
	}

	public CounterSample NextSample()
	{
		throw null;
	}

	public float NextValue()
	{
		throw null;
	}

	public void RemoveInstance()
	{
	}
}
