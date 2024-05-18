namespace System.Text.Json.Serialization;

public abstract class JsonConverterFactory : JsonConverter
{
	public sealed override Type? Type
	{
		get
		{
			throw null;
		}
	}

	public abstract JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options);
}
