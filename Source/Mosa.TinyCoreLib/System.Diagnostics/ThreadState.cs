namespace System.Diagnostics;

public enum ThreadState
{
	Initialized,
	Ready,
	Running,
	Standby,
	Terminated,
	Wait,
	Transition,
	Unknown
}
