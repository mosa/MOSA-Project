namespace System.Diagnostics.Tracing;

[Flags]
public enum EventActivityOptions
{
	None = 0,
	Disable = 2,
	Recursive = 4,
	Detachable = 8
}
