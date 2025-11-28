using System;

namespace Microsoft.Win32;

public class SessionEndingEventArgs : EventArgs
{
	public bool Cancel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SessionEndReasons Reason
	{
		get
		{
			throw null;
		}
	}

	public SessionEndingEventArgs(SessionEndReasons reason)
	{
	}
}
