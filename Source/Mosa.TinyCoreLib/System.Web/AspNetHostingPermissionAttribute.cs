using System.Security;
using System.Security.Permissions;

namespace System.Web;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public sealed class AspNetHostingPermissionAttribute : CodeAccessSecurityAttribute
{
	public AspNetHostingPermissionLevel Level
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AspNetHostingPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
