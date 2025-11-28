namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public abstract class IsolatedStoragePermission : CodeAccessPermission, IUnrestrictedPermission
{
	public IsolatedStorageContainment UsageAllowed
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long UserQuota
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected IsolatedStoragePermission(PermissionState state)
	{
	}

	public override void FromXml(SecurityElement esd)
	{
	}

	public bool IsUnrestricted()
	{
		throw null;
	}

	public override SecurityElement ToXml()
	{
		throw null;
	}
}
