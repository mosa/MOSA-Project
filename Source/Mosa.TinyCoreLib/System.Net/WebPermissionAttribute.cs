using System.Security;
using System.Security.Permissions;

namespace System.Net;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class WebPermissionAttribute : CodeAccessSecurityAttribute
{
	public string Accept
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string AcceptPattern
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Connect
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ConnectPattern
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public WebPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
