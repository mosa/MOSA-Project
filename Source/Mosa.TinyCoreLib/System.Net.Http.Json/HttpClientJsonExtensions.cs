using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Json;

public static class HttpClientJsonExtensions
{
	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<object?> DeleteFromJsonAsync(this HttpClient client, [StringSyntax("Uri")] string? requestUri, Type type, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<object?> DeleteFromJsonAsync(this HttpClient client, [StringSyntax("Uri")] string? requestUri, Type type, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<object?> DeleteFromJsonAsync(this HttpClient client, [StringSyntax("Uri")] string? requestUri, Type type, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<object?> DeleteFromJsonAsync(this HttpClient client, Uri? requestUri, Type type, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<object?> DeleteFromJsonAsync(this HttpClient client, Uri? requestUri, Type type, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<object?> DeleteFromJsonAsync(this HttpClient client, Uri? requestUri, Type type, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<TValue?> DeleteFromJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<TValue?> DeleteFromJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<TValue?> DeleteFromJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<TValue?> DeleteFromJsonAsync<TValue>(this HttpClient client, Uri? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<TValue?> DeleteFromJsonAsync<TValue>(this HttpClient client, Uri? requestUri, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<TValue?> DeleteFromJsonAsync<TValue>(this HttpClient client, Uri? requestUri, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static IAsyncEnumerable<TValue?> GetFromJsonAsAsyncEnumerable<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static IAsyncEnumerable<TValue?> GetFromJsonAsAsyncEnumerable<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static IAsyncEnumerable<TValue?> GetFromJsonAsAsyncEnumerable<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static IAsyncEnumerable<TValue?> GetFromJsonAsAsyncEnumerable<TValue>(this HttpClient client, Uri? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static IAsyncEnumerable<TValue?> GetFromJsonAsAsyncEnumerable<TValue>(this HttpClient client, Uri? requestUri, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static IAsyncEnumerable<TValue?> GetFromJsonAsAsyncEnumerable<TValue>(this HttpClient client, Uri? requestUri, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<object?> GetFromJsonAsync(this HttpClient client, [StringSyntax("Uri")] string? requestUri, Type type, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<object?> GetFromJsonAsync(this HttpClient client, [StringSyntax("Uri")] string? requestUri, Type type, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<object?> GetFromJsonAsync(this HttpClient client, [StringSyntax("Uri")] string? requestUri, Type type, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<object?> GetFromJsonAsync(this HttpClient client, Uri? requestUri, Type type, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<object?> GetFromJsonAsync(this HttpClient client, Uri? requestUri, Type type, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<object?> GetFromJsonAsync(this HttpClient client, Uri? requestUri, Type type, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, Uri? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, Uri? requestUri, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, Uri? requestUri, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TValue value, CancellationToken cancellationToken)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, CancellationToken cancellationToken)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TValue value, CancellationToken cancellationToken)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, CancellationToken cancellationToken)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TValue value, CancellationToken cancellationToken)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, CancellationToken cancellationToken)
	{
		throw null;
	}
}
