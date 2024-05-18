using System;

namespace Microsoft.Win32;

public class SessionEndedEventArgs : EventArgs
{
	public SessionEndReasons Reason
	{
		get
		{
			throw null;
		}
	}

	public SessionEndedEventArgs(SessionEndReasons reason)
	{
	}
}
