using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;

namespace System.Net;

public class DnsEndPoint : EndPoint
{
	public override AddressFamily AddressFamily
	{
		get
		{
			throw null;
		}
	}

	public string Host
	{
		get
		{
			throw null;
		}
	}

	public int Port
	{
		get
		{
			throw null;
		}
	}

	public DnsEndPoint(string host, int port)
	{
	}

	public DnsEndPoint(string host, int port, AddressFamily addressFamily)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? comparand)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
