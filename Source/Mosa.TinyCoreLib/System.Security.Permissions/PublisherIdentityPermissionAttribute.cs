namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class PublisherIdentityPermissionAttribute : CodeAccessSecurityAttribute
{
	public string CertFile
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string SignedFile
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string X509Certificate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PublisherIdentityPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
