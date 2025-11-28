namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[Flags]
public enum HostProtectionResource
{
	None = 0,
	Synchronization = 1,
	SharedState = 2,
	ExternalProcessMgmt = 4,
	SelfAffectingProcessMgmt = 8,
	ExternalThreading = 0x10,
	SelfAffectingThreading = 0x20,
	SecurityInfrastructure = 0x40,
	UI = 0x80,
	MayLeakOnAbort = 0x100,
	All = 0x1FF
}
