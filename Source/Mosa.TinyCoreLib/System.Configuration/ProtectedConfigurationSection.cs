namespace System.Configuration;

public sealed class ProtectedConfigurationSection : ConfigurationSection
{
	public string DefaultProvider
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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
