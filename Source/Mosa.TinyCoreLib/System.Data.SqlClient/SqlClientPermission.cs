using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.SqlClient;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class SqlClientPermission : DBDataPermission
{
	public SqlClientPermission()
	{
	}

	public SqlClientPermission(PermissionState state)
	{
	}

	public SqlClientPermission(PermissionState state, bool allowBlankPassword)
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
