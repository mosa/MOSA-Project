using System.Diagnostics.CodeAnalysis;

namespace System.Net.NetworkInformation;

public class PhysicalAddress
{
	public static readonly PhysicalAddress None;

	public PhysicalAddress(byte[] address)
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

	public static PhysicalAddress Parse(ReadOnlySpan<char> address)
	{
		throw null;
	}

	public static PhysicalAddress Parse(string? address)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> address, [NotNullWhen(true)] out PhysicalAddress? value)
	{
		throw null;
	}

	public static bool TryParse(string? address, [NotNullWhen(true)] out PhysicalAddress? value)
	{
		throw null;
	}
}
