namespace System.DirectoryServices.ActiveDirectory;

public class ApplicationPartition : ActiveDirectoryPartition
{
	public DirectoryServerCollection DirectoryServers
	{
		get
		{
			throw null;
		}
	}

	public string? SecurityReferenceDomain
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ApplicationPartition(DirectoryContext context, string distinguishedName)
	{
	}

	public ApplicationPartition(DirectoryContext context, string distinguishedName, string objectClass)
	{
	}

	public void Delete()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public ReadOnlyDirectoryServerCollection FindAllDirectoryServers()
	{
		throw null;
	}

	public ReadOnlyDirectoryServerCollection FindAllDirectoryServers(string siteName)
	{
		throw null;
	}

	public ReadOnlyDirectoryServerCollection FindAllDiscoverableDirectoryServers()
	{
		throw null;
	}

	public ReadOnlyDirectoryServerCollection FindAllDiscoverableDirectoryServers(string siteName)
	{
		throw null;
	}

	public static ApplicationPartition FindByName(DirectoryContext context, string distinguishedName)
	{
		throw null;
	}

	public DirectoryServer FindDirectoryServer()
	{
		throw null;
	}

	public DirectoryServer FindDirectoryServer(bool forceRediscovery)
	{
		throw null;
	}

	public DirectoryServer FindDirectoryServer(string siteName)
	{
		throw null;
	}

	public DirectoryServer FindDirectoryServer(string siteName, bool forceRediscovery)
	{
		throw null;
	}

	public static ApplicationPartition GetApplicationPartition(DirectoryContext context)
	{
		throw null;
	}

	public override DirectoryEntry GetDirectoryEntry()
	{
		throw null;
	}

	public void Save()
	{
	}
}
