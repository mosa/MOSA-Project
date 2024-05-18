using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ReadOnlySiteLinkBridgeCollection : ReadOnlyCollectionBase
{
	public ActiveDirectorySiteLinkBridge this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal ReadOnlySiteLinkBridgeCollection()
	{
	}

	public bool Contains(ActiveDirectorySiteLinkBridge bridge)
	{
		throw null;
	}

	public void CopyTo(ActiveDirectorySiteLinkBridge[] bridges, int index)
	{
	}

	public int IndexOf(ActiveDirectorySiteLinkBridge bridge)
	{
		throw null;
	}
}
