namespace System.DirectoryServices.ActiveDirectory;

public class AdamInstance : DirectoryServer
{
	public ConfigurationSet ConfigurationSet
	{
		get
		{
			throw null;
		}
	}

	public string? DefaultPartition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string HostName
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

	public int LdapPort
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

	public AdamRoleCollection Roles
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

	public int SslPort
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

	internal AdamInstance()
	{
	}

	public override void CheckReplicationConsistency()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	~AdamInstance()
	{
	}

	public static AdamInstanceCollection FindAll(DirectoryContext context, string partitionName)
	{
		throw null;
	}

	public static AdamInstance FindOne(DirectoryContext context, string partitionName)
	{
		throw null;
	}

	public static AdamInstance GetAdamInstance(DirectoryContext context)
	{
		throw null;
	}

	public override ReplicationNeighborCollection GetAllReplicationNeighbors()
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

	public void Save()
	{
	}

	public void SeizeRoleOwnership(AdamRole role)
	{
	}

	public override void SyncReplicaFromAllServers(string partition, SyncFromAllServersOptions options)
	{
	}

	public override void SyncReplicaFromServer(string partition, string sourceServer)
	{
	}

	public void TransferRoleOwnership(AdamRole role)
	{
	}

	public override void TriggerSyncReplicaFromNeighbors(string partition)
	{
	}
}
