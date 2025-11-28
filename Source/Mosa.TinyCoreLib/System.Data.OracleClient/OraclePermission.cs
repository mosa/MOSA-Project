using System.Security;
using System.Security.Permissions;

namespace System.Data.OracleClient;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class OraclePermission : CodeAccessPermission, IUnrestrictedPermission
{
	public bool AllowBlankPassword
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public OraclePermission(PermissionState state)
	{
	}

	public void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior)
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
