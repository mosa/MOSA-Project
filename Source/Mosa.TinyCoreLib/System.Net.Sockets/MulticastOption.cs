namespace System.Net.Sockets;

public class MulticastOption
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

	public int InterfaceIndex
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IPAddress? LocalAddress
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MulticastOption(IPAddress group)
	{
	}

	public MulticastOption(IPAddress group, int interfaceIndex)
	{
	}

	public MulticastOption(IPAddress group, IPAddress mcint)
	{
	}
}
