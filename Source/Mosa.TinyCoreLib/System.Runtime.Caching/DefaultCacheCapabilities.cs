namespace System.Runtime.Caching;

[Flags]
public enum DefaultCacheCapabilities
{
	None = 0,
	InMemoryProvider = 1,
	OutOfProcessProvider = 2,
	CacheEntryChangeMonitors = 4,
	AbsoluteExpirations = 8,
	SlidingExpirations = 0x10,
	CacheEntryUpdateCallback = 0x20,
	CacheEntryRemovedCallback = 0x40,
	CacheRegions = 0x80
}
