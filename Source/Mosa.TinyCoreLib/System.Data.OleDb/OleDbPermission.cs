using System.ComponentModel;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.OleDb;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class OleDbPermission : DBDataPermission
{
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string Provider
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public OleDbPermission()
	{
	}

	public OleDbPermission(PermissionState state)
	{
	}

	public OleDbPermission(PermissionState state, bool allowBlankPassword)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}
}
