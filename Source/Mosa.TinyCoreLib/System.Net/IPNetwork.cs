using System.Diagnostics.CodeAnalysis;

namespace System.Net;

public readonly struct IPNetwork : IEquatable<IPNetwork>, IFormattable, IParsable<IPNetwork>, ISpanFormattable, ISpanParsable<IPNetwork>, IUtf8SpanFormattable
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public IPAddress BaseAddress
	{
		get
		{
			throw null;
		}
	}

	public int PrefixLength
	{
		get
		{
			throw null;
		}
	}

	public IPNetwork(IPAddress baseAddress, int prefixLength)
	{
		throw null;
	}

	public bool Contains(IPAddress address)
	{
		throw null;
	}

	public bool Equals(IPNetwork other)
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

	public static bool operator ==(IPNetwork left, IPNetwork right)
	{
		throw null;
	}

	public static bool operator !=(IPNetwork left, IPNetwork right)
	{
		throw null;
	}

	public static IPNetwork Parse(ReadOnlySpan<char> s)
	{
		throw null;
	}

	public static IPNetwork Parse(string s)
	{
		throw null;
	}

	string IFormattable.ToString(string? format, IFormatProvider? provider)
	{
		throw null;
	}

	static IPNetwork IParsable<IPNetwork>.Parse([NotNull] string s, IFormatProvider? provider)
	{
		throw null;
	}

	static bool IParsable<IPNetwork>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out IPNetwork result)
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

	static IPNetwork ISpanParsable<IPNetwork>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	static bool ISpanParsable<IPNetwork>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out IPNetwork result)
	{
		throw null;
	}

	public override string ToString()
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

	public static bool TryParse(ReadOnlySpan<char> s, out IPNetwork result)
	{
		throw null;
	}

	public static bool TryParse(string? s, out IPNetwork result)
	{
		throw null;
	}
}
