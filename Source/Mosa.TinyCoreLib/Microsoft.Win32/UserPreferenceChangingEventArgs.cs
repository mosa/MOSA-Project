using System;

namespace Microsoft.Win32;

public class UserPreferenceChangingEventArgs : EventArgs
{
	public UserPreferenceCategory Category
	{
		get
		{
			throw null;
		}
	}

	public UserPreferenceChangingEventArgs(UserPreferenceCategory category)
	{
	}
}
