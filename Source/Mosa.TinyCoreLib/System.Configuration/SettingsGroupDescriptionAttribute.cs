namespace System.Configuration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SettingsGroupDescriptionAttribute : Attribute
{
	public string Description
	{
		get
		{
			throw null;
		}
	}

	public SettingsGroupDescriptionAttribute(string description)
	{
	}
}
