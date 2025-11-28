namespace System.Configuration;

public interface IPersistComponentSettings
{
	bool SaveSettings { get; set; }

	string SettingsKey { get; set; }

	void LoadComponentSettings();

	void ResetComponentSettings();

	void SaveComponentSettings();
}
