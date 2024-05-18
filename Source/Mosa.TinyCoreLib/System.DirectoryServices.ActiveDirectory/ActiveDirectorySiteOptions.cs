namespace System.DirectoryServices.ActiveDirectory;

[Flags]
public enum ActiveDirectorySiteOptions
{
	None = 0,
	AutoTopologyDisabled = 1,
	TopologyCleanupDisabled = 2,
	AutoMinimumHopDisabled = 4,
	StaleServerDetectDisabled = 8,
	AutoInterSiteTopologyDisabled = 0x10,
	GroupMembershipCachingEnabled = 0x20,
	ForceKccWindows2003Behavior = 0x40,
	UseWindows2000IstgElection = 0x80,
	RandomBridgeHeaderServerSelectionDisabled = 0x100,
	UseHashingForReplicationSchedule = 0x200,
	RedundantServerTopologyEnabled = 0x400
}
