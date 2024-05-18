using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Json;

public sealed class JsonContent : HttpContent
{
	public Type ObjectType
	{
		get
		{
			throw null;
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
	}

	internal JsonContent()
	{
	}

	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	public static JsonContent Create(object? inputValue, Type inputType, MediaTypeHeaderValue? mediaType = null, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
	public static JsonContent Create<T>(T inputValue, MediaTypeHeaderValue? mediaType = null, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static JsonContent Create<T>(T? inputValue, JsonTypeInfo<T> jsonTypeInfo, MediaTypeHeaderValue? mediaType = null)
	{
		throw null;
	}

	public static JsonContent Create(object? inputValue, JsonTypeInfo jsonTypeInfo, MediaTypeHeaderValue? mediaType = null)
	{
		throw null;
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
	{
		throw null;
	}

	protected internal override bool TryComputeLength(out long length)
	{
		throw null;
	}

	protected override void SerializeToStream(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
		throw null;
	}
}
