using System.Security;
using System.Security.Permissions;

namespace System.Net.Mail;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class SmtpPermissionAttribute : CodeAccessSecurityAttribute
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

	public SmtpPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
