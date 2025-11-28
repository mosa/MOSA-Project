namespace System.Net.Cache;

public enum RequestCacheLevel
{
	Default,
	BypassCache,
	CacheOnly,
	CacheIfAvailable,
	Revalidate,
	Reload,
	NoCacheNoStore
}
