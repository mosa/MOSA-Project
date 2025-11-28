namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public abstract class IsolatedStoragePermissionAttribute : CodeAccessSecurityAttribute
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

	protected IsolatedStoragePermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}
}
