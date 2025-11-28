namespace System.Configuration;

public class ProtectedProviderSettings : ConfigurationElement
{
	protected override ConfigurationPropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	public ProviderSettingsCollection Providers
	{
		get
		{
			throw null;
		}
	}
}
