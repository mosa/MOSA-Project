using System.Collections.ObjectModel;

namespace System.Runtime.Caching;

public abstract class CacheEntryChangeMonitor : ChangeMonitor
{
	public abstract ReadOnlyCollection<string> CacheKeys { get; }

	public abstract DateTimeOffset LastModified { get; }

	public abstract string RegionName { get; }
}
