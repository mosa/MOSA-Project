namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public abstract class SecurityAttribute : Attribute
{
	public SecurityAction Action
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Unrestricted
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected SecurityAttribute(SecurityAction action)
	{
	}

	public abstract IPermission? CreatePermission();
}
