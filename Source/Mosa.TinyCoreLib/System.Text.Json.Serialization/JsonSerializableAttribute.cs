namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class JsonSerializableAttribute : JsonAttribute
{
	public JsonSourceGenerationMode GenerationMode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? TypeInfoPropertyName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonSerializableAttribute(Type type)
	{
	}
}
