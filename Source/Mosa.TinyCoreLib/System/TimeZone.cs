using System.Globalization;

namespace System;

[Obsolete("System.TimeZone has been deprecated. Investigate the use of System.TimeZoneInfo instead.")]
public abstract class TimeZone
{
	public static TimeZone CurrentTimeZone
	{
		get
		{
			throw null;
		}
	}

	public abstract string DaylightName { get; }

	public abstract string StandardName { get; }

	public abstract DaylightTime GetDaylightChanges(int year);

	public abstract TimeSpan GetUtcOffset(DateTime time);

	public virtual bool IsDaylightSavingTime(DateTime time)
	{
		throw null;
	}

	public static bool IsDaylightSavingTime(DateTime time, DaylightTime daylightTimes)
	{
		throw null;
	}

	public virtual DateTime ToLocalTime(DateTime time)
	{
		throw null;
	}

	public virtual DateTime ToUniversalTime(DateTime time)
	{
		throw null;
	}
}
