namespace System.Configuration;

public interface ISettingsProviderService
{
	SettingsProvider GetSettingsProvider(SettingsProperty property);
}
