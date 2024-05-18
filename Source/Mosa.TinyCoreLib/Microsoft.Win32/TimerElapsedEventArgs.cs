using System;

namespace Microsoft.Win32;

public class TimerElapsedEventArgs : EventArgs
{
	public IntPtr TimerId
	{
		get
		{
			throw null;
		}
	}

	public TimerElapsedEventArgs(IntPtr timerId)
	{
	}
}
