namespace System.Globalization;

public abstract class EastAsianLunisolarCalendar : Calendar
{
	public override CalendarAlgorithmType AlgorithmType
	{
		get
		{
			throw null;
		}
	}

	public override int TwoDigitYearMax
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal EastAsianLunisolarCalendar()
	{
	}

	public override DateTime AddMonths(DateTime time, int months)
	{
		throw null;
	}

	public override DateTime AddYears(DateTime time, int years)
	{
		throw null;
	}

	public int GetCelestialStem(int sexagenaryYear)
	{
		throw null;
	}

	public override int GetDayOfMonth(DateTime time)
	{
		throw null;
	}

	public override DayOfWeek GetDayOfWeek(DateTime time)
	{
		throw null;
	}

	public override int GetDayOfYear(DateTime time)
	{
		throw null;
	}

	public override int GetDaysInMonth(int year, int month, int era)
	{
		throw null;
	}

	public override int GetDaysInYear(int year, int era)
	{
		throw null;
	}

	public override int GetLeapMonth(int year, int era)
	{
		throw null;
	}

	public override int GetMonth(DateTime time)
	{
		throw null;
	}

	public override int GetMonthsInYear(int year, int era)
	{
		throw null;
	}

	public virtual int GetSexagenaryYear(DateTime time)
	{
		throw null;
	}

	public int GetTerrestrialBranch(int sexagenaryYear)
	{
		throw null;
	}

	public override int GetYear(DateTime time)
	{
		throw null;
	}

	public override bool IsLeapDay(int year, int month, int day, int era)
	{
		throw null;
	}

	public override bool IsLeapMonth(int year, int month, int era)
	{
		throw null;
	}

	public override bool IsLeapYear(int year, int era)
	{
		throw null;
	}

	public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
	{
		throw null;
	}

	public override int ToFourDigitYear(int year)
	{
		throw null;
	}
}
