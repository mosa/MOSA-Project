using System.Runtime.Versioning;

namespace System.Net.NetworkInformation;

public abstract class IPv6InterfaceProperties
{
	public abstract int Index { get; }

	public abstract int Mtu { get; }

	[UnsupportedOSPlatform("freebsd")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("osx")]
	[UnsupportedOSPlatform("tvos")]
	public virtual long GetScopeId(ScopeLevel scopeLevel)
	{
		throw null;
	}
}
