namespace System.Text.Json.Serialization.Metadata;

public interface IJsonTypeInfoResolver
{
	JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options);
}
