using System.Diagnostics.CodeAnalysis;

namespace System.Net.Sockets;

public struct IPPacketInformation : IEquatable<IPPacketInformation>
{
	private object _dummy;

	private int _dummyPrimitive;

	public IPAddress Address
	{
		get
		{
			throw null;
		}
	}

	public int Interface
	{
		get
		{
			throw null;
		}
	}

	public bool Equals(IPPacketInformation other)
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

	public static bool operator ==(IPPacketInformation packetInformation1, IPPacketInformation packetInformation2)
	{
		throw null;
	}

	public static bool operator !=(IPPacketInformation packetInformation1, IPPacketInformation packetInformation2)
	{
		throw null;
	}
}
