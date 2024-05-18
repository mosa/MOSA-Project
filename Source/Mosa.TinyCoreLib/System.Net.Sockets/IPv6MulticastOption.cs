namespace System.Net.Sockets;

public class IPv6MulticastOption
{
	public IPAddress Group
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long InterfaceIndex
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IPv6MulticastOption(IPAddress group)
	{
	}

	public IPv6MulticastOption(IPAddress group, long ifindex)
	{
	}
}
