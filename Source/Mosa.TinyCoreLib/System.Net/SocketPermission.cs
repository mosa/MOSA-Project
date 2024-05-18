using System.Collections;
using System.Security;
using System.Security.Permissions;

namespace System.Net;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class SocketPermission : CodeAccessPermission, IUnrestrictedPermission
{
	public const int AllPorts = -1;

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

	public SocketPermission(NetworkAccess access, TransportType transport, string hostName, int portNumber)
	{
	}

	public SocketPermission(PermissionState state)
	{
	}

	public void AddPermission(NetworkAccess access, TransportType transport, string hostName, int portNumber)
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
