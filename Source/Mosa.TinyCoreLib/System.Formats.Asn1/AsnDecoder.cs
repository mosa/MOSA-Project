using System.Collections;
using System.Numerics;

namespace System.Formats.Asn1;

public static class AsnDecoder
{
	public static byte[] ReadBitString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int unusedBitCount, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static bool ReadBoolean(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static string ReadCharacterString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, UniversalTagNumber encodingType, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static Asn1Tag ReadEncodedValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int contentOffset, out int contentLength, out int bytesConsumed)
	{
		throw null;
	}

	public static ReadOnlySpan<byte> ReadEnumeratedBytes(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static Enum ReadEnumeratedValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Type enumType, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static TEnum ReadEnumeratedValue<TEnum>(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) where TEnum : Enum
	{
		throw null;
	}

	public static DateTimeOffset ReadGeneralizedTime(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static BigInteger ReadInteger(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static ReadOnlySpan<byte> ReadIntegerBytes(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static BitArray ReadNamedBitList(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static Enum ReadNamedBitListValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Type flagsEnumType, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static TFlagsEnum ReadNamedBitListValue<TFlagsEnum>(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) where TFlagsEnum : Enum
	{
		throw null;
	}

	public static void ReadNull(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static string ReadObjectIdentifier(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static byte[] ReadOctetString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static void ReadSequence(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int contentOffset, out int contentLength, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static void ReadSetOf(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int contentOffset, out int contentLength, out int bytesConsumed, bool skipSortOrderValidation = false, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static DateTimeOffset ReadUtcTime(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, int twoDigitYearMax = 2049, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static bool TryReadBitString(ReadOnlySpan<byte> source, Span<byte> destination, AsnEncodingRules ruleSet, out int unusedBitCount, out int bytesConsumed, out int bytesWritten, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static bool TryReadCharacterString(ReadOnlySpan<byte> source, Span<char> destination, AsnEncodingRules ruleSet, UniversalTagNumber encodingType, out int bytesConsumed, out int charsWritten, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static bool TryReadCharacterStringBytes(ReadOnlySpan<byte> source, Span<byte> destination, AsnEncodingRules ruleSet, Asn1Tag expectedTag, out int bytesConsumed, out int bytesWritten)
	{
		throw null;
	}

	public static bool TryReadEncodedValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out Asn1Tag tag, out int contentOffset, out int contentLength, out int bytesConsumed)
	{
		throw null;
	}

	public static bool TryReadInt32(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int value, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static bool TryReadInt64(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out long value, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static bool TryReadOctetString(ReadOnlySpan<byte> source, Span<byte> destination, AsnEncodingRules ruleSet, out int bytesConsumed, out int bytesWritten, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static bool TryReadPrimitiveBitString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int unusedBitCount, out ReadOnlySpan<byte> value, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	public static bool TryReadPrimitiveCharacterStringBytes(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Asn1Tag expectedTag, out ReadOnlySpan<byte> value, out int bytesConsumed)
	{
		throw null;
	}

	public static bool TryReadPrimitiveOctetString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out ReadOnlySpan<byte> value, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool TryReadUInt32(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out uint value, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool TryReadUInt64(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out ulong value, out int bytesConsumed, Asn1Tag? expectedTag = null)
	{
		throw null;
	}
}
