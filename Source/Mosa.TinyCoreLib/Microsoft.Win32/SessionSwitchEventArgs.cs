using System;

namespace Microsoft.Win32;

public class SessionSwitchEventArgs : EventArgs
{
	public SessionSwitchReason Reason
	{
		get
		{
			throw null;
		}
	}

	public SessionSwitchEventArgs(SessionSwitchReason reason)
	{
	}
}
