using System.Threading;

namespace System;

public abstract class TimeProvider
{
	public static TimeProvider System { get; }

	public virtual TimeZoneInfo LocalTimeZone { get; }

	public virtual long TimestampFrequency { get; }

	protected TimeProvider()
	{
		throw null;
	}

	public virtual DateTimeOffset GetUtcNow()
	{
		throw null;
	}

	public DateTimeOffset GetLocalNow()
	{
		throw null;
	}

	public virtual long GetTimestamp()
	{
		throw null;
	}

	public TimeSpan GetElapsedTime(long startingTimestamp)
	{
		throw null;
	}

	public TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp)
	{
		throw null;
	}

	public virtual ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
	{
		throw null;
	}
}
