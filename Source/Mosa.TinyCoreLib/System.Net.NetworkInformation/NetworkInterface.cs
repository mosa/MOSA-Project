using System.Runtime.Versioning;

namespace System.Net.NetworkInformation;

public abstract class NetworkInterface
{
	public virtual string Description
	{
		get
		{
			throw null;
		}
	}

	public virtual string Id
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("illumos")]
	[UnsupportedOSPlatform("solaris")]
	public static int IPv6LoopbackInterfaceIndex
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsReceiveOnly
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("illumos")]
	[UnsupportedOSPlatform("solaris")]
	public static int LoopbackInterfaceIndex
	{
		get
		{
			throw null;
		}
	}

	public virtual string Name
	{
		get
		{
			throw null;
		}
	}

	public virtual NetworkInterfaceType NetworkInterfaceType
	{
		get
		{
			throw null;
		}
	}

	public virtual OperationalStatus OperationalStatus
	{
		get
		{
			throw null;
		}
	}

	public virtual long Speed
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsMulticast
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("illumos")]
	[UnsupportedOSPlatform("solaris")]
	public static NetworkInterface[] GetAllNetworkInterfaces()
	{
		throw null;
	}

	public virtual IPInterfaceProperties GetIPProperties()
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	public virtual IPInterfaceStatistics GetIPStatistics()
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	public virtual IPv4InterfaceStatistics GetIPv4Statistics()
	{
		throw null;
	}

	[UnsupportedOSPlatform("illumos")]
	[UnsupportedOSPlatform("solaris")]
	public static bool GetIsNetworkAvailable()
	{
		throw null;
	}

	public virtual PhysicalAddress GetPhysicalAddress()
	{
		throw null;
	}

	public virtual bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
	{
		throw null;
	}
}
