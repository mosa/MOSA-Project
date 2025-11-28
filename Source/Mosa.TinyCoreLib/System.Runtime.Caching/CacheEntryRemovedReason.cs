namespace System.Runtime.Caching;

public enum CacheEntryRemovedReason
{
	Removed,
	Expired,
	Evicted,
	ChangeMonitorChanged,
	CacheSpecificEviction
}
