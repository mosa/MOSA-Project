namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class WebBrowserPermission : CodeAccessPermission, IUnrestrictedPermission
{
	public WebBrowserPermissionLevel Level
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public WebBrowserPermission()
	{
	}

	public WebBrowserPermission(PermissionState state)
	{
	}

	public WebBrowserPermission(WebBrowserPermissionLevel webBrowserPermissionLevel)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	public override void FromXml(SecurityElement securityElement)
	{
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

	public override SecurityElement ToXml()
	{
		throw null;
	}

	public override IPermission Union(IPermission target)
	{
		throw null;
	}
}
