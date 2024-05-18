namespace System.ServiceProcess;

public enum ServiceControllerStatus
{
	Stopped = 1,
	StartPending,
	StopPending,
	Running,
	ContinuePending,
	PausePending,
	Paused
}
