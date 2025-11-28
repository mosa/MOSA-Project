using System.ComponentModel;
using System.Security.Permissions;

namespace System.Data.Common;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public abstract class DBDataPermissionAttribute : CodeAccessSecurityAttribute
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

	public string ConnectionString
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public KeyRestrictionBehavior KeyRestrictionBehavior
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string KeyRestrictions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected DBDataPermissionAttribute(SecurityAction action)
		: base((SecurityAction)0)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeConnectionString()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeKeyRestrictions()
	{
		throw null;
	}
}
