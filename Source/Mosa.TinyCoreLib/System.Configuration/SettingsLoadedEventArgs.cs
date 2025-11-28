namespace System.Configuration;

public class SettingsLoadedEventArgs : EventArgs
{
	public SettingsProvider Provider
	{
		get
		{
			throw null;
		}
	}

	public SettingsLoadedEventArgs(SettingsProvider provider)
	{
	}
}
