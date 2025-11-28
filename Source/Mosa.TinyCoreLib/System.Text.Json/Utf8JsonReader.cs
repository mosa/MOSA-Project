using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json;

public ref struct Utf8JsonReader
{
	private object _dummy;

	private int _dummyPrimitive;

	public readonly long BytesConsumed
	{
		get
		{
			throw null;
		}
	}

	public readonly int CurrentDepth
	{
		get
		{
			throw null;
		}
	}

	public readonly JsonReaderState CurrentState
	{
		get
		{
			throw null;
		}
	}

	public readonly bool HasValueSequence
	{
		get
		{
			throw null;
		}
	}

	public readonly bool IsFinalBlock
	{
		get
		{
			throw null;
		}
	}

	public readonly SequencePosition Position
	{
		get
		{
			throw null;
		}
	}

	public readonly long TokenStartIndex
	{
		get
		{
			throw null;
		}
	}

	public readonly JsonTokenType TokenType
	{
		get
		{
			throw null;
		}
	}

	public readonly bool ValueIsEscaped
	{
		get
		{
			throw null;
		}
	}

	public readonly ReadOnlySequence<byte> ValueSequence
	{
		get
		{
			throw null;
		}
	}

	public readonly ReadOnlySpan<byte> ValueSpan
	{
		get
		{
			throw null;
		}
	}

	public Utf8JsonReader(ReadOnlySequence<byte> jsonData, bool isFinalBlock, JsonReaderState state)
	{
		throw null;
	}

	public Utf8JsonReader(ReadOnlySequence<byte> jsonData, JsonReaderOptions options = default(JsonReaderOptions))
	{
		throw null;
	}

	public Utf8JsonReader(ReadOnlySpan<byte> jsonData, bool isFinalBlock, JsonReaderState state)
	{
		throw null;
	}

	public Utf8JsonReader(ReadOnlySpan<byte> jsonData, JsonReaderOptions options = default(JsonReaderOptions))
	{
		throw null;
	}

	public readonly int CopyString(Span<byte> utf8Destination)
	{
		throw null;
	}

	public readonly int CopyString(Span<char> destination)
	{
		throw null;
	}

	public bool GetBoolean()
	{
		throw null;
	}

	public byte GetByte()
	{
		throw null;
	}

	public byte[] GetBytesFromBase64()
	{
		throw null;
	}

	public string GetComment()
	{
		throw null;
	}

	public DateTime GetDateTime()
	{
		throw null;
	}

	public DateTimeOffset GetDateTimeOffset()
	{
		throw null;
	}

	public decimal GetDecimal()
	{
		throw null;
	}

	public double GetDouble()
	{
		throw null;
	}

	public Guid GetGuid()
	{
		throw null;
	}

	public short GetInt16()
	{
		throw null;
	}

	public int GetInt32()
	{
		throw null;
	}

	public long GetInt64()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public sbyte GetSByte()
	{
		throw null;
	}

	public float GetSingle()
	{
		throw null;
	}

	public string? GetString()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public ushort GetUInt16()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public uint GetUInt32()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public ulong GetUInt64()
	{
		throw null;
	}

	public bool Read()
	{
		throw null;
	}

	public void Skip()
	{
	}

	public bool TryGetByte(out byte value)
	{
		throw null;
	}

	public bool TryGetBytesFromBase64([NotNullWhen(true)] out byte[]? value)
	{
		throw null;
	}

	public bool TryGetDateTime(out DateTime value)
	{
		throw null;
	}

	public bool TryGetDateTimeOffset(out DateTimeOffset value)
	{
		throw null;
	}

	public bool TryGetDecimal(out decimal value)
	{
		throw null;
	}

	public bool TryGetDouble(out double value)
	{
		throw null;
	}

	public bool TryGetGuid(out Guid value)
	{
		throw null;
	}

	public bool TryGetInt16(out short value)
	{
		throw null;
	}

	public bool TryGetInt32(out int value)
	{
		throw null;
	}

	public bool TryGetInt64(out long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public bool TryGetSByte(out sbyte value)
	{
		throw null;
	}

	public bool TryGetSingle(out float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public bool TryGetUInt16(out ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public bool TryGetUInt32(out uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public bool TryGetUInt64(out ulong value)
	{
		throw null;
	}

	public bool TrySkip()
	{
		throw null;
	}

	public readonly bool ValueTextEquals(ReadOnlySpan<byte> utf8Text)
	{
		throw null;
	}

	public readonly bool ValueTextEquals(ReadOnlySpan<char> text)
	{
		throw null;
	}

	public readonly bool ValueTextEquals(string? text)
	{
		throw null;
	}
}
