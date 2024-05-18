namespace System.ServiceProcess;

public enum PowerBroadcastStatus
{
	QuerySuspend = 0,
	QuerySuspendFailed = 2,
	Suspend = 4,
	ResumeCritical = 6,
	ResumeSuspend = 7,
	BatteryLow = 9,
	PowerStatusChange = 10,
	OemEvent = 11,
	ResumeAutomatic = 18
}
