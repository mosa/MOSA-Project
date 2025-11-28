using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;

namespace System.Net;

public class IPAddress : ISpanFormattable, IFormattable, ISpanParsable<IPAddress>, IParsable<IPAddress>, IUtf8SpanFormattable
{
	public static readonly IPAddress Any;

	public static readonly IPAddress Broadcast;

	public static readonly IPAddress IPv6Any;

	public static readonly IPAddress IPv6Loopback;

	public static readonly IPAddress IPv6None;

	public static readonly IPAddress Loopback;

	public static readonly IPAddress None;

	[Obsolete("IPAddress.Address is address family dependent and has been deprecated. Use IPAddress.Equals to perform comparisons instead.")]
	public long Address
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AddressFamily AddressFamily
	{
		get
		{
			throw null;
		}
	}

	public bool IsIPv4MappedToIPv6
	{
		get
		{
			throw null;
		}
	}

	public bool IsIPv6LinkLocal
	{
		get
		{
			throw null;
		}
	}

	public bool IsIPv6Multicast
	{
		get
		{
			throw null;
		}
	}

	public bool IsIPv6SiteLocal
	{
		get
		{
			throw null;
		}
	}

	public bool IsIPv6Teredo
	{
		get
		{
			throw null;
		}
	}

	public bool IsIPv6UniqueLocal
	{
		get
		{
			throw null;
		}
	}

	public long ScopeId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IPAddress(byte[] address)
	{
	}

	public IPAddress(byte[] address, long scopeid)
	{
	}

	public IPAddress(long newAddress)
	{
	}

	public IPAddress(ReadOnlySpan<byte> address)
	{
	}

	public IPAddress(ReadOnlySpan<byte> address, long scopeid)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? comparand)
	{
		throw null;
	}

	public byte[] GetAddressBytes()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static short HostToNetworkOrder(short host)
	{
		throw null;
	}

	public static int HostToNetworkOrder(int host)
	{
		throw null;
	}

	public static long HostToNetworkOrder(long host)
	{
		throw null;
	}

	public static bool IsLoopback(IPAddress address)
	{
		throw null;
	}

	public IPAddress MapToIPv4()
	{
		throw null;
	}

	public IPAddress MapToIPv6()
	{
		throw null;
	}

	public static short NetworkToHostOrder(short network)
	{
		throw null;
	}

	public static int NetworkToHostOrder(int network)
	{
		throw null;
	}

	public static long NetworkToHostOrder(long network)
	{
		throw null;
	}

	public static IPAddress Parse(ReadOnlySpan<char> ipSpan)
	{
		throw null;
	}

	public static IPAddress Parse(string ipString)
	{
		throw null;
	}

	static IPAddress ISpanParsable<IPAddress>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	static IPAddress IParsable<IPAddress>.Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten)
	{
		throw null;
	}

	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	bool IUtf8SpanFormattable.TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> ipSpan, [NotNullWhen(true)] out IPAddress? address)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? ipString, [NotNullWhen(true)] out IPAddress? address)
	{
		throw null;
	}

	static bool ISpanParsable<IPAddress>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out IPAddress result)
	{
		throw null;
	}

	static bool IParsable<IPAddress>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [NotNullWhen(true)] out IPAddress? result)
	{
		throw null;
	}

	public bool TryWriteBytes(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
