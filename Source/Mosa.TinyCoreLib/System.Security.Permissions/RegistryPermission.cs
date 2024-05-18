using System.Security.AccessControl;

namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class RegistryPermission : CodeAccessPermission, IUnrestrictedPermission
{
	public RegistryPermission(PermissionState state)
	{
	}

	public RegistryPermission(RegistryPermissionAccess access, AccessControlActions control, string pathList)
	{
	}

	public RegistryPermission(RegistryPermissionAccess access, string pathList)
	{
	}

	public void AddPathList(RegistryPermissionAccess access, AccessControlActions actions, string pathList)
	{
	}

	public void AddPathList(RegistryPermissionAccess access, string pathList)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	public override void FromXml(SecurityElement elem)
	{
	}

	public string GetPathList(RegistryPermissionAccess access)
	{
		throw null;
	}

	public override IPermission Intersect(IPermission target)
	{
		throw null;
	}

	public override bool IsSubsetOf(IPermission target)
	{
		throw null;
	}

	public bool IsUnrestricted()
	{
		throw null;
	}

	public void SetPathList(RegistryPermissionAccess access, string pathList)
	{
	}

	public override SecurityElement ToXml()
	{
		throw null;
	}

	public override IPermission Union(IPermission other)
	{
		throw null;
	}
}
