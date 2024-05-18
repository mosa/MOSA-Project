using System.Text.Json.Serialization.Metadata;

namespace System.Text.Json.Serialization;

public abstract class JsonSerializerContext : IJsonTypeInfoResolver
{
	protected abstract JsonSerializerOptions? GeneratedSerializerOptions { get; }

	public JsonSerializerOptions Options
	{
		get
		{
			throw null;
		}
	}

	protected JsonSerializerContext(JsonSerializerOptions? options)
	{
	}

	public abstract JsonTypeInfo? GetTypeInfo(Type type);

	JsonTypeInfo IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		throw null;
	}
}
