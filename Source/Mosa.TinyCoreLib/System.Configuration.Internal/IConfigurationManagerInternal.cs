namespace System.Configuration.Internal;

public interface IConfigurationManagerInternal
{
	string ApplicationConfigUri { get; }

	string ExeLocalConfigDirectory { get; }

	string ExeLocalConfigPath { get; }

	string ExeProductName { get; }

	string ExeProductVersion { get; }

	string ExeRoamingConfigDirectory { get; }

	string ExeRoamingConfigPath { get; }

	string MachineConfigPath { get; }

	bool SetConfigurationSystemInProgress { get; }

	bool SupportsUserConfig { get; }

	string UserConfigFilename { get; }
}
