namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public class ResourcePermissionBaseEntry
{
	public int PermissionAccess
	{
		get
		{
			throw null;
		}
	}

	public string[] PermissionAccessPath
	{
		get
		{
			throw null;
		}
	}

	public ResourcePermissionBaseEntry()
	{
	}

	public ResourcePermissionBaseEntry(int permissionAccess, string[] permissionAccessPath)
	{
	}
}
