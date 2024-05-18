using System.Threading.Tasks;

namespace System.Threading.RateLimiting;

public sealed class SlidingWindowRateLimiter : ReplenishingRateLimiter
{
	public override TimeSpan? IdleDuration
	{
		get
		{
			throw null;
		}
	}

	public override bool IsAutoReplenishing
	{
		get
		{
			throw null;
		}
	}

	public override TimeSpan ReplenishmentPeriod
	{
		get
		{
			throw null;
		}
	}

	public SlidingWindowRateLimiter(SlidingWindowRateLimiterOptions options)
	{
	}

	protected override ValueTask<RateLimitLease> AcquireAsyncCore(int permitCount, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	protected override RateLimitLease AttemptAcquireCore(int permitCount)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	protected override ValueTask DisposeAsyncCore()
	{
		throw null;
	}

	public override RateLimiterStatistics? GetStatistics()
	{
		throw null;
	}

	public override bool TryReplenish()
	{
		throw null;
	}
}
