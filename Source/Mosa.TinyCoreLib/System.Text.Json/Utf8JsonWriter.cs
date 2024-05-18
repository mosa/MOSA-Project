using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Text.Json;

public sealed class Utf8JsonWriter : IAsyncDisposable, IDisposable
{
	public long BytesCommitted
	{
		get
		{
			throw null;
		}
	}

	public int BytesPending
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

	public JsonWriterOptions Options
	{
		get
		{
			throw null;
		}
	}

	public Utf8JsonWriter(IBufferWriter<byte> bufferWriter, JsonWriterOptions options = default(JsonWriterOptions))
	{
	}

	public Utf8JsonWriter(Stream utf8Json, JsonWriterOptions options = default(JsonWriterOptions))
	{
	}

	public void Dispose()
	{
	}

	public ValueTask DisposeAsync()
	{
		throw null;
	}

	public void Flush()
	{
	}

	public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public void Reset()
	{
	}

	public void Reset(IBufferWriter<byte> bufferWriter)
	{
	}

	public void Reset(Stream utf8Json)
	{
	}

	public void WriteBase64String(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<byte> bytes)
	{
	}

	public void WriteBase64String(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> bytes)
	{
	}

	public void WriteBase64String(string propertyName, ReadOnlySpan<byte> bytes)
	{
	}

	public void WriteBase64String(JsonEncodedText propertyName, ReadOnlySpan<byte> bytes)
	{
	}

	public void WriteBase64StringValue(ReadOnlySpan<byte> bytes)
	{
	}

	public void WriteBoolean(ReadOnlySpan<byte> utf8PropertyName, bool value)
	{
	}

	public void WriteBoolean(ReadOnlySpan<char> propertyName, bool value)
	{
	}

	public void WriteBoolean(string propertyName, bool value)
	{
	}

	public void WriteBoolean(JsonEncodedText propertyName, bool value)
	{
	}

	public void WriteBooleanValue(bool value)
	{
	}

	public void WriteCommentValue(ReadOnlySpan<byte> utf8Value)
	{
	}

	public void WriteCommentValue(ReadOnlySpan<char> value)
	{
	}

	public void WriteCommentValue(string value)
	{
	}

	public void WriteEndArray()
	{
	}

	public void WriteEndObject()
	{
	}

	public void WriteNull(ReadOnlySpan<byte> utf8PropertyName)
	{
	}

	public void WriteNull(ReadOnlySpan<char> propertyName)
	{
	}

	public void WriteNull(string propertyName)
	{
	}

	public void WriteNull(JsonEncodedText propertyName)
	{
	}

	public void WriteNullValue()
	{
	}

	public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, decimal value)
	{
	}

	public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, double value)
	{
	}

	public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, int value)
	{
	}

	public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, long value)
	{
	}

	public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, float value)
	{
	}

	[CLSCompliant(false)]
	public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, uint value)
	{
	}

	[CLSCompliant(false)]
	public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, ulong value)
	{
	}

	public void WriteNumber(ReadOnlySpan<char> propertyName, decimal value)
	{
	}

	public void WriteNumber(ReadOnlySpan<char> propertyName, double value)
	{
	}

	public void WriteNumber(ReadOnlySpan<char> propertyName, int value)
	{
	}

	public void WriteNumber(ReadOnlySpan<char> propertyName, long value)
	{
	}

	public void WriteNumber(ReadOnlySpan<char> propertyName, float value)
	{
	}

	[CLSCompliant(false)]
	public void WriteNumber(ReadOnlySpan<char> propertyName, uint value)
	{
	}

	[CLSCompliant(false)]
	public void WriteNumber(ReadOnlySpan<char> propertyName, ulong value)
	{
	}

	public void WriteNumber(string propertyName, decimal value)
	{
	}

	public void WriteNumber(string propertyName, double value)
	{
	}

	public void WriteNumber(string propertyName, int value)
	{
	}

	public void WriteNumber(string propertyName, long value)
	{
	}

	public void WriteNumber(string propertyName, float value)
	{
	}

	[CLSCompliant(false)]
	public void WriteNumber(string propertyName, uint value)
	{
	}

	[CLSCompliant(false)]
	public void WriteNumber(string propertyName, ulong value)
	{
	}

	public void WriteNumber(JsonEncodedText propertyName, decimal value)
	{
	}

	public void WriteNumber(JsonEncodedText propertyName, double value)
	{
	}

	public void WriteNumber(JsonEncodedText propertyName, int value)
	{
	}

	public void WriteNumber(JsonEncodedText propertyName, long value)
	{
	}

	public void WriteNumber(JsonEncodedText propertyName, float value)
	{
	}

	[CLSCompliant(false)]
	public void WriteNumber(JsonEncodedText propertyName, uint value)
	{
	}

	[CLSCompliant(false)]
	public void WriteNumber(JsonEncodedText propertyName, ulong value)
	{
	}

	public void WriteNumberValue(decimal value)
	{
	}

	public void WriteNumberValue(double value)
	{
	}

	public void WriteNumberValue(int value)
	{
	}

	public void WriteNumberValue(long value)
	{
	}

	public void WriteNumberValue(float value)
	{
	}

	[CLSCompliant(false)]
	public void WriteNumberValue(uint value)
	{
	}

	[CLSCompliant(false)]
	public void WriteNumberValue(ulong value)
	{
	}

	public void WritePropertyName(ReadOnlySpan<byte> utf8PropertyName)
	{
	}

	public void WritePropertyName(ReadOnlySpan<char> propertyName)
	{
	}

	public void WritePropertyName(string propertyName)
	{
	}

	public void WritePropertyName(JsonEncodedText propertyName)
	{
	}

	public void WriteRawValue(ReadOnlySequence<byte> utf8Json, bool skipInputValidation = false)
	{
	}

	public void WriteRawValue(ReadOnlySpan<byte> utf8Json, bool skipInputValidation = false)
	{
	}

	public void WriteRawValue([StringSyntax("Json")] ReadOnlySpan<char> json, bool skipInputValidation = false)
	{
	}

	public void WriteRawValue([StringSyntax("Json")] string json, bool skipInputValidation = false)
	{
	}

	public void WriteStartArray()
	{
	}

	public void WriteStartArray(ReadOnlySpan<byte> utf8PropertyName)
	{
	}

	public void WriteStartArray(ReadOnlySpan<char> propertyName)
	{
	}

	public void WriteStartArray(string propertyName)
	{
	}

	public void WriteStartArray(JsonEncodedText propertyName)
	{
	}

	public void WriteStartObject()
	{
	}

	public void WriteStartObject(ReadOnlySpan<byte> utf8PropertyName)
	{
	}

	public void WriteStartObject(ReadOnlySpan<char> propertyName)
	{
	}

	public void WriteStartObject(string propertyName)
	{
	}

	public void WriteStartObject(JsonEncodedText propertyName)
	{
	}

	public void WriteString(ReadOnlySpan<byte> utf8PropertyName, DateTime value)
	{
	}

	public void WriteString(ReadOnlySpan<byte> utf8PropertyName, DateTimeOffset value)
	{
	}

	public void WriteString(ReadOnlySpan<byte> utf8PropertyName, Guid value)
	{
	}

	public void WriteString(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<byte> utf8Value)
	{
	}

	public void WriteString(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<char> value)
	{
	}

	public void WriteString(ReadOnlySpan<byte> utf8PropertyName, string? value)
	{
	}

	public void WriteString(ReadOnlySpan<byte> utf8PropertyName, JsonEncodedText value)
	{
	}

	public void WriteString(ReadOnlySpan<char> propertyName, DateTime value)
	{
	}

	public void WriteString(ReadOnlySpan<char> propertyName, DateTimeOffset value)
	{
	}

	public void WriteString(ReadOnlySpan<char> propertyName, Guid value)
	{
	}

	public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> utf8Value)
	{
	}

	public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
	{
	}

	public void WriteString(ReadOnlySpan<char> propertyName, string? value)
	{
	}

	public void WriteString(ReadOnlySpan<char> propertyName, JsonEncodedText value)
	{
	}

	public void WriteString(string propertyName, DateTime value)
	{
	}

	public void WriteString(string propertyName, DateTimeOffset value)
	{
	}

	public void WriteString(string propertyName, Guid value)
	{
	}

	public void WriteString(string propertyName, ReadOnlySpan<byte> utf8Value)
	{
	}

	public void WriteString(string propertyName, ReadOnlySpan<char> value)
	{
	}

	public void WriteString(string propertyName, string? value)
	{
	}

	public void WriteString(string propertyName, JsonEncodedText value)
	{
	}

	public void WriteString(JsonEncodedText propertyName, DateTime value)
	{
	}

	public void WriteString(JsonEncodedText propertyName, DateTimeOffset value)
	{
	}

	public void WriteString(JsonEncodedText propertyName, Guid value)
	{
	}

	public void WriteString(JsonEncodedText propertyName, ReadOnlySpan<byte> utf8Value)
	{
	}

	public void WriteString(JsonEncodedText propertyName, ReadOnlySpan<char> value)
	{
	}

	public void WriteString(JsonEncodedText propertyName, string? value)
	{
	}

	public void WriteString(JsonEncodedText propertyName, JsonEncodedText value)
	{
	}

	public void WriteStringValue(DateTime value)
	{
	}

	public void WriteStringValue(DateTimeOffset value)
	{
	}

	public void WriteStringValue(Guid value)
	{
	}

	public void WriteStringValue(ReadOnlySpan<byte> utf8Value)
	{
	}

	public void WriteStringValue(ReadOnlySpan<char> value)
	{
	}

	public void WriteStringValue(string? value)
	{
	}

	public void WriteStringValue(JsonEncodedText value)
	{
	}
}
