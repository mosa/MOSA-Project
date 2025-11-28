namespace System.Diagnostics;

public enum TraceEventType
{
	Critical = 1,
	Error = 2,
	Warning = 4,
	Information = 8,
	Verbose = 0x10,
	Start = 0x100,
	Stop = 0x200,
	Suspend = 0x400,
	Resume = 0x800,
	Transfer = 0x1000
}
