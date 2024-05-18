using System.Security.Permissions;

namespace System.Diagnostics;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class EventLogPermission : ResourcePermissionBase
{
	public EventLogPermissionEntryCollection PermissionEntries
	{
		get
		{
			throw null;
		}
	}

	public EventLogPermission()
	{
	}

	public EventLogPermission(EventLogPermissionAccess permissionAccess, string machineName)
	{
	}

	public EventLogPermission(EventLogPermissionEntry[] permissionAccessEntries)
	{
	}

	public EventLogPermission(PermissionState state)
	{
	}
}
