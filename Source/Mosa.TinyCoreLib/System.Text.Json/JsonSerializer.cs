using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace System.Text.Json;

public static class JsonSerializer
{
	public static bool IsReflectionEnabledByDefault
	{
		get
		{
			throw null;
		}
	}

	public static object? Deserialize(Stream utf8Json, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static object? Deserialize(Stream utf8Json, Type returnType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static object? Deserialize(Stream utf8Json, Type returnType, JsonSerializerContext context)
	{
		throw null;
	}

	public static object? Deserialize(ReadOnlySpan<byte> utf8Json, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static object? Deserialize(ReadOnlySpan<byte> utf8Json, Type returnType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static object? Deserialize(ReadOnlySpan<byte> utf8Json, Type returnType, JsonSerializerContext context)
	{
		throw null;
	}

	public static object? Deserialize([StringSyntax("Json")] ReadOnlySpan<char> json, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static object? Deserialize([StringSyntax("Json")] ReadOnlySpan<char> json, Type returnType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static object? Deserialize([StringSyntax("Json")] ReadOnlySpan<char> json, Type returnType, JsonSerializerContext context)
	{
		throw null;
	}

	public static object? Deserialize([StringSyntax("Json")] string json, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static object? Deserialize([StringSyntax("Json")] string json, Type returnType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static object? Deserialize([StringSyntax("Json")] string json, Type returnType, JsonSerializerContext context)
	{
		throw null;
	}

	public static object? Deserialize(this JsonDocument document, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static object? Deserialize(this JsonDocument document, Type returnType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static object? Deserialize(this JsonDocument document, Type returnType, JsonSerializerContext context)
	{
		throw null;
	}

	public static object? Deserialize(this JsonElement element, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static object? Deserialize(this JsonElement element, Type returnType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static object? Deserialize(this JsonElement element, Type returnType, JsonSerializerContext context)
	{
		throw null;
	}

	public static object? Deserialize(this JsonNode? node, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static object? Deserialize(this JsonNode? node, Type returnType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static object? Deserialize(this JsonNode? node, Type returnType, JsonSerializerContext context)
	{
		throw null;
	}

	public static object? Deserialize(ref Utf8JsonReader reader, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static object? Deserialize(ref Utf8JsonReader reader, Type returnType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static object? Deserialize(ref Utf8JsonReader reader, Type returnType, JsonSerializerContext context)
	{
		throw null;
	}

	public static ValueTask<object?> DeserializeAsync(Stream utf8Json, JsonTypeInfo jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static ValueTask<object?> DeserializeAsync(Stream utf8Json, Type returnType, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static ValueTask<object?> DeserializeAsync(Stream utf8Json, Type returnType, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static IAsyncEnumerable<TValue?> DeserializeAsyncEnumerable<TValue>(Stream utf8Json, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static IAsyncEnumerable<TValue?> DeserializeAsyncEnumerable<TValue>(Stream utf8Json, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static ValueTask<TValue?> DeserializeAsync<TValue>(Stream utf8Json, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static ValueTask<TValue?> DeserializeAsync<TValue>(Stream utf8Json, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static TValue? Deserialize<TValue>(Stream utf8Json, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static TValue? Deserialize<TValue>(Stream utf8Json, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static TValue? Deserialize<TValue>(ReadOnlySpan<byte> utf8Json, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static TValue? Deserialize<TValue>(ReadOnlySpan<byte> utf8Json, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static TValue? Deserialize<TValue>([StringSyntax("Json")] ReadOnlySpan<char> json, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static TValue? Deserialize<TValue>([StringSyntax("Json")] ReadOnlySpan<char> json, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static TValue? Deserialize<TValue>([StringSyntax("Json")] string json, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static TValue? Deserialize<TValue>([StringSyntax("Json")] string json, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static TValue? Deserialize<TValue>(this JsonDocument document, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static TValue? Deserialize<TValue>(this JsonDocument document, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static TValue? Deserialize<TValue>(this JsonElement element, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static TValue? Deserialize<TValue>(this JsonElement element, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static TValue? Deserialize<TValue>(this JsonNode? node, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static TValue? Deserialize<TValue>(this JsonNode? node, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static TValue? Deserialize<TValue>(ref Utf8JsonReader reader, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static TValue? Deserialize<TValue>(ref Utf8JsonReader reader, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	public static void Serialize(Stream utf8Json, object? value, JsonTypeInfo jsonTypeInfo)
	{
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static void Serialize(Stream utf8Json, object? value, Type inputType, JsonSerializerOptions? options = null)
	{
	}

	public static void Serialize(Stream utf8Json, object? value, Type inputType, JsonSerializerContext context)
	{
	}

	public static string Serialize(object? value, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static string Serialize(object? value, Type inputType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static string Serialize(object? value, Type inputType, JsonSerializerContext context)
	{
		throw null;
	}

	public static void Serialize(Utf8JsonWriter writer, object? value, JsonTypeInfo jsonTypeInfo)
	{
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static void Serialize(Utf8JsonWriter writer, object? value, Type inputType, JsonSerializerOptions? options = null)
	{
	}

	public static void Serialize(Utf8JsonWriter writer, object? value, Type inputType, JsonSerializerContext context)
	{
	}

	public static Task SerializeAsync(Stream utf8Json, object? value, JsonTypeInfo jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task SerializeAsync(Stream utf8Json, object? value, Type inputType, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task SerializeAsync(Stream utf8Json, object? value, Type inputType, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task SerializeAsync<TValue>(Stream utf8Json, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task SerializeAsync<TValue>(Stream utf8Json, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static JsonDocument SerializeToDocument(object? value, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static JsonDocument SerializeToDocument(object? value, Type inputType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static JsonDocument SerializeToDocument(object? value, Type inputType, JsonSerializerContext context)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static JsonDocument SerializeToDocument<TValue>(TValue value, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static JsonDocument SerializeToDocument<TValue>(TValue value, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	public static JsonElement SerializeToElement(object? value, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static JsonElement SerializeToElement(object? value, Type inputType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static JsonElement SerializeToElement(object? value, Type inputType, JsonSerializerContext context)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static JsonElement SerializeToElement<TValue>(TValue value, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static JsonElement SerializeToElement<TValue>(TValue value, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	public static JsonNode? SerializeToNode(object? value, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static JsonNode? SerializeToNode(object? value, Type inputType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static JsonNode? SerializeToNode(object? value, Type inputType, JsonSerializerContext context)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static JsonNode? SerializeToNode<TValue>(TValue value, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static JsonNode? SerializeToNode<TValue>(TValue value, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	public static byte[] SerializeToUtf8Bytes(object? value, JsonTypeInfo jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static byte[] SerializeToUtf8Bytes(object? value, Type inputType, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static byte[] SerializeToUtf8Bytes(object? value, Type inputType, JsonSerializerContext context)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static byte[] SerializeToUtf8Bytes<TValue>(TValue value, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static byte[] SerializeToUtf8Bytes<TValue>(TValue value, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static void Serialize<TValue>(Stream utf8Json, TValue value, JsonSerializerOptions? options = null)
	{
	}

	public static void Serialize<TValue>(Stream utf8Json, TValue value, JsonTypeInfo<TValue> jsonTypeInfo)
	{
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static void Serialize<TValue>(Utf8JsonWriter writer, TValue value, JsonSerializerOptions? options = null)
	{
	}

	public static void Serialize<TValue>(Utf8JsonWriter writer, TValue value, JsonTypeInfo<TValue> jsonTypeInfo)
	{
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static string Serialize<TValue>(TValue value, JsonTypeInfo<TValue> jsonTypeInfo)
	{
		throw null;
	}
}
