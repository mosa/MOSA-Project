namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class PrincipalPermissionAttribute : CodeAccessSecurityAttribute
{
	public bool Authenticated
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Role
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("PrincipalPermissionAttribute is not honored by the runtime and must not be used.", true, DiagnosticId = "SYSLIB0002", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public PrincipalPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
