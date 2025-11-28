using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;

namespace System.Net;

public class IPEndPoint : EndPoint
{
	public const int MaxPort = 65535;

	public const int MinPort = 0;

	public IPAddress Address
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override AddressFamily AddressFamily
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
		set
		{
		}
	}

	public IPEndPoint(long address, int port)
	{
	}

	public IPEndPoint(IPAddress address, int port)
	{
	}

	public override EndPoint Create(SocketAddress socketAddress)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? comparand)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static IPEndPoint Parse(ReadOnlySpan<char> s)
	{
		throw null;
	}

	public static IPEndPoint Parse(string s)
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

	public static bool TryParse(ReadOnlySpan<char> s, [NotNullWhen(true)] out IPEndPoint? result)
	{
		throw null;
	}

	public static bool TryParse(string s, [NotNullWhen(true)] out IPEndPoint? result)
	{
		throw null;
	}
}
