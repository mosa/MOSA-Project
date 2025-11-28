using System.Numerics;

namespace System.Formats.Cbor;

public class CborWriter
{
	public bool AllowMultipleRootLevelValues
	{
		get
		{
			throw null;
		}
	}

	public int BytesWritten
	{
		get
		{
			throw null;
		}
	}

	public CborConformanceMode ConformanceMode
	{
		get
		{
			throw null;
		}
	}

	public bool ConvertIndefiniteLengthEncodings
	{
		get
		{
			throw null;
		}
	}

	public int CurrentDepth
	{
		get
		{
			throw null;
		}
	}

	public bool IsWriteCompleted
	{
		get
		{
			throw null;
		}
	}

	public CborWriter(CborConformanceMode conformanceMode = CborConformanceMode.Strict, bool convertIndefiniteLengthEncodings = false, bool allowMultipleRootLevelValues = false)
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

	public void Reset()
	{
	}

	public bool TryEncode(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public void WriteBigInteger(BigInteger value)
	{
	}

	public void WriteBoolean(bool value)
	{
	}

	public void WriteByteString(byte[] value)
	{
	}

	public void WriteByteString(ReadOnlySpan<byte> value)
	{
	}

	[CLSCompliant(false)]
	public void WriteCborNegativeIntegerRepresentation(ulong value)
	{
	}

	public void WriteDateTimeOffset(DateTimeOffset value)
	{
	}

	public void WriteDecimal(decimal value)
	{
	}

	public void WriteDouble(double value)
	{
	}

	public void WriteEncodedValue(ReadOnlySpan<byte> encodedValue)
	{
	}

	public void WriteEndArray()
	{
	}

	public void WriteEndIndefiniteLengthByteString()
	{
	}

	public void WriteEndIndefiniteLengthTextString()
	{
	}

	public void WriteEndMap()
	{
	}

	public void WriteInt32(int value)
	{
	}

	public void WriteInt64(long value)
	{
	}

	public void WriteNull()
	{
	}

	public void WriteSimpleValue(CborSimpleValue value)
	{
	}

	public void WriteSingle(float value)
	{
	}

	public void WriteStartArray(int? definiteLength)
	{
	}

	public void WriteStartIndefiniteLengthByteString()
	{
	}

	public void WriteStartIndefiniteLengthTextString()
	{
	}

	public void WriteStartMap(int? definiteLength)
	{
	}

	[CLSCompliant(false)]
	public void WriteTag(CborTag tag)
	{
	}

	public void WriteTextString(ReadOnlySpan<char> value)
	{
	}

	public void WriteTextString(string value)
	{
	}

	[CLSCompliant(false)]
	public void WriteUInt32(uint value)
	{
	}

	[CLSCompliant(false)]
	public void WriteUInt64(ulong value)
	{
	}

	public void WriteUnixTimeSeconds(double seconds)
	{
	}

	public void WriteUnixTimeSeconds(long seconds)
	{
	}

	public void WriteHalf(Half value)
	{
	}
}
