using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.Odbc;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class OdbcPermission : DBDataPermission
{
	public OdbcPermission()
	{
	}

	public OdbcPermission(PermissionState state)
	{
	}

	public OdbcPermission(PermissionState state, bool allowBlankPassword)
	{
	}

	public override void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}
}
