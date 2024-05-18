namespace System.Runtime.Caching;

public class CacheEntryRemovedArguments
{
	public CacheItem CacheItem
	{
		get
		{
			throw null;
		}
	}

	public CacheEntryRemovedReason RemovedReason
	{
		get
		{
			throw null;
		}
	}

	public ObjectCache Source
	{
		get
		{
			throw null;
		}
	}

	public CacheEntryRemovedArguments(ObjectCache source, CacheEntryRemovedReason reason, CacheItem cacheItem)
	{
	}
}
