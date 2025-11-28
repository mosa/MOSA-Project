using System;
using System.ComponentModel;

namespace Microsoft.Win32;

public sealed class SystemEvents
{
	public static event EventHandler? DisplaySettingsChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event EventHandler? DisplaySettingsChanging
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event EventHandler? EventsThreadShutdown
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event EventHandler? InstalledFontsChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("The LowMemory event has been deprecated and is not supported.")]
	public static event EventHandler? LowMemory
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event EventHandler? PaletteChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event PowerModeChangedEventHandler? PowerModeChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event SessionEndedEventHandler? SessionEnded
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event SessionEndingEventHandler? SessionEnding
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event SessionSwitchEventHandler? SessionSwitch
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event EventHandler? TimeChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event TimerElapsedEventHandler? TimerElapsed
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event UserPreferenceChangedEventHandler? UserPreferenceChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event UserPreferenceChangingEventHandler? UserPreferenceChanging
	{
		add
		{
		}
		remove
		{
		}
	}

	internal SystemEvents()
	{
	}

	public static IntPtr CreateTimer(int interval)
	{
		throw null;
	}

	public static void InvokeOnEventsThread(Delegate method)
	{
	}

	public static void KillTimer(IntPtr timerId)
	{
	}
}
