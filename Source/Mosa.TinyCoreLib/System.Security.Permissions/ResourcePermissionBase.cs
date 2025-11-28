namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public abstract class ResourcePermissionBase : CodeAccessPermission, IUnrestrictedPermission
{
	public const string Any = "*";

	public const string Local = ".";

	protected Type PermissionAccessType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected string[] TagNames
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected ResourcePermissionBase()
	{
	}

	protected ResourcePermissionBase(PermissionState state)
	{
	}

	protected void AddPermissionAccess(ResourcePermissionBaseEntry entry)
	{
	}

	protected void Clear()
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	public override void FromXml(SecurityElement securityElement)
	{
	}

	protected ResourcePermissionBaseEntry[] GetPermissionEntries()
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

	protected void RemovePermissionAccess(ResourcePermissionBaseEntry entry)
	{
	}

	public override SecurityElement ToXml()
	{
		throw null;
	}

	public override IPermission Union(IPermission target)
	{
		throw null;
	}
}
