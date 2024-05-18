using System.Security.Cryptography.X509Certificates;

namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class PublisherIdentityPermission : CodeAccessPermission
{
	public X509Certificate Certificate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PublisherIdentityPermission(X509Certificate certificate)
	{
	}

	public PublisherIdentityPermission(PermissionState state)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	public override void FromXml(SecurityElement esd)
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

	public override SecurityElement ToXml()
	{
		throw null;
	}

	public override IPermission Union(IPermission target)
	{
		throw null;
	}
}
