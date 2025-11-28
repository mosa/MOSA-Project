namespace System.Configuration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SettingsGroupNameAttribute : Attribute
{
	public string GroupName
	{
		get
		{
			throw null;
		}
	}

	public SettingsGroupNameAttribute(string groupName)
	{
	}
}
