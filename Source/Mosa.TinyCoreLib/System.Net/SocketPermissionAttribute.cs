using System.Security;
using System.Security.Permissions;

namespace System.Net;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class SocketPermissionAttribute : CodeAccessSecurityAttribute
{
	public string Access
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Host
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Port
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Transport
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SocketPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
