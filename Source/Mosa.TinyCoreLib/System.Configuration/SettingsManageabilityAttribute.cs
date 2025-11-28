namespace System.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class SettingsManageabilityAttribute : Attribute
{
	public SettingsManageability Manageability
	{
		get
		{
			throw null;
		}
	}

	public SettingsManageabilityAttribute(SettingsManageability manageability)
	{
	}
}
