namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[Flags]
public enum FileDialogPermissionAccess
{
	None = 0,
	Open = 1,
	Save = 2,
	OpenSave = 3
}
