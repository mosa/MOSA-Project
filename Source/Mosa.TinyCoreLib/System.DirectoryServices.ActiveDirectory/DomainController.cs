namespace System.DirectoryServices.ActiveDirectory;

public class DomainController : DirectoryServer
{
	public DateTime CurrentTime
	{
		get
		{
			throw null;
		}
	}

	public Domain Domain
	{
		get
		{
			throw null;
		}
	}

	public Forest Forest
	{
		get
		{
			throw null;
		}
	}

	public long HighestCommittedUsn
	{
		get
		{
			throw null;
		}
	}

	public override ReplicationConnectionCollection InboundConnections
	{
		get
		{
			throw null;
		}
	}

	public override string? IPAddress
	{
		get
		{
			throw null;
		}
	}

	public string OSVersion
	{
		get
		{
			throw null;
		}
	}

	public override ReplicationConnectionCollection OutboundConnections
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectoryRoleCollection Roles
	{
		get
		{
			throw null;
		}
	}

	public override string SiteName
	{
		get
		{
			throw null;
		}
	}

	public override SyncUpdateCallback? SyncFromAllServersCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected DomainController()
	{
	}

	public override void CheckReplicationConsistency()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public virtual GlobalCatalog EnableGlobalCatalog()
	{
		throw null;
	}

	~DomainController()
	{
	}

	public static DomainControllerCollection FindAll(DirectoryContext context)
	{
		throw null;
	}

	public static DomainControllerCollection FindAll(DirectoryContext context, string siteName)
	{
		throw null;
	}

	public static DomainController FindOne(DirectoryContext context)
	{
		throw null;
	}

	public static DomainController FindOne(DirectoryContext context, LocatorOptions flag)
	{
		throw null;
	}

	public static DomainController FindOne(DirectoryContext context, string siteName)
	{
		throw null;
	}

	public static DomainController FindOne(DirectoryContext context, string siteName, LocatorOptions flag)
	{
		throw null;
	}

	public override ReplicationNeighborCollection GetAllReplicationNeighbors()
	{
		throw null;
	}

	public virtual DirectorySearcher GetDirectorySearcher()
	{
		throw null;
	}

	public static DomainController GetDomainController(DirectoryContext context)
	{
		throw null;
	}

	public override ReplicationFailureCollection GetReplicationConnectionFailures()
	{
		throw null;
	}

	public override ReplicationCursorCollection GetReplicationCursors(string partition)
	{
		throw null;
	}

	public override ActiveDirectoryReplicationMetadata GetReplicationMetadata(string objectPath)
	{
		throw null;
	}

	public override ReplicationNeighborCollection GetReplicationNeighbors(string partition)
	{
		throw null;
	}

	public override ReplicationOperationInformation GetReplicationOperationInformation()
	{
		throw null;
	}

	public virtual bool IsGlobalCatalog()
	{
		throw null;
	}

	public void SeizeRoleOwnership(ActiveDirectoryRole role)
	{
	}

	public override void SyncReplicaFromAllServers(string partition, SyncFromAllServersOptions options)
	{
	}

	public override void SyncReplicaFromServer(string partition, string sourceServer)
	{
	}

	public void TransferRoleOwnership(ActiveDirectoryRole role)
	{
	}

	public override void TriggerSyncReplicaFromNeighbors(string partition)
	{
	}
}
