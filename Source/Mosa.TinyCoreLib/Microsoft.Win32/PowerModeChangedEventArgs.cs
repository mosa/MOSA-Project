using System;

namespace Microsoft.Win32;

public class PowerModeChangedEventArgs : EventArgs
{
	public PowerModes Mode
	{
		get
		{
			throw null;
		}
	}

	public PowerModeChangedEventArgs(PowerModes mode)
	{
	}
}
