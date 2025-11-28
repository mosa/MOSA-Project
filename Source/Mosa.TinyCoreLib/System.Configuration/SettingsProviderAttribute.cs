namespace System.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class SettingsProviderAttribute : Attribute
{
	public string ProviderTypeName
	{
		get
		{
			throw null;
		}
	}

	public SettingsProviderAttribute(string providerTypeName)
	{
	}

	public SettingsProviderAttribute(Type providerType)
	{
	}
}
