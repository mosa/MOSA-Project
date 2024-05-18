namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class PermissionSetAttribute : CodeAccessSecurityAttribute
{
	public string File
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Hex
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

	public bool UnicodeEncoded
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string XML
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PermissionSetAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}

	public PermissionSet CreatePermissionSet()
	{
		throw null;
	}
}
