using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Json;

public static class HttpContentJsonExtensions
{
	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static IAsyncEnumerable<TValue?> ReadFromJsonAsAsyncEnumerable<TValue>(this HttpContent content, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static IAsyncEnumerable<TValue?> ReadFromJsonAsAsyncEnumerable<TValue>(this HttpContent content, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static IAsyncEnumerable<TValue?> ReadFromJsonAsAsyncEnumerable<TValue>(this HttpContent content, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<object?> ReadFromJsonAsync(this HttpContent content, Type type, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<object?> ReadFromJsonAsync(this HttpContent content, Type type, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<object?> ReadFromJsonAsync(this HttpContent content, Type type, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<T?> ReadFromJsonAsync<T>(this HttpContent content, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<T?> ReadFromJsonAsync<T>(this HttpContent content, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<T?> ReadFromJsonAsync<T>(this HttpContent content, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
