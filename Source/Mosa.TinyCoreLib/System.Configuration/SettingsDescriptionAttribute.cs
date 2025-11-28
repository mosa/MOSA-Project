namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SettingsDescriptionAttribute : Attribute
{
	public string Description
	{
		get
		{
			throw null;
		}
	}

	public SettingsDescriptionAttribute(string description)
	{
	}
}
