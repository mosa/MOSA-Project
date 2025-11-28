namespace System.Runtime.Versioning;

[Flags]
public enum ResourceScope
{
	None = 0,
	Machine = 1,
	Process = 2,
	AppDomain = 4,
	Library = 8,
	Private = 0x10,
	Assembly = 0x20
}
