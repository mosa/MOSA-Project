using System.Collections;
using System.Numerics;

namespace System.Formats.Asn1;

public class AsnReader
{
	public bool HasData
	{
		get
		{
			throw null;
		}
	}

	public AsnEncodingRules RuleSet
	{
		get
		{
			throw null;
		}
	}

	public AsnReader(ReadOnlyMemory<byte> data, AsnEncodingRules ruleSet, AsnReaderOptions options = default(AsnReaderOptions))
	{
	}

	public AsnReader Clone()
	{
		throw null;
	}

	public ReadOnlyMemory<byte> PeekContentBytes()
	{
		throw null;
	}

	public ReadOnlyMemory<byte> PeekEncodedValue()
	{
		throw null;
	}

	public Asn1Tag PeekTag()
	{
		throw null;
	}

	public byte[] ReadBitString(out int unusedBitCount, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public bool ReadBoolean(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public string ReadCharacterString(UniversalTagNumber encodingType, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public ReadOnlyMemory<byte> ReadEncodedValue()
	{
		throw null;
	}

	public ReadOnlyMemory<byte> ReadEnumeratedBytes(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public Enum ReadEnumeratedValue(Type enumType, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public TEnum ReadEnumeratedValue<TEnum>(Asn1Tag? expectedTag = null) where TEnum : Enum
	{
		throw null;
	}

	public DateTimeOffset ReadGeneralizedTime(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public BigInteger ReadInteger(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public ReadOnlyMemory<byte> ReadIntegerBytes(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public BitArray ReadNamedBitList(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public Enum ReadNamedBitListValue(Type flagsEnumType, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public TFlagsEnum ReadNamedBitListValue<TFlagsEnum>(Asn1Tag? expectedTag = null) where TFlagsEnum : Enum
	{
		throw null;
	}

	public void ReadNull(Asn1Tag? expectedTag = null)
	{
	}

	public string ReadObjectIdentifier(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public byte[] ReadOctetString(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public AsnReader ReadSequence(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public AsnReader ReadSetOf(bool skipSortOrderValidation, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public AsnReader ReadSetOf(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public DateTimeOffset ReadUtcTime(int twoDigitYearMax, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public DateTimeOffset ReadUtcTime(Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public void ThrowIfNotEmpty()
	{
	}

	public bool TryReadBitString(Span<byte> destination, out int unusedBitCount, out int bytesWritten, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public bool TryReadCharacterString(Span<char> destination, UniversalTagNumber encodingType, out int charsWritten, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public bool TryReadCharacterStringBytes(Span<byte> destination, Asn1Tag expectedTag, out int bytesWritten)
	{
		throw null;
	}

	public bool TryReadInt32(out int value, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public bool TryReadInt64(out long value, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public bool TryReadOctetString(Span<byte> destination, out int bytesWritten, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public bool TryReadPrimitiveBitString(out int unusedBitCount, out ReadOnlyMemory<byte> value, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public bool TryReadPrimitiveCharacterStringBytes(Asn1Tag expectedTag, out ReadOnlyMemory<byte> contents)
	{
		throw null;
	}

	public bool TryReadPrimitiveOctetString(out ReadOnlyMemory<byte> contents, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public bool TryReadUInt32(out uint value, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public bool TryReadUInt64(out ulong value, Asn1Tag? expectedTag = null)
	{
		throw null;
	}
}
