namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySchedule
{
	public bool[,,] RawSchedule
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ActiveDirectorySchedule()
	{
	}

	public ActiveDirectorySchedule(ActiveDirectorySchedule schedule)
	{
	}

	public void ResetSchedule()
	{
	}

	public void SetDailySchedule(HourOfDay fromHour, MinuteOfHour fromMinute, HourOfDay toHour, MinuteOfHour toMinute)
	{
	}

	public void SetSchedule(DayOfWeek day, HourOfDay fromHour, MinuteOfHour fromMinute, HourOfDay toHour, MinuteOfHour toMinute)
	{
	}

	public void SetSchedule(DayOfWeek[] days, HourOfDay fromHour, MinuteOfHour fromMinute, HourOfDay toHour, MinuteOfHour toMinute)
	{
	}
}
