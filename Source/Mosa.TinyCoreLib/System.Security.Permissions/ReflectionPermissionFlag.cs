namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[Flags]
public enum ReflectionPermissionFlag
{
	NoFlags = 0,
	[Obsolete("ReflectionPermissionFlag.TypeInformation has been deprecated and is not supported.")]
	TypeInformation = 1,
	MemberAccess = 2,
	[Obsolete("ReflectionPermissionFlag.ReflectionEmit  has been deprecated and is not supported.")]
	ReflectionEmit = 4,
	[Obsolete("ReflectionPermissionFlag.AllFlags has been deprecated. Use PermissionState.Unrestricted to get full access.")]
	AllFlags = 7,
	RestrictedMemberAccess = 8
}
