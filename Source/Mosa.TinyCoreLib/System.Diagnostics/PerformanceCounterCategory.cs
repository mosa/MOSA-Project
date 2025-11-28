namespace System.Diagnostics;

public sealed class PerformanceCounterCategory
{
	public string CategoryHelp
	{
		get
		{
			throw null;
		}
	}

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

	public PerformanceCounterCategoryType CategoryType
	{
		get
		{
			throw null;
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

	public PerformanceCounterCategory()
	{
	}

	public PerformanceCounterCategory(string categoryName)
	{
	}

	public PerformanceCounterCategory(string categoryName, string machineName)
	{
	}

	public bool CounterExists(string counterName)
	{
		throw null;
	}

	public static bool CounterExists(string counterName, string categoryName)
	{
		throw null;
	}

	public static bool CounterExists(string counterName, string categoryName, string machineName)
	{
		throw null;
	}

	[Obsolete("This overload of PerformanceCounterCategory.Create has been deprecated. Use System.Diagnostics.PerformanceCounterCategory.Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, CounterCreationDataCollection counterData) instead.")]
	public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, CounterCreationDataCollection counterData)
	{
		throw null;
	}

	public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, CounterCreationDataCollection counterData)
	{
		throw null;
	}

	public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, string counterName, string counterHelp)
	{
		throw null;
	}

	[Obsolete("This overload of PerformanceCounterCategory.Create has been deprecated. Use System.Diagnostics.PerformanceCounterCategory.Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, string counterName, string counterHelp) instead.")]
	public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, string counterName, string counterHelp)
	{
		throw null;
	}

	public static void Delete(string categoryName)
	{
	}

	public static bool Exists(string categoryName)
	{
		throw null;
	}

	public static bool Exists(string categoryName, string machineName)
	{
		throw null;
	}

	public static PerformanceCounterCategory[] GetCategories()
	{
		throw null;
	}

	public static PerformanceCounterCategory[] GetCategories(string machineName)
	{
		throw null;
	}

	public PerformanceCounter[] GetCounters()
	{
		throw null;
	}

	public PerformanceCounter[] GetCounters(string instanceName)
	{
		throw null;
	}

	public string[] GetInstanceNames()
	{
		throw null;
	}

	public bool InstanceExists(string instanceName)
	{
		throw null;
	}

	public static bool InstanceExists(string instanceName, string categoryName)
	{
		throw null;
	}

	public static bool InstanceExists(string instanceName, string categoryName, string machineName)
	{
		throw null;
	}

	public InstanceDataCollectionCollection ReadCategory()
	{
		throw null;
	}
}
