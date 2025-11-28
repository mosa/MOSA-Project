using System.ComponentModel;

namespace System.Timers;

[AttributeUsage(AttributeTargets.All)]
public class TimersDescriptionAttribute : DescriptionAttribute
{
	public override string Description
	{
		get
		{
			throw null;
		}
	}

	public TimersDescriptionAttribute(string description)
	{
	}
}
