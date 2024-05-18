namespace System.Globalization;

public sealed class DateTimeFormatInfo : ICloneable, IFormatProvider
{
	public string[] AbbreviatedDayNames
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string[] AbbreviatedMonthGenitiveNames
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string[] AbbreviatedMonthNames
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string AMDesignator
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Calendar Calendar
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CalendarWeekRule CalendarWeekRule
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static DateTimeFormatInfo CurrentInfo
	{
		get
		{
			throw null;
		}
	}

	public string DateSeparator
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string[] DayNames
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DayOfWeek FirstDayOfWeek
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string FullDateTimePattern
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static DateTimeFormatInfo InvariantInfo
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public string LongDatePattern
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string LongTimePattern
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string MonthDayPattern
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string[] MonthGenitiveNames
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string[] MonthNames
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string NativeCalendarName
	{
		get
		{
			throw null;
		}
	}

	public string PMDesignator
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string RFC1123Pattern
	{
		get
		{
			throw null;
		}
	}

	public string ShortDatePattern
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string[] ShortestDayNames
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ShortTimePattern
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string SortableDateTimePattern
	{
		get
		{
			throw null;
		}
	}

	public string TimeSeparator
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string UniversalSortableDateTimePattern
	{
		get
		{
			throw null;
		}
	}

	public string YearMonthPattern
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object Clone()
	{
		throw null;
	}

	public string GetAbbreviatedDayName(DayOfWeek dayofweek)
	{
		throw null;
	}

	public string GetAbbreviatedEraName(int era)
	{
		throw null;
	}

	public string GetAbbreviatedMonthName(int month)
	{
		throw null;
	}

	public string[] GetAllDateTimePatterns()
	{
		throw null;
	}

	public string[] GetAllDateTimePatterns(char format)
	{
		throw null;
	}

	public string GetDayName(DayOfWeek dayofweek)
	{
		throw null;
	}

	public int GetEra(string eraName)
	{
		throw null;
	}

	public string GetEraName(int era)
	{
		throw null;
	}

	public object? GetFormat(Type? formatType)
	{
		throw null;
	}

	public static DateTimeFormatInfo GetInstance(IFormatProvider? provider)
	{
		throw null;
	}

	public string GetMonthName(int month)
	{
		throw null;
	}

	public string GetShortestDayName(DayOfWeek dayOfWeek)
	{
		throw null;
	}

	public static DateTimeFormatInfo ReadOnly(DateTimeFormatInfo dtfi)
	{
		throw null;
	}

	public void SetAllDateTimePatterns(string[] patterns, char format)
	{
	}
}
