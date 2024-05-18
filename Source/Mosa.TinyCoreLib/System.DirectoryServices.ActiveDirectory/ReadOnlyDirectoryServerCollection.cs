using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ReadOnlyDirectoryServerCollection : ReadOnlyCollectionBase
{
	public DirectoryServer this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal ReadOnlyDirectoryServerCollection()
	{
	}

	public bool Contains(DirectoryServer directoryServer)
	{
		throw null;
	}

	public void CopyTo(DirectoryServer[] directoryServers, int index)
	{
	}

	public int IndexOf(DirectoryServer directoryServer)
	{
		throw null;
	}
}
