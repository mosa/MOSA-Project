namespace System.Diagnostics;

public enum ThreadPriorityLevel
{
	Idle = -15,
	Lowest = -2,
	BelowNormal = -1,
	Normal = 0,
	AboveNormal = 1,
	Highest = 2,
	TimeCritical = 15
}
