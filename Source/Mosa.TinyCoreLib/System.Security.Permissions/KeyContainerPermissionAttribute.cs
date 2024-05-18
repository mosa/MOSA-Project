namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class KeyContainerPermissionAttribute : CodeAccessSecurityAttribute
{
	public KeyContainerPermissionFlags Flags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string KeyContainerName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int KeySpec
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string KeyStore
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ProviderName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ProviderType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public KeyContainerPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
