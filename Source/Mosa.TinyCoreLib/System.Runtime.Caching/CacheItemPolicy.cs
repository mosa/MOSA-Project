using System.Collections.ObjectModel;

namespace System.Runtime.Caching;

public class CacheItemPolicy
{
	public DateTimeOffset AbsoluteExpiration
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Collection<ChangeMonitor> ChangeMonitors
	{
		get
		{
			throw null;
		}
	}

	public CacheItemPriority Priority
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CacheEntryRemovedCallback RemovedCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan SlidingExpiration
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CacheEntryUpdateCallback UpdateCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
