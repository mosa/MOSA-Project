namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class DefaultSettingValueAttribute : Attribute
{
	public string Value
	{
		get
		{
			throw null;
		}
	}

	public DefaultSettingValueAttribute(string value)
	{
	}
}
