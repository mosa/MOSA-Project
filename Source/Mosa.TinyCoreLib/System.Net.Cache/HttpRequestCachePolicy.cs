namespace System.Net.Cache;

public class HttpRequestCachePolicy : RequestCachePolicy
{
	public DateTime CacheSyncDate
	{
		get
		{
			throw null;
		}
	}

	public new HttpRequestCacheLevel Level
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan MaxAge
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan MaxStale
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan MinFresh
	{
		get
		{
			throw null;
		}
	}

	public HttpRequestCachePolicy()
	{
	}

	public HttpRequestCachePolicy(DateTime cacheSyncDate)
	{
	}

	public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan ageOrFreshOrStale)
	{
	}

	public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale)
	{
	}

	public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale, DateTime cacheSyncDate)
	{
	}

	public HttpRequestCachePolicy(HttpRequestCacheLevel level)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
