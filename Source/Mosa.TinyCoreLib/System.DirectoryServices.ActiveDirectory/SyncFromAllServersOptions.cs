namespace System.DirectoryServices.ActiveDirectory;

[Flags]
public enum SyncFromAllServersOptions
{
	None = 0,
	AbortIfServerUnavailable = 1,
	SyncAdjacentServerOnly = 2,
	CheckServerAlivenessOnly = 8,
	SkipInitialCheck = 0x10,
	PushChangeOutward = 0x20,
	CrossSite = 0x40
}
