namespace Microsoft.Win32;

public enum SessionSwitchReason
{
	ConsoleConnect = 1,
	ConsoleDisconnect,
	RemoteConnect,
	RemoteDisconnect,
	SessionLogon,
	SessionLogoff,
	SessionLock,
	SessionUnlock,
	SessionRemoteControl
}
