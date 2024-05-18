using System.Diagnostics.CodeAnalysis;

namespace System.Formats.Asn1;

public readonly struct Asn1Tag : IEquatable<Asn1Tag>
{
	private readonly int _dummyPrimitive;

	public static readonly Asn1Tag Boolean;

	public static readonly Asn1Tag ConstructedBitString;

	public static readonly Asn1Tag ConstructedOctetString;

	public static readonly Asn1Tag Enumerated;

	public static readonly Asn1Tag GeneralizedTime;

	public static readonly Asn1Tag Integer;

	public static readonly Asn1Tag Null;

	public static readonly Asn1Tag ObjectIdentifier;

	public static readonly Asn1Tag PrimitiveBitString;

	public static readonly Asn1Tag PrimitiveOctetString;

	public static readonly Asn1Tag Sequence;

	public static readonly Asn1Tag SetOf;

	public static readonly Asn1Tag UtcTime;

	public bool IsConstructed
	{
		get
		{
			throw null;
		}
	}

	public TagClass TagClass
	{
		get
		{
			throw null;
		}
	}

	public int TagValue
	{
		get
		{
			throw null;
		}
	}

	public Asn1Tag(TagClass tagClass, int tagValue, bool isConstructed = false)
	{
		throw null;
	}

	public Asn1Tag(UniversalTagNumber universalTagNumber, bool isConstructed = false)
	{
		throw null;
	}

	public Asn1Tag AsConstructed()
	{
		throw null;
	}

	public Asn1Tag AsPrimitive()
	{
		throw null;
	}

	public int CalculateEncodedSize()
	{
		throw null;
	}

	public static Asn1Tag Decode(ReadOnlySpan<byte> source, out int bytesConsumed)
	{
		throw null;
	}

	public int Encode(Span<byte> destination)
	{
		throw null;
	}

	public bool Equals(Asn1Tag other)
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

	public bool HasSameClassAndValue(Asn1Tag other)
	{
		throw null;
	}

	public static bool operator ==(Asn1Tag left, Asn1Tag right)
	{
		throw null;
	}

	public static bool operator !=(Asn1Tag left, Asn1Tag right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static bool TryDecode(ReadOnlySpan<byte> source, out Asn1Tag tag, out int bytesConsumed)
	{
		throw null;
	}

	public bool TryEncode(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
