using System;
using System.Runtime.Versioning;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic;

[StandardModule]
public sealed class DateAndTime
{
	public static string DateString
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public static DateTime Now
	{
		get
		{
			throw null;
		}
	}

	public static DateTime TimeOfDay
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public static double Timer
	{
		get
		{
			throw null;
		}
	}

	public static string TimeString
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public static DateTime Today
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	internal DateAndTime()
	{
	}

	public static DateTime DateAdd(DateInterval Interval, double Number, DateTime DateValue)
	{
		throw null;
	}

	public static DateTime DateAdd(string Interval, double Number, object? DateValue)
	{
		throw null;
	}

	public static long DateDiff(DateInterval Interval, DateTime Date1, DateTime Date2, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday, FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1)
	{
		throw null;
	}

	public static long DateDiff(string Interval, object? Date1, object? Date2, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday, FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1)
	{
		throw null;
	}

	public static int DatePart(DateInterval Interval, DateTime DateValue, FirstDayOfWeek FirstDayOfWeekValue = FirstDayOfWeek.Sunday, FirstWeekOfYear FirstWeekOfYearValue = FirstWeekOfYear.Jan1)
	{
		throw null;
	}

	public static int DatePart(string Interval, object? DateValue, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday, FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1)
	{
		throw null;
	}

	public static DateTime DateSerial(int Year, int Month, int Day)
	{
		throw null;
	}

	public static DateTime DateValue(string StringDate)
	{
		throw null;
	}

	public static int Day(DateTime DateValue)
	{
		throw null;
	}

	public static int Hour(DateTime TimeValue)
	{
		throw null;
	}

	public static int Minute(DateTime TimeValue)
	{
		throw null;
	}

	public static int Month(DateTime DateValue)
	{
		throw null;
	}

	public static string MonthName(int Month, bool Abbreviate = false)
	{
		throw null;
	}

	public static int Second(DateTime TimeValue)
	{
		throw null;
	}

	public static DateTime TimeSerial(int Hour, int Minute, int Second)
	{
		throw null;
	}

	public static DateTime TimeValue(string StringTime)
	{
		throw null;
	}

	public static int Weekday(DateTime DateValue, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday)
	{
		throw null;
	}

	public static string WeekdayName(int Weekday, bool Abbreviate = false, FirstDayOfWeek FirstDayOfWeekValue = FirstDayOfWeek.System)
	{
		throw null;
	}

	public static int Year(DateTime DateValue)
	{
		throw null;
	}
}
