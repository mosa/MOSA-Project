namespace System.Runtime.Caching.Hosting;

public interface IMemoryCacheManager
{
	void ReleaseCache(MemoryCache cache);

	void UpdateCacheSize(long size, MemoryCache cache);
}
