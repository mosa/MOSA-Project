using System.ComponentModel;

namespace System.Configuration;

public class SettingChangingEventArgs : CancelEventArgs
{
	public object NewValue
	{
		get
		{
			throw null;
		}
	}

	public string SettingClass
	{
		get
		{
			throw null;
		}
	}

	public string SettingKey
	{
		get
		{
			throw null;
		}
	}

	public string SettingName
	{
		get
		{
			throw null;
		}
	}

	public SettingChangingEventArgs(string settingName, string settingClass, string settingKey, object newValue, bool cancel)
	{
	}
}
