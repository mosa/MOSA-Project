using System.Numerics;

namespace System.Formats.Cbor;

public class CborReader
{
	public bool AllowMultipleRootLevelValues
	{
		get
		{
			throw null;
		}
	}

	public int BytesRemaining
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

	public int CurrentDepth
	{
		get
		{
			throw null;
		}
	}

	public CborReader(ReadOnlyMemory<byte> data, CborConformanceMode conformanceMode = CborConformanceMode.Strict, bool allowMultipleRootLevelValues = false)
	{
	}

	public CborReaderState PeekState()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public CborTag PeekTag()
	{
		throw null;
	}

	public BigInteger ReadBigInteger()
	{
		throw null;
	}

	public bool ReadBoolean()
	{
		throw null;
	}

	public byte[] ReadByteString()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public ulong ReadCborNegativeIntegerRepresentation()
	{
		throw null;
	}

	public DateTimeOffset ReadDateTimeOffset()
	{
		throw null;
	}

	public decimal ReadDecimal()
	{
		throw null;
	}

	public ReadOnlyMemory<byte> ReadDefiniteLengthByteString()
	{
		throw null;
	}

	public ReadOnlyMemory<byte> ReadDefiniteLengthTextStringBytes()
	{
		throw null;
	}

	public double ReadDouble()
	{
		throw null;
	}

	public ReadOnlyMemory<byte> ReadEncodedValue(bool disableConformanceModeChecks = false)
	{
		throw null;
	}

	public void ReadEndArray()
	{
	}

	public void ReadEndIndefiniteLengthByteString()
	{
	}

	public void ReadEndIndefiniteLengthTextString()
	{
	}

	public void ReadEndMap()
	{
	}

	public int ReadInt32()
	{
		throw null;
	}

	public long ReadInt64()
	{
		throw null;
	}

	public void ReadNull()
	{
	}

	public CborSimpleValue ReadSimpleValue()
	{
		throw null;
	}

	public float ReadSingle()
	{
		throw null;
	}

	public int? ReadStartArray()
	{
		throw null;
	}

	public void ReadStartIndefiniteLengthByteString()
	{
	}

	public void ReadStartIndefiniteLengthTextString()
	{
	}

	public int? ReadStartMap()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public CborTag ReadTag()
	{
		throw null;
	}

	public string ReadTextString()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public uint ReadUInt32()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public ulong ReadUInt64()
	{
		throw null;
	}

	public DateTimeOffset ReadUnixTimeSeconds()
	{
		throw null;
	}

	public void Reset(ReadOnlyMemory<byte> data)
	{
	}

	public void SkipToParent(bool disableConformanceModeChecks = false)
	{
	}

	public void SkipValue(bool disableConformanceModeChecks = false)
	{
	}

	public bool TryReadByteString(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryReadTextString(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public Half ReadHalf()
	{
		throw null;
	}
}
