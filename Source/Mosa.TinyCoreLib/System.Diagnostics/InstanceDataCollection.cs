using System.Collections;

namespace System.Diagnostics;

public class InstanceDataCollection : DictionaryBase
{
	public string CounterName
	{
		get
		{
			throw null;
		}
	}

	public InstanceData this[string instanceName]
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

	[Obsolete("This constructor has been deprecated. Use System.Diagnostics.InstanceDataCollectionCollection.get_Item to get an instance of this collection instead.")]
	public InstanceDataCollection(string counterName)
	{
	}

	public bool Contains(string instanceName)
	{
		throw null;
	}

	public void CopyTo(InstanceData[] instances, int index)
	{
	}
}
