using System.Security;
using System.Security.Permissions;

namespace System.Net.Mail;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class SmtpPermission : CodeAccessPermission, IUnrestrictedPermission
{
	public SmtpAccess Access
	{
		get
		{
			throw null;
		}
	}

	public SmtpPermission(bool unrestricted)
	{
	}

	public SmtpPermission(SmtpAccess access)
	{
	}

	public SmtpPermission(PermissionState state)
	{
	}

	public void AddPermission(SmtpAccess access)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	public override void FromXml(SecurityElement securityElement)
	{
	}

	public override IPermission Intersect(IPermission target)
	{
		throw null;
	}

	public override bool IsSubsetOf(IPermission target)
	{
		throw null;
	}

	public bool IsUnrestricted()
	{
		throw null;
	}

	public override SecurityElement ToXml()
	{
		throw null;
	}

	public override IPermission Union(IPermission target)
	{
		throw null;
	}
}
