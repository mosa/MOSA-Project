using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography.Cose;

public readonly struct CoseHeaderValue : IEquatable<CoseHeaderValue>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ReadOnlyMemory<byte> EncodedValue
	{
		get
		{
			throw null;
		}
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(CoseHeaderValue other)
	{
		throw null;
	}

	public static CoseHeaderValue FromBytes(byte[] value)
	{
		throw null;
	}

	public static CoseHeaderValue FromBytes(ReadOnlySpan<byte> value)
	{
		throw null;
	}

	public static CoseHeaderValue FromEncodedValue(byte[] encodedValue)
	{
		throw null;
	}

	public static CoseHeaderValue FromEncodedValue(ReadOnlySpan<byte> encodedValue)
	{
		throw null;
	}

	public static CoseHeaderValue FromInt32(int value)
	{
		throw null;
	}

	public static CoseHeaderValue FromString(string value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public byte[] GetValueAsBytes()
	{
		throw null;
	}

	public int GetValueAsBytes(Span<byte> destination)
	{
		throw null;
	}

	public int GetValueAsInt32()
	{
		throw null;
	}

	public string GetValueAsString()
	{
		throw null;
	}

	public static bool operator ==(CoseHeaderValue left, CoseHeaderValue right)
	{
		throw null;
	}

	public static bool operator !=(CoseHeaderValue left, CoseHeaderValue right)
	{
		throw null;
	}
}
