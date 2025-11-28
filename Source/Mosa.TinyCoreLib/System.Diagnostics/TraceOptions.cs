namespace System.Diagnostics;

[Flags]
public enum TraceOptions
{
	None = 0,
	LogicalOperationStack = 1,
	DateTime = 2,
	Timestamp = 4,
	ProcessId = 8,
	ThreadId = 0x10,
	Callstack = 0x20
}
