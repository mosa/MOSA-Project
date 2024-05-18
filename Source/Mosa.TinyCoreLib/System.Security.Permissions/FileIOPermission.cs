using System.Security.AccessControl;

namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class FileIOPermission : CodeAccessPermission, IUnrestrictedPermission
{
	public FileIOPermissionAccess AllFiles
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public FileIOPermissionAccess AllLocalFiles
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public FileIOPermission(FileIOPermissionAccess access, AccessControlActions actions, string path)
	{
	}

	public FileIOPermission(FileIOPermissionAccess access, AccessControlActions actions, string[] pathList)
	{
	}

	public FileIOPermission(FileIOPermissionAccess access, string path)
	{
	}

	public FileIOPermission(FileIOPermissionAccess access, string[] pathList)
	{
	}

	public FileIOPermission(PermissionState state)
	{
	}

	public void AddPathList(FileIOPermissionAccess access, string path)
	{
	}

	public void AddPathList(FileIOPermissionAccess access, string[] pathList)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public override void FromXml(SecurityElement esd)
	{
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public string[] GetPathList(FileIOPermissionAccess access)
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

	public void SetPathList(FileIOPermissionAccess access, string path)
	{
	}

	public void SetPathList(FileIOPermissionAccess access, string[] pathList)
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
