using System.Collections.Specialized;

namespace System.Configuration;

public static class ConfigurationManager
{
	public static NameValueCollection AppSettings
	{
		get
		{
			throw null;
		}
	}

	public static ConnectionStringSettingsCollection ConnectionStrings
	{
		get
		{
			throw null;
		}
	}

	public static object GetSection(string sectionName)
	{
		throw null;
	}

	public static Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel)
	{
		throw null;
	}

	public static Configuration OpenExeConfiguration(string exePath)
	{
		throw null;
	}

	public static Configuration OpenMachineConfiguration()
	{
		throw null;
	}

	public static Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel)
	{
		throw null;
	}

	public static Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel, bool preLoad)
	{
		throw null;
	}

	public static Configuration OpenMappedMachineConfiguration(ConfigurationFileMap fileMap)
	{
		throw null;
	}

	public static void RefreshSection(string sectionName)
	{
	}
}
