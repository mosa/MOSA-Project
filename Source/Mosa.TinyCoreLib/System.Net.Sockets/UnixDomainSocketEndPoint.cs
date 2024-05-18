using System.Diagnostics.CodeAnalysis;

namespace System.Net.Sockets;

public sealed class UnixDomainSocketEndPoint : EndPoint
{
	public override AddressFamily AddressFamily
	{
		get
		{
			throw null;
		}
	}

	public UnixDomainSocketEndPoint(string path)
	{
	}

	public override EndPoint Create(SocketAddress socketAddress)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override SocketAddress Serialize()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
