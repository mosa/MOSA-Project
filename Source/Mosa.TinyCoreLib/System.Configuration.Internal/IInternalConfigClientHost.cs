namespace System.Configuration.Internal;

public interface IInternalConfigClientHost
{
	string GetExeConfigPath();

	string GetLocalUserConfigPath();

	string GetRoamingUserConfigPath();

	bool IsExeConfig(string configPath);

	bool IsLocalUserConfig(string configPath);

	bool IsRoamingUserConfig(string configPath);
}
