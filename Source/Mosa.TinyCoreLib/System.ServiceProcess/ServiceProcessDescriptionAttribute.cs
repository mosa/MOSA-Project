using System.ComponentModel;

namespace System.ServiceProcess;

[AttributeUsage(AttributeTargets.All)]
public class ServiceProcessDescriptionAttribute : DescriptionAttribute
{
	public override string Description
	{
		get
		{
			throw null;
		}
	}

	public ServiceProcessDescriptionAttribute(string description)
	{
	}
}
