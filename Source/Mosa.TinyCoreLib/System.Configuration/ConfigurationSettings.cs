using System.Collections.Specialized;

namespace System.Configuration;

public sealed class ConfigurationSettings
{
	[Obsolete("ConfigurationSettings.AppSettings has been deprecated. Use System.Configuration.ConfigurationManager.AppSettings instead.")]
	public static NameValueCollection AppSettings
	{
		get
		{
			throw null;
		}
	}

	internal ConfigurationSettings()
	{
	}

	[Obsolete("ConfigurationSettings.GetConfig has been deprecated. Use System.Configuration.ConfigurationManager.GetSection instead.")]
	public static object GetConfig(string sectionName)
	{
		throw null;
	}
}
