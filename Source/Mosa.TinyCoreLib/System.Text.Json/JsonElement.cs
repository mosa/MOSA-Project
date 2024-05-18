using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json;

public readonly struct JsonElement
{
	public struct ArrayEnumerator : IEnumerable<JsonElement>, IEnumerable, IEnumerator<JsonElement>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public JsonElement Current
		{
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				throw null;
			}
		}

		public void Dispose()
		{
		}

		public ArrayEnumerator GetEnumerator()
		{
			throw null;
		}

		public bool MoveNext()
		{
			throw null;
		}

		public void Reset()
		{
		}

		IEnumerator<JsonElement> IEnumerable<JsonElement>.GetEnumerator()
		{
			throw null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	public struct ObjectEnumerator : IEnumerable<JsonProperty>, IEnumerable, IEnumerator<JsonProperty>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public JsonProperty Current
		{
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				throw null;
			}
		}

		public void Dispose()
		{
		}

		public ObjectEnumerator GetEnumerator()
		{
			throw null;
		}

		public bool MoveNext()
		{
			throw null;
		}

		public void Reset()
		{
		}

		IEnumerator<JsonProperty> IEnumerable<JsonProperty>.GetEnumerator()
		{
			throw null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public JsonElement this[int index]
	{
		get
		{
			throw null;
		}
	}

	public JsonValueKind ValueKind
	{
		get
		{
			throw null;
		}
	}

	public JsonElement Clone()
	{
		throw null;
	}

	public ArrayEnumerator EnumerateArray()
	{
		throw null;
	}

	public ObjectEnumerator EnumerateObject()
	{
		throw null;
	}

	public int GetArrayLength()
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

	public JsonElement GetProperty(ReadOnlySpan<byte> utf8PropertyName)
	{
		throw null;
	}

	public JsonElement GetProperty(ReadOnlySpan<char> propertyName)
	{
		throw null;
	}

	public JsonElement GetProperty(string propertyName)
	{
		throw null;
	}

	public string GetRawText()
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

	public static JsonElement ParseValue(ref Utf8JsonReader reader)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
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

	public bool TryGetProperty(ReadOnlySpan<byte> utf8PropertyName, out JsonElement value)
	{
		throw null;
	}

	public bool TryGetProperty(ReadOnlySpan<char> propertyName, out JsonElement value)
	{
		throw null;
	}

	public bool TryGetProperty(string propertyName, out JsonElement value)
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

	public static bool TryParseValue(ref Utf8JsonReader reader, [NotNullWhen(true)] out JsonElement? element)
	{
		throw null;
	}

	public bool ValueEquals(ReadOnlySpan<byte> utf8Text)
	{
		throw null;
	}

	public bool ValueEquals(ReadOnlySpan<char> text)
	{
		throw null;
	}

	public bool ValueEquals(string? text)
	{
		throw null;
	}

	public void WriteTo(Utf8JsonWriter writer)
	{
	}
}
