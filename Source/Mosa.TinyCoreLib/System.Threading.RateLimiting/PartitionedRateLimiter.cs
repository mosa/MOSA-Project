using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Threading.RateLimiting;

public static class PartitionedRateLimiter
{
	public static PartitionedRateLimiter<TResource> CreateChained<TResource>(params PartitionedRateLimiter<TResource>[] limiters)
	{
		throw null;
	}

	public static PartitionedRateLimiter<TResource> Create<TResource, TPartitionKey>(Func<TResource, RateLimitPartition<TPartitionKey>> partitioner, IEqualityComparer<TPartitionKey>? equalityComparer = null) where TPartitionKey : notnull
	{
		throw null;
	}
}
public abstract class PartitionedRateLimiter<TResource> : IAsyncDisposable, IDisposable
{
	public ValueTask<RateLimitLease> AcquireAsync(TResource resource, int permitCount = 1, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	protected abstract ValueTask<RateLimitLease> AcquireAsyncCore(TResource resource, int permitCount, CancellationToken cancellationToken);

	public RateLimitLease AttemptAcquire(TResource resource, int permitCount = 1)
	{
		throw null;
	}

	protected abstract RateLimitLease AttemptAcquireCore(TResource resource, int permitCount);

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public ValueTask DisposeAsync()
	{
		throw null;
	}

	protected virtual ValueTask DisposeAsyncCore()
	{
		throw null;
	}

	public abstract RateLimiterStatistics? GetStatistics(TResource resource);

	public PartitionedRateLimiter<TOuter> WithTranslatedKey<TOuter>(Func<TOuter, TResource> keyAdapter, bool leaveOpen)
	{
		throw null;
	}
}
