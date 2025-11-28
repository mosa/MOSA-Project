namespace System.Text.Json.Serialization;

public sealed class JsonNumberEnumConverter<TEnum> : JsonConverterFactory where TEnum : struct
{
	public override bool CanConvert(Type typeToConvert)
	{
		throw null;
	}

	public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		throw null;
	}
}
