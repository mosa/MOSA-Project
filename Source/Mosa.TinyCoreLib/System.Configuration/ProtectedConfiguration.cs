namespace System.Configuration;

public static class ProtectedConfiguration
{
	public const string DataProtectionProviderName = "DataProtectionConfigurationProvider";

	public const string ProtectedDataSectionName = "configProtectedData";

	public const string RsaProviderName = "RsaProtectedConfigurationProvider";

	public static string DefaultProvider
	{
		get
		{
			throw null;
		}
	}

	public static ProtectedConfigurationProviderCollection Providers
	{
		get
		{
			throw null;
		}
	}
}
