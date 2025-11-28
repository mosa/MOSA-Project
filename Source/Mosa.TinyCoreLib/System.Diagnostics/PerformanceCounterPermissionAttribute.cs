using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
public class PerformanceCounterPermissionAttribute : CodeAccessSecurityAttribute
{
	public string CategoryName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public PerformanceCounterPermissionAccess PermissionAccess
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PerformanceCounterPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
