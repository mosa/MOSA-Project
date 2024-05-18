using System.Security.Permissions;

namespace System.DirectoryServices;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class DirectoryServicesPermission : ResourcePermissionBase
{
	public DirectoryServicesPermissionEntryCollection PermissionEntries
	{
		get
		{
			throw null;
		}
	}

	public DirectoryServicesPermission()
	{
	}

	public DirectoryServicesPermission(DirectoryServicesPermissionAccess permissionAccess, string path)
	{
	}

	public DirectoryServicesPermission(DirectoryServicesPermissionEntry[] permissionAccessEntries)
	{
	}

	public DirectoryServicesPermission(PermissionState state)
	{
	}
}
