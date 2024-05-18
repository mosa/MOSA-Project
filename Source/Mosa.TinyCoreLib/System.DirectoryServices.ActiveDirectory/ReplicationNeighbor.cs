namespace System.DirectoryServices.ActiveDirectory;

public class ReplicationNeighbor
{
	[Flags]
	public enum ReplicationNeighborOptions : long
	{
		Writeable = 0x10L,
		SyncOnStartup = 0x20L,
		ScheduledSync = 0x40L,
		UseInterSiteTransport = 0x80L,
		TwoWaySync = 0x200L,
		ReturnObjectParent = 0x800L,
		FullSyncInProgress = 0x10000L,
		FullSyncNextPacket = 0x20000L,
		NeverSynced = 0x200000L,
		Preempted = 0x1000000L,
		IgnoreChangeNotifications = 0x4000000L,
		DisableScheduledSync = 0x8000000L,
		CompressChanges = 0x10000000L,
		NoChangeNotifications = 0x20000000L,
		PartialAttributeSet = 0x40000000L
	}

	public int ConsecutiveFailureCount
	{
		get
		{
			throw null;
		}
	}

	public DateTime LastAttemptedSync
	{
		get
		{
			throw null;
		}
	}

	public DateTime LastSuccessfulSync
	{
		get
		{
			throw null;
		}
	}

	public string LastSyncMessage
	{
		get
		{
			throw null;
		}
	}

	public int LastSyncResult
	{
		get
		{
			throw null;
		}
	}

	public string? PartitionName
	{
		get
		{
			throw null;
		}
	}

	public ReplicationNeighborOptions ReplicationNeighborOption
	{
		get
		{
			throw null;
		}
	}

	public Guid SourceInvocationId
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

	public long UsnAttributeFilter
	{
		get
		{
			throw null;
		}
	}

	public long UsnLastObjectChangeSynced
	{
		get
		{
			throw null;
		}
	}

	internal ReplicationNeighbor()
	{
	}
}
