using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json.Serialization;

[RequiresDynamicCode("JsonStringEnumConverter cannot be statically analyzed and requires runtime code generation. Applications should use the generic JsonStringEnumConverter<TEnum> instead.")]
public class JsonStringEnumConverter : JsonConverterFactory
{
	public JsonStringEnumConverter()
	{
	}

	public JsonStringEnumConverter(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true)
	{
	}

	public sealed override bool CanConvert(Type typeToConvert)
	{
		throw null;
	}

	public sealed override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		throw null;
	}
}
public class JsonStringEnumConverter<TEnum> : JsonConverterFactory where TEnum : struct, Enum
{
	public JsonStringEnumConverter()
	{
	}

	public JsonStringEnumConverter(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true)
	{
	}

	public sealed override bool CanConvert(Type typeToConvert)
	{
		throw null;
	}

	public sealed override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		throw null;
	}
}
