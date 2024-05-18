using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;

namespace System.Xaml.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class XamlLoadPermission : CodeAccessPermission, IUnrestrictedPermission
{
	[SupportedOSPlatform("windows")]
	public IList<XamlAccessLevel> AllowedAccess
	{
		get
		{
			throw null;
		}
	}

	public XamlLoadPermission(IEnumerable<XamlAccessLevel> allowedAccess)
	{
	}

	public XamlLoadPermission(PermissionState state)
	{
	}

	public XamlLoadPermission(XamlAccessLevel allowedAccess)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public override void FromXml(SecurityElement elem)
	{
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public bool Includes(XamlAccessLevel requestedAccess)
	{
		throw null;
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

	public override IPermission Union(IPermission other)
	{
		throw null;
	}
}
