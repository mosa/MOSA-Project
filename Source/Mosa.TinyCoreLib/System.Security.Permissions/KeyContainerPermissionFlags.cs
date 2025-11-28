namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public enum KeyContainerPermissionFlags
{
	NoFlags = 0,
	Create = 1,
	Open = 2,
	Delete = 4,
	Import = 16,
	Export = 32,
	Sign = 256,
	Decrypt = 512,
	ViewAcl = 4096,
	ChangeAcl = 8192,
	AllFlags = 13111
}
