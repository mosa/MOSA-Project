using System.Diagnostics.CodeAnalysis;

namespace System.Net.Sockets;

public struct UdpReceiveResult : IEquatable<UdpReceiveResult>
{
	private object _dummy;

	private int _dummyPrimitive;

	public byte[] Buffer
	{
		get
		{
			throw null;
		}
	}

	public IPEndPoint RemoteEndPoint
	{
		get
		{
			throw null;
		}
	}

	public UdpReceiveResult(byte[] buffer, IPEndPoint remoteEndPoint)
	{
		throw null;
	}

	public bool Equals(UdpReceiveResult other)
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

	public static bool operator ==(UdpReceiveResult left, UdpReceiveResult right)
	{
		throw null;
	}

	public static bool operator !=(UdpReceiveResult left, UdpReceiveResult right)
	{
		throw null;
	}
}
