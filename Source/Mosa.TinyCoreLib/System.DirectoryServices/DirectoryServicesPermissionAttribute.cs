using System.Security;
using System.Security.Permissions;

namespace System.DirectoryServices;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
public class DirectoryServicesPermissionAttribute : CodeAccessSecurityAttribute
{
	public string Path
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectoryServicesPermissionAccess PermissionAccess
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectoryServicesPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
