using System.Security.Permissions;

namespace System.ServiceProcess;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class ServiceControllerPermission : ResourcePermissionBase
{
	public ServiceControllerPermissionEntryCollection PermissionEntries
	{
		get
		{
			throw null;
		}
	}

	public ServiceControllerPermission()
	{
	}

	public ServiceControllerPermission(PermissionState state)
	{
	}

	public ServiceControllerPermission(ServiceControllerPermissionAccess permissionAccess, string machineName, string serviceName)
	{
	}

	public ServiceControllerPermission(ServiceControllerPermissionEntry[] permissionAccessEntries)
	{
	}
}
