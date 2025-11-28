namespace System.ServiceProcess;

public enum SessionChangeReason
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
