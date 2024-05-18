namespace System.Runtime.InteropServices;

[Flags]
public enum CreateObjectFlags
{
	None = 0,
	TrackerObject = 1,
	UniqueInstance = 2,
	Aggregation = 4,
	Unwrap = 8
}
