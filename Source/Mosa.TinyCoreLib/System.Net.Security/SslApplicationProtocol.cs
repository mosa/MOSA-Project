using System.Diagnostics.CodeAnalysis;

namespace System.Net.Security;

public readonly struct SslApplicationProtocol : IEquatable<SslApplicationProtocol>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static readonly SslApplicationProtocol Http11;

	public static readonly SslApplicationProtocol Http2;

	public static readonly SslApplicationProtocol Http3;

	public ReadOnlyMemory<byte> Protocol
	{
		get
		{
			throw null;
		}
	}

	public SslApplicationProtocol(byte[] protocol)
	{
		throw null;
	}

	public SslApplicationProtocol(string protocol)
	{
		throw null;
	}

	public bool Equals(SslApplicationProtocol other)
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

	public static bool operator ==(SslApplicationProtocol left, SslApplicationProtocol right)
	{
		throw null;
	}

	public static bool operator !=(SslApplicationProtocol left, SslApplicationProtocol right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
