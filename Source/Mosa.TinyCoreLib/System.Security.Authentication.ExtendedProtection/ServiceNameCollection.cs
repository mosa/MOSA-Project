using System.Collections;

namespace System.Security.Authentication.ExtendedProtection;

public class ServiceNameCollection : ReadOnlyCollectionBase
{
	public ServiceNameCollection(ICollection items)
	{
	}

	public bool Contains(string? searchServiceName)
	{
		throw null;
	}

	public ServiceNameCollection Merge(IEnumerable serviceNames)
	{
		throw null;
	}

	public ServiceNameCollection Merge(string serviceName)
	{
		throw null;
	}
}
