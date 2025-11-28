using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ReadOnlySiteLinkCollection : ReadOnlyCollectionBase
{
	public ActiveDirectorySiteLink this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal ReadOnlySiteLinkCollection()
	{
	}

	public bool Contains(ActiveDirectorySiteLink link)
	{
		throw null;
	}

	public void CopyTo(ActiveDirectorySiteLink[] links, int index)
	{
	}

	public int IndexOf(ActiveDirectorySiteLink link)
	{
		throw null;
	}
}
