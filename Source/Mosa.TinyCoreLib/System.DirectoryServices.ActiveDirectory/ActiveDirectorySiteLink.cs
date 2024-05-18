namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySiteLink : IDisposable
{
	public int Cost
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool DataCompressionEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ActiveDirectorySchedule? InterSiteReplicationSchedule
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

	public bool NotificationEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ReciprocalReplicationEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan ReplicationInterval
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ActiveDirectorySiteCollection Sites
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

	public ActiveDirectorySiteLink(DirectoryContext context, string siteLinkName)
	{
	}

	public ActiveDirectorySiteLink(DirectoryContext context, string siteLinkName, ActiveDirectoryTransportType transport)
	{
	}

	public ActiveDirectorySiteLink(DirectoryContext context, string siteLinkName, ActiveDirectoryTransportType transport, ActiveDirectorySchedule? schedule)
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

	public static ActiveDirectorySiteLink FindByName(DirectoryContext context, string siteLinkName)
	{
		throw null;
	}

	public static ActiveDirectorySiteLink FindByName(DirectoryContext context, string siteLinkName, ActiveDirectoryTransportType transport)
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
