using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Runtime.Caching;

public class MemoryCache : ObjectCache, IEnumerable, IDisposable
{
	public long CacheMemoryLimit
	{
		get
		{
			throw null;
		}
	}

	public static MemoryCache Default
	{
		get
		{
			throw null;
		}
	}

	public override DefaultCacheCapabilities DefaultCacheCapabilities
	{
		get
		{
			throw null;
		}
	}

	public override object this[string key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string Name
	{
		get
		{
			throw null;
		}
	}

	public long PhysicalMemoryLimit
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan PollingInterval
	{
		get
		{
			throw null;
		}
	}

	public MemoryCache(string name, NameValueCollection config = null)
	{
	}

	public MemoryCache(string name, NameValueCollection config, bool ignoreConfigSection)
	{
	}

	public override bool Add(CacheItem item, CacheItemPolicy policy)
	{
		throw null;
	}

	public override CacheItem AddOrGetExisting(CacheItem item, CacheItemPolicy policy)
	{
		throw null;
	}

	public override object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
	{
		throw null;
	}

	public override object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null)
	{
		throw null;
	}

	public override bool Contains(string key, string regionName = null)
	{
		throw null;
	}

	public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys, string regionName = null)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public override object Get(string key, string regionName = null)
	{
		throw null;
	}

	public override CacheItem GetCacheItem(string key, string regionName = null)
	{
		throw null;
	}

	public override long GetCount(string regionName = null)
	{
		throw null;
	}

	protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
	{
		throw null;
	}

	public long GetLastSize(string regionName = null)
	{
		throw null;
	}

	public override IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null)
	{
		throw null;
	}

	public object Remove(string key, CacheEntryRemovedReason reason, string regionName = null)
	{
		throw null;
	}

	public override object Remove(string key, string regionName = null)
	{
		throw null;
	}

	public override void Set(CacheItem item, CacheItemPolicy policy)
	{
	}

	public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
	{
	}

	public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public long Trim(int percent)
	{
		throw null;
	}
}
