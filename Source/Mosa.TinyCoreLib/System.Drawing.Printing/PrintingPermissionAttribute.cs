using System.Security;
using System.Security.Permissions;

namespace System.Drawing.Printing;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class PrintingPermissionAttribute : CodeAccessSecurityAttribute
{
	public PrintingPermissionLevel Level
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PrintingPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
