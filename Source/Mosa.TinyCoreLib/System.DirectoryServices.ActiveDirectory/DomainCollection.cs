using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class DomainCollection : ReadOnlyCollectionBase
{
	public Domain this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal DomainCollection()
	{
	}

	public bool Contains(Domain domain)
	{
		throw null;
	}

	public void CopyTo(Domain[] domains, int index)
	{
	}

	public int IndexOf(Domain domain)
	{
		throw null;
	}
}
