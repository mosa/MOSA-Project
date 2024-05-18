using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class DomainControllerCollection : ReadOnlyCollectionBase
{
	public DomainController this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal DomainControllerCollection()
	{
	}

	public bool Contains(DomainController domainController)
	{
		throw null;
	}

	public void CopyTo(DomainController[] domainControllers, int index)
	{
	}

	public int IndexOf(DomainController domainController)
	{
		throw null;
	}
}
