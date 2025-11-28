using System.Diagnostics.CodeAnalysis;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySite : IDisposable
{
	public ReadOnlySiteCollection AdjacentSites
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyDirectoryServerCollection BridgeheadServers
	{
		get
		{
			throw null;
		}
	}

	public DomainCollection Domains
	{
		get
		{
			throw null;
		}
	}

	public DirectoryServer? InterSiteTopologyGenerator
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public ActiveDirectorySchedule? IntraSiteReplicationSchedule
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Location
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectorySiteOptions Options
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectoryServerCollection PreferredRpcBridgeheadServers
	{
		get
		{
			throw null;
		}
	}

	public DirectoryServerCollection PreferredSmtpBridgeheadServers
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyDirectoryServerCollection Servers
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

	public ActiveDirectorySubnetCollection Subnets
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectorySite(DirectoryContext context, string siteName)
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

	public static ActiveDirectorySite FindByName(DirectoryContext context, string siteName)
	{
		throw null;
	}

	public static ActiveDirectorySite GetComputerSite()
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
