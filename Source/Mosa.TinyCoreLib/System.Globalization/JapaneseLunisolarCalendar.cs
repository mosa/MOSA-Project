namespace System.Globalization;

public class JapaneseLunisolarCalendar : EastAsianLunisolarCalendar
{
	public const int JapaneseEra = 1;

	protected override int DaysInYearBeforeMinSupportedYear
	{
		get
		{
			throw null;
		}
	}

	public override int[] Eras
	{
		get
		{
			throw null;
		}
	}

	public override DateTime MaxSupportedDateTime
	{
		get
		{
			throw null;
		}
	}

	public override DateTime MinSupportedDateTime
	{
		get
		{
			throw null;
		}
	}

	public override int GetEra(DateTime time)
	{
		throw null;
	}
}
