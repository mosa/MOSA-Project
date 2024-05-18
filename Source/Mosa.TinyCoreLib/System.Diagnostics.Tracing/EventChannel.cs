namespace System.Diagnostics.Tracing;

public enum EventChannel : byte
{
	None = 0,
	Admin = 16,
	Operational = 17,
	Analytic = 18,
	Debug = 19
}
