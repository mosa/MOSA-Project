namespace System.Text.Json.Serialization;

public sealed class BinaryDataJsonConverter : JsonConverter<BinaryData>
{
	public override BinaryData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		throw null;
	}

	public override void Write(Utf8JsonWriter writer, BinaryData value, JsonSerializerOptions options)
	{
	}
}
