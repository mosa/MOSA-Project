namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[Flags]
public enum DataProtectionPermissionFlags
{
	NoFlags = 0,
	ProtectData = 1,
	UnprotectData = 2,
	ProtectMemory = 4,
	UnprotectMemory = 8,
	AllFlags = 0xF
}
