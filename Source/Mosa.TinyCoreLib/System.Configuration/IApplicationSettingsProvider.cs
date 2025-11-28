namespace System.Configuration;

public interface IApplicationSettingsProvider
{
	SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property);

	void Reset(SettingsContext context);

	void Upgrade(SettingsContext context, SettingsPropertyCollection properties);
}
