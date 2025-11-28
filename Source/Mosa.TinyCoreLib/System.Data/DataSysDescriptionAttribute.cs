using System.ComponentModel;

namespace System.Data;

[AttributeUsage(AttributeTargets.All)]
[Obsolete("DataSysDescriptionAttribute has been deprecated and is not supported.")]
public class DataSysDescriptionAttribute : DescriptionAttribute
{
	public override string Description
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("DataSysDescriptionAttribute has been deprecated and is not supported.")]
	public DataSysDescriptionAttribute(string description)
	{
	}
}
