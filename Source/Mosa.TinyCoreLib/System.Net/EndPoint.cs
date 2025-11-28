using System.Net.Sockets;

namespace System.Net;

public abstract class EndPoint
{
	public virtual AddressFamily AddressFamily
	{
		get
		{
			throw null;
		}
	}

	public virtual EndPoint Create(SocketAddress socketAddress)
	{
		throw null;
	}

	public virtual SocketAddress Serialize()
	{
		throw null;
	}
}
