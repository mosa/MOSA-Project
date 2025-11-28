using System;

namespace Microsoft.Win32;

public class UserPreferenceChangedEventArgs : EventArgs
{
	public UserPreferenceCategory Category
	{
		get
		{
			throw null;
		}
	}

	public UserPreferenceChangedEventArgs(UserPreferenceCategory category)
	{
	}
}
