using System.Collections;
using System.Collections.Generic;

namespace System.Runtime.Caching;

public abstract class ObjectCache : IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
	public static readonly DateTimeOffset InfiniteAbsoluteExpiration;

	public static readonly TimeSpan NoSlidingExpiration;

	public abstract DefaultCacheCapabilities DefaultCacheCapabilities { get; }

	public static IServiceProvider Host
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public abstract object this[string key] { get; set; }

	public abstract string Name { get; }

	public virtual bool Add(CacheItem item, CacheItemPolicy policy)
	{
		throw null;
	}

	public virtual bool Add(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
	{
		throw null;
	}

	public virtual bool Add(string key, object value, CacheItemPolicy policy, string regionName = null)
	{
		throw null;
	}

	public abstract CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy);

	public abstract object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);

	public abstract object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null);

	public abstract bool Contains(string key, string regionName = null);

	public abstract CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys, string regionName = null);

	public abstract object Get(string key, string regionName = null);

	public abstract CacheItem GetCacheItem(string key, string regionName = null);

	public abstract long GetCount(string regionName = null);

	protected abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();

	public abstract IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null);

	public virtual IDictionary<string, object> GetValues(string regionName, params string[] keys)
	{
		throw null;
	}

	public abstract object Remove(string key, string regionName = null);

	public abstract void Set(CacheItem item, CacheItemPolicy policy);

	public abstract void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);

	public abstract void Set(string key, object value, CacheItemPolicy policy, string regionName = null);

	IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
