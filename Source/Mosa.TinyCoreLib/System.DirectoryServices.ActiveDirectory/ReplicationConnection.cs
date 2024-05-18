namespace System.DirectoryServices.ActiveDirectory;

public class ReplicationConnection : IDisposable
{
	public NotificationStatus ChangeNotificationStatus
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

	public string DestinationServer
	{
		get
		{
			throw null;
		}
	}

	public bool Enabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool GeneratedByKcc
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

	public ActiveDirectorySchedule? ReplicationSchedule
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ReplicationScheduleOwnedByUser
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ReplicationSpan ReplicationSpan
	{
		get
		{
			throw null;
		}
	}

	public string? SourceServer
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

	public ReplicationConnection(DirectoryContext context, string name, DirectoryServer sourceServer)
	{
	}

	public ReplicationConnection(DirectoryContext context, string name, DirectoryServer sourceServer, ActiveDirectorySchedule? schedule)
	{
	}

	public ReplicationConnection(DirectoryContext context, string name, DirectoryServer sourceServer, ActiveDirectorySchedule? schedule, ActiveDirectoryTransportType transport)
	{
	}

	public ReplicationConnection(DirectoryContext context, string name, DirectoryServer sourceServer, ActiveDirectoryTransportType transport)
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

	~ReplicationConnection()
	{
	}

	public static ReplicationConnection FindByName(DirectoryContext context, string name)
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
