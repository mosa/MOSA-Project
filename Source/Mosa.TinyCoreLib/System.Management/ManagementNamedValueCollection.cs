using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace System.Management;

public class ManagementNamedValueCollection : NameObjectCollectionBase
{
	public object this[string name]
	{
		get
		{
			throw null;
		}
	}

	public ManagementNamedValueCollection()
	{
	}

	protected ManagementNamedValueCollection(SerializationInfo info, StreamingContext context)
	{
	}

	public void Add(string name, object value)
	{
	}

	public ManagementNamedValueCollection Clone()
	{
		throw null;
	}

	public void Remove(string name)
	{
	}

	public void RemoveAll()
	{
	}
}
