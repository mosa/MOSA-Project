namespace System.DirectoryServices.ActiveDirectory;

public abstract class DirectoryServer : IDisposable
{
	public abstract ReplicationConnectionCollection InboundConnections { get; }

	public abstract string? IPAddress { get; }

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public abstract ReplicationConnectionCollection OutboundConnections { get; }

	public ReadOnlyStringCollection Partitions
	{
		get
		{
			throw null;
		}
	}

	public abstract string SiteName { get; }

	public abstract SyncUpdateCallback? SyncFromAllServersCallback { get; set; }

	public abstract void CheckReplicationConsistency();

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~DirectoryServer()
	{
	}

	public abstract ReplicationNeighborCollection GetAllReplicationNeighbors();

	public DirectoryEntry GetDirectoryEntry()
	{
		throw null;
	}

	public abstract ReplicationFailureCollection GetReplicationConnectionFailures();

	public abstract ReplicationCursorCollection GetReplicationCursors(string partition);

	public abstract ActiveDirectoryReplicationMetadata GetReplicationMetadata(string objectPath);

	public abstract ReplicationNeighborCollection GetReplicationNeighbors(string partition);

	public abstract ReplicationOperationInformation GetReplicationOperationInformation();

	public void MoveToAnotherSite(string siteName)
	{
	}

	public abstract void SyncReplicaFromAllServers(string partition, SyncFromAllServersOptions options);

	public abstract void SyncReplicaFromServer(string partition, string sourceServer);

	public override string ToString()
	{
		throw null;
	}

	public abstract void TriggerSyncReplicaFromNeighbors(string partition);
}
