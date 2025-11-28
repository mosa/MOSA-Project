namespace System.Diagnostics;

[Flags]
public enum PerformanceCounterPermissionAccess
{
	None = 0,
	Browse = 1,
	Read = 1,
	Write = 2,
	Instrument = 3,
	Administer = 7
}
