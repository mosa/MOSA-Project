namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectoryInterSiteTransport : IDisposable
{
	public bool BridgeAllSiteLinks
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IgnoreReplicationSchedule
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ReadOnlySiteLinkBridgeCollection SiteLinkBridges
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlySiteLinkCollection SiteLinks
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

	internal ActiveDirectoryInterSiteTransport()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public static ActiveDirectoryInterSiteTransport FindByTransportType(DirectoryContext context, ActiveDirectoryTransportType transport)
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
