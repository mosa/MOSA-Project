using System.Security;
using System.Security.Permissions;

namespace System.Transactions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class DistributedTransactionPermissionAttribute : CodeAccessSecurityAttribute
{
	public new bool Unrestricted
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DistributedTransactionPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	public override IPermission CreatePermission()
	{
		throw null;
	}
}
