using System.Net.Sockets;

namespace System.Net;

public class SocketAddress : IEquatable<SocketAddress>
{
	public AddressFamily Family
	{
		get
		{
			throw null;
		}
	}

	public byte this[int offset]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Size
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Memory<byte> Buffer
	{
		get
		{
			throw null;
		}
	}

	public SocketAddress(AddressFamily family)
	{
	}

	public SocketAddress(AddressFamily family, int size)
	{
	}

	public static int GetMaximumAddressSize(AddressFamily addressFamily)
	{
		throw null;
	}

	public override bool Equals(object? comparand)
	{
		throw null;
	}

	public bool Equals(SocketAddress? comparand)
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
