using System.ComponentModel;

namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.All)]
public class MonitoringDescriptionAttribute : DescriptionAttribute
{
	public override string Description
	{
		get
		{
			throw null;
		}
	}

	public MonitoringDescriptionAttribute(string description)
	{
	}
}
