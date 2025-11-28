namespace System.Runtime.Caching;

public class CacheEntryUpdateArguments
{
	public string Key
	{
		get
		{
			throw null;
		}
	}

	public string RegionName
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

	public CacheItem UpdatedCacheItem
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CacheItemPolicy UpdatedCacheItemPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CacheEntryUpdateArguments(ObjectCache source, CacheEntryRemovedReason reason, string key, string regionName)
	{
	}
}
