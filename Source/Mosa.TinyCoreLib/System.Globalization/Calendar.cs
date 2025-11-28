namespace System.Globalization;

public abstract class Calendar : ICloneable
{
	public const int CurrentEra = 0;

	public virtual CalendarAlgorithmType AlgorithmType
	{
		get
		{
			throw null;
		}
	}

	protected virtual int DaysInYearBeforeMinSupportedYear
	{
		get
		{
			throw null;
		}
	}

	public abstract int[] Eras { get; }

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public virtual DateTime MaxSupportedDateTime
	{
		get
		{
			throw null;
		}
	}

	public virtual DateTime MinSupportedDateTime
	{
		get
		{
			throw null;
		}
	}

	public virtual int TwoDigitYearMax
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual DateTime AddDays(DateTime time, int days)
	{
		throw null;
	}

	public virtual DateTime AddHours(DateTime time, int hours)
	{
		throw null;
	}

	public virtual DateTime AddMilliseconds(DateTime time, double milliseconds)
	{
		throw null;
	}

	public virtual DateTime AddMinutes(DateTime time, int minutes)
	{
		throw null;
	}

	public abstract DateTime AddMonths(DateTime time, int months);

	public virtual DateTime AddSeconds(DateTime time, int seconds)
	{
		throw null;
	}

	public virtual DateTime AddWeeks(DateTime time, int weeks)
	{
		throw null;
	}

	public abstract DateTime AddYears(DateTime time, int years);

	public virtual object Clone()
	{
		throw null;
	}

	public abstract int GetDayOfMonth(DateTime time);

	public abstract DayOfWeek GetDayOfWeek(DateTime time);

	public abstract int GetDayOfYear(DateTime time);

	public virtual int GetDaysInMonth(int year, int month)
	{
		throw null;
	}

	public abstract int GetDaysInMonth(int year, int month, int era);

	public virtual int GetDaysInYear(int year)
	{
		throw null;
	}

	public abstract int GetDaysInYear(int year, int era);

	public abstract int GetEra(DateTime time);

	public virtual int GetHour(DateTime time)
	{
		throw null;
	}

	public virtual int GetLeapMonth(int year)
	{
		throw null;
	}

	public virtual int GetLeapMonth(int year, int era)
	{
		throw null;
	}

	public virtual double GetMilliseconds(DateTime time)
	{
		throw null;
	}

	public virtual int GetMinute(DateTime time)
	{
		throw null;
	}

	public abstract int GetMonth(DateTime time);

	public virtual int GetMonthsInYear(int year)
	{
		throw null;
	}

	public abstract int GetMonthsInYear(int year, int era);

	public virtual int GetSecond(DateTime time)
	{
		throw null;
	}

	public virtual int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
	{
		throw null;
	}

	public abstract int GetYear(DateTime time);

	public virtual bool IsLeapDay(int year, int month, int day)
	{
		throw null;
	}

	public abstract bool IsLeapDay(int year, int month, int day, int era);

	public virtual bool IsLeapMonth(int year, int month)
	{
		throw null;
	}

	public abstract bool IsLeapMonth(int year, int month, int era);

	public virtual bool IsLeapYear(int year)
	{
		throw null;
	}

	public abstract bool IsLeapYear(int year, int era);

	public static Calendar ReadOnly(Calendar calendar)
	{
		throw null;
	}

	public virtual DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
	{
		throw null;
	}

	public abstract DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era);

	public virtual int ToFourDigitYear(int year)
	{
		throw null;
	}
}
