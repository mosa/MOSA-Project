namespace System.Threading.RateLimiting;

public abstract class ReplenishingRateLimiter : RateLimiter
{
	public abstract bool IsAutoReplenishing { get; }

	public abstract TimeSpan ReplenishmentPeriod { get; }

	public abstract bool TryReplenish();
}
