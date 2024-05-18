namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySiteLinkBridge : IDisposable
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectorySiteLinkCollection SiteLinks
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectoryTransportType TransportType
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectorySiteLinkBridge(DirectoryContext context, string bridgeName)
	{
	}

	public ActiveDirectorySiteLinkBridge(DirectoryContext context, string bridgeName, ActiveDirectoryTransportType transport)
	{
	}

	public void Delete()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public static ActiveDirectorySiteLinkBridge FindByName(DirectoryContext context, string bridgeName)
	{
		throw null;
	}

	public static ActiveDirectorySiteLinkBridge FindByName(DirectoryContext context, string bridgeName, ActiveDirectoryTransportType transport)
	{
		throw null;
	}

	public DirectoryEntry GetDirectoryEntry()
	{
		throw null;
	}

	public void Save()
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
