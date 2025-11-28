using System.Collections;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.Net;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class WebPermission : CodeAccessPermission, IUnrestrictedPermission
{
	public IEnumerator AcceptList
	{
		get
		{
			throw null;
		}
	}

	public IEnumerator ConnectList
	{
		get
		{
			throw null;
		}
	}

	public WebPermission()
	{
	}

	public WebPermission(NetworkAccess access, string uriString)
	{
	}

	public WebPermission(NetworkAccess access, Regex uriRegex)
	{
	}

	public WebPermission(PermissionState state)
	{
	}

	public void AddPermission(NetworkAccess access, string uriString)
	{
	}

	public void AddPermission(NetworkAccess access, Regex uriRegex)
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
