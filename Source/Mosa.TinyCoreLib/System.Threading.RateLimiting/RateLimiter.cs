using System.Threading.Tasks;

namespace System.Threading.RateLimiting;

public abstract class RateLimiter : IAsyncDisposable, IDisposable
{
	public abstract TimeSpan? IdleDuration { get; }

	public ValueTask<RateLimitLease> AcquireAsync(int permitCount = 1, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	protected abstract ValueTask<RateLimitLease> AcquireAsyncCore(int permitCount, CancellationToken cancellationToken);

	public RateLimitLease AttemptAcquire(int permitCount = 1)
	{
		throw null;
	}

	protected abstract RateLimitLease AttemptAcquireCore(int permitCount);

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

	public abstract RateLimiterStatistics? GetStatistics();
}
