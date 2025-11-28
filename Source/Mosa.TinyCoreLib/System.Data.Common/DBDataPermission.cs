using System.Security;
using System.Security.Permissions;

namespace System.Data.Common;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public abstract class DBDataPermission : CodeAccessPermission, IUnrestrictedPermission
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

	protected DBDataPermission()
	{
	}

	protected DBDataPermission(DBDataPermission permission)
	{
	}

	protected DBDataPermission(DBDataPermissionAttribute permissionAttribute)
	{
	}

	protected DBDataPermission(PermissionState state)
	{
	}

	protected DBDataPermission(PermissionState state, bool allowBlankPassword)
	{
	}

	public virtual void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior)
	{
	}

	protected void Clear()
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	protected virtual DBDataPermission CreateInstance()
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
