namespace System.Runtime.InteropServices;

[Flags]
public enum CreateComInterfaceFlags
{
	None = 0,
	CallerDefinedIUnknown = 1,
	TrackerSupport = 2
}
