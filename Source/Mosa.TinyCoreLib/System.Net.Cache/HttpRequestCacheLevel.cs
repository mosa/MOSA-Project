namespace System.Net.Cache;

public enum HttpRequestCacheLevel
{
	Default,
	BypassCache,
	CacheOnly,
	CacheIfAvailable,
	Revalidate,
	Reload,
	NoCacheNoStore,
	CacheOrNextCacheOnly,
	Refresh
}
