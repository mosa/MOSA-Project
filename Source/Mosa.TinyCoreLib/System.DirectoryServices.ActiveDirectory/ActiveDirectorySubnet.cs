namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySubnet : IDisposable
{
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

	public ActiveDirectorySite? Site
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ActiveDirectorySubnet(DirectoryContext context, string subnetName)
	{
	}

	public ActiveDirectorySubnet(DirectoryContext context, string subnetName, string siteName)
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

	public static ActiveDirectorySubnet FindByName(DirectoryContext context, string subnetName)
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
