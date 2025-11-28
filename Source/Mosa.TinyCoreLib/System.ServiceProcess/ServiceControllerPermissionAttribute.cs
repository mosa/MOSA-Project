using System.Security;
using System.Security.Permissions;

namespace System.ServiceProcess;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
public class ServiceControllerPermissionAttribute : CodeAccessSecurityAttribute
{
	public string MachineName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ServiceControllerPermissionAccess PermissionAccess
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ServiceName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ServiceControllerPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
