using System.Collections;

namespace System.Diagnostics;

public class InstanceDataCollectionCollection : DictionaryBase
{
	public InstanceDataCollection this[string counterName]
	{
		get
		{
			throw null;
		}
	}

	public ICollection Keys
	{
		get
		{
			throw null;
		}
	}

	public ICollection Values
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("This constructor has been deprecated. Use System.Diagnostics.PerformanceCounterCategory.ReadCategory() to get an instance of this collection instead.")]
	public InstanceDataCollectionCollection()
	{
	}

	public bool Contains(string counterName)
	{
		throw null;
	}

	public void CopyTo(InstanceDataCollection[] counters, int index)
	{
	}
}
