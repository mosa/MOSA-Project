namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class JsonNumberHandlingAttribute : JsonAttribute
{
	public JsonNumberHandling Handling
	{
		get
		{
			throw null;
		}
	}

	public JsonNumberHandlingAttribute(JsonNumberHandling handling)
	{
	}
}
