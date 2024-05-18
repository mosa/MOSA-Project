using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace System.Net.NetworkInformation;

public abstract class IPGlobalProperties
{
	[UnsupportedOSPlatform("android")]
	public abstract string DhcpScopeName { get; }

	public abstract string DomainName { get; }

	public abstract string HostName { get; }

	[UnsupportedOSPlatform("android")]
	public abstract bool IsWinsProxy { get; }

	public abstract NetBiosNodeType NodeType { get; }

	public virtual IAsyncResult BeginGetUnicastAddresses(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public virtual UnicastIPAddressInformationCollection EndGetUnicastAddresses(IAsyncResult asyncResult)
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	public abstract TcpConnectionInformation[] GetActiveTcpConnections();

	[UnsupportedOSPlatform("android")]
	public abstract IPEndPoint[] GetActiveTcpListeners();

	[UnsupportedOSPlatform("android")]
	public abstract IPEndPoint[] GetActiveUdpListeners();

	[UnsupportedOSPlatform("android")]
	public abstract IcmpV4Statistics GetIcmpV4Statistics();

	[UnsupportedOSPlatform("android")]
	public abstract IcmpV6Statistics GetIcmpV6Statistics();

	[UnsupportedOSPlatform("illumos")]
	[UnsupportedOSPlatform("solaris")]
	public static IPGlobalProperties GetIPGlobalProperties()
	{
		throw null;
	}

	public abstract IPGlobalStatistics GetIPv4GlobalStatistics();

	[UnsupportedOSPlatform("freebsd")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("osx")]
	[UnsupportedOSPlatform("tvos")]
	public abstract IPGlobalStatistics GetIPv6GlobalStatistics();

	[UnsupportedOSPlatform("android")]
	public abstract TcpStatistics GetTcpIPv4Statistics();

	[UnsupportedOSPlatform("android")]
	public abstract TcpStatistics GetTcpIPv6Statistics();

	[UnsupportedOSPlatform("android")]
	public abstract UdpStatistics GetUdpIPv4Statistics();

	[UnsupportedOSPlatform("android")]
	public abstract UdpStatistics GetUdpIPv6Statistics();

	public virtual UnicastIPAddressInformationCollection GetUnicastAddresses()
	{
		throw null;
	}

	public virtual Task<UnicastIPAddressInformationCollection> GetUnicastAddressesAsync()
	{
		throw null;
	}
}
