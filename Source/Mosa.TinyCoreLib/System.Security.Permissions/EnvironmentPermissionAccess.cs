namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[Flags]
public enum EnvironmentPermissionAccess
{
	NoAccess = 0,
	Read = 1,
	Write = 2,
	AllAccess = 3
}
