namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[Flags]
public enum RegistryPermissionAccess
{
	NoAccess = 0,
	Read = 1,
	Write = 2,
	Create = 4,
	AllAccess = 7
}
