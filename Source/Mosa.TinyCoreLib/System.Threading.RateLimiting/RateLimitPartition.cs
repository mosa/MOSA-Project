namespace System.Threading.RateLimiting;

public static class RateLimitPartition
{
	public static RateLimitPartition<TKey> GetConcurrencyLimiter<TKey>(TKey partitionKey, Func<TKey, ConcurrencyLimiterOptions> factory)
	{
		throw null;
	}

	public static RateLimitPartition<TKey> GetFixedWindowLimiter<TKey>(TKey partitionKey, Func<TKey, FixedWindowRateLimiterOptions> factory)
	{
		throw null;
	}

	public static RateLimitPartition<TKey> GetNoLimiter<TKey>(TKey partitionKey)
	{
		throw null;
	}

	public static RateLimitPartition<TKey> GetSlidingWindowLimiter<TKey>(TKey partitionKey, Func<TKey, SlidingWindowRateLimiterOptions> factory)
	{
		throw null;
	}

	public static RateLimitPartition<TKey> GetTokenBucketLimiter<TKey>(TKey partitionKey, Func<TKey, TokenBucketRateLimiterOptions> factory)
	{
		throw null;
	}

	public static RateLimitPartition<TKey> Get<TKey>(TKey partitionKey, Func<TKey, RateLimiter> factory)
	{
		throw null;
	}
}
public struct RateLimitPartition<TKey>
{
	private readonly TKey _PartitionKey_k__BackingField;

	private object _dummy;

	private int _dummyPrimitive;

	public readonly Func<TKey, RateLimiter> Factory
	{
		get
		{
			throw null;
		}
	}

	public readonly TKey PartitionKey
	{
		get
		{
			throw null;
		}
	}

	public RateLimitPartition(TKey partitionKey, Func<TKey, RateLimiter> factory)
	{
		throw null;
	}
}
