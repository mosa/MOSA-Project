using System.Collections;
using System.Numerics;

namespace System.Formats.Asn1;

public sealed class AsnWriter
{
	public readonly struct Scope : IDisposable
	{
		private readonly object _dummy;

		private readonly int _dummyPrimitive;

		public void Dispose()
		{
		}
	}

	public AsnEncodingRules RuleSet
	{
		get
		{
			throw null;
		}
	}

	public AsnWriter(AsnEncodingRules ruleSet)
	{
	}

	public AsnWriter(AsnEncodingRules ruleSet, int initialCapacity)
	{
	}

	public void CopyTo(AsnWriter destination)
	{
	}

	public byte[] Encode()
	{
		throw null;
	}

	public int Encode(Span<byte> destination)
	{
		throw null;
	}

	public bool EncodedValueEquals(AsnWriter other)
	{
		throw null;
	}

	public bool EncodedValueEquals(ReadOnlySpan<byte> other)
	{
		throw null;
	}

	public int GetEncodedLength()
	{
		throw null;
	}

	public void PopOctetString(Asn1Tag? tag = null)
	{
	}

	public void PopSequence(Asn1Tag? tag = null)
	{
	}

	public void PopSetOf(Asn1Tag? tag = null)
	{
	}

	public Scope PushOctetString(Asn1Tag? tag = null)
	{
		throw null;
	}

	public Scope PushSequence(Asn1Tag? tag = null)
	{
		throw null;
	}

	public Scope PushSetOf(Asn1Tag? tag = null)
	{
		throw null;
	}

	public void Reset()
	{
	}

	public bool TryEncode(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public void WriteBitString(ReadOnlySpan<byte> value, int unusedBitCount = 0, Asn1Tag? tag = null)
	{
	}

	public void WriteBoolean(bool value, Asn1Tag? tag = null)
	{
	}

	public void WriteCharacterString(UniversalTagNumber encodingType, ReadOnlySpan<char> str, Asn1Tag? tag = null)
	{
	}

	public void WriteCharacterString(UniversalTagNumber encodingType, string value, Asn1Tag? tag = null)
	{
	}

	public void WriteEncodedValue(ReadOnlySpan<byte> value)
	{
	}

	public void WriteEnumeratedValue(Enum value, Asn1Tag? tag = null)
	{
	}

	public void WriteEnumeratedValue<TEnum>(TEnum value, Asn1Tag? tag = null) where TEnum : Enum
	{
	}

	public void WriteGeneralizedTime(DateTimeOffset value, bool omitFractionalSeconds = false, Asn1Tag? tag = null)
	{
	}

	public void WriteInteger(long value, Asn1Tag? tag = null)
	{
	}

	public void WriteInteger(BigInteger value, Asn1Tag? tag = null)
	{
	}

	public void WriteInteger(ReadOnlySpan<byte> value, Asn1Tag? tag = null)
	{
	}

	[CLSCompliant(false)]
	public void WriteInteger(ulong value, Asn1Tag? tag = null)
	{
	}

	public void WriteIntegerUnsigned(ReadOnlySpan<byte> value, Asn1Tag? tag = null)
	{
	}

	public void WriteNamedBitList(BitArray value, Asn1Tag? tag = null)
	{
	}

	public void WriteNamedBitList(Enum value, Asn1Tag? tag = null)
	{
	}

	public void WriteNamedBitList<TEnum>(TEnum value, Asn1Tag? tag = null) where TEnum : Enum
	{
	}

	public void WriteNull(Asn1Tag? tag = null)
	{
	}

	public void WriteObjectIdentifier(ReadOnlySpan<char> oidValue, Asn1Tag? tag = null)
	{
	}

	public void WriteObjectIdentifier(string oidValue, Asn1Tag? tag = null)
	{
	}

	public void WriteOctetString(ReadOnlySpan<byte> value, Asn1Tag? tag = null)
	{
	}

	public void WriteUtcTime(DateTimeOffset value, int twoDigitYearMax, Asn1Tag? tag = null)
	{
	}

	public void WriteUtcTime(DateTimeOffset value, Asn1Tag? tag = null)
	{
	}
}
