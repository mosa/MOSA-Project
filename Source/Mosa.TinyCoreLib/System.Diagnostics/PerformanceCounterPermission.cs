using System.Security.Permissions;

namespace System.Diagnostics;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class PerformanceCounterPermission : ResourcePermissionBase
{
	public PerformanceCounterPermissionEntryCollection PermissionEntries
	{
		get
		{
			throw null;
		}
	}

	public PerformanceCounterPermission()
	{
	}

	public PerformanceCounterPermission(PerformanceCounterPermissionAccess permissionAccess, string machineName, string categoryName)
	{
	}

	public PerformanceCounterPermission(PerformanceCounterPermissionEntry[] permissionAccessEntries)
	{
	}

	public PerformanceCounterPermission(PermissionState state)
	{
	}
}
