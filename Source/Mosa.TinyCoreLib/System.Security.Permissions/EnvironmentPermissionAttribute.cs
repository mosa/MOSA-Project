namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class EnvironmentPermissionAttribute : CodeAccessSecurityAttribute
{
	public string All
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Read
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Write
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EnvironmentPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
