using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Text.Json;

public sealed class JsonDocument : IDisposable
{
	public JsonElement RootElement
	{
		get
		{
			throw null;
		}
	}

	internal JsonDocument()
	{
	}

	public void Dispose()
	{
	}

	public static JsonDocument Parse(ReadOnlySequence<byte> utf8Json, JsonDocumentOptions options = default(JsonDocumentOptions))
	{
		throw null;
	}

	public static JsonDocument Parse(Stream utf8Json, JsonDocumentOptions options = default(JsonDocumentOptions))
	{
		throw null;
	}

	public static JsonDocument Parse(ReadOnlyMemory<byte> utf8Json, JsonDocumentOptions options = default(JsonDocumentOptions))
	{
		throw null;
	}

	public static JsonDocument Parse([StringSyntax("Json")] ReadOnlyMemory<char> json, JsonDocumentOptions options = default(JsonDocumentOptions))
	{
		throw null;
	}

	public static JsonDocument Parse([StringSyntax("Json")] string json, JsonDocumentOptions options = default(JsonDocumentOptions))
	{
		throw null;
	}

	public static Task<JsonDocument> ParseAsync(Stream utf8Json, JsonDocumentOptions options = default(JsonDocumentOptions), CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static JsonDocument ParseValue(ref Utf8JsonReader reader)
	{
		throw null;
	}

	public static bool TryParseValue(ref Utf8JsonReader reader, [NotNullWhen(true)] out JsonDocument? document)
	{
		throw null;
	}

	public void WriteTo(Utf8JsonWriter writer)
	{
	}
}
