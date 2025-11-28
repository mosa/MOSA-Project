using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ReadOnlySiteCollection : ReadOnlyCollectionBase
{
	public ActiveDirectorySite this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal ReadOnlySiteCollection()
	{
	}

	public bool Contains(ActiveDirectorySite site)
	{
		throw null;
	}

	public void CopyTo(ActiveDirectorySite[] sites, int index)
	{
	}

	public int IndexOf(ActiveDirectorySite site)
	{
		throw null;
	}
}
