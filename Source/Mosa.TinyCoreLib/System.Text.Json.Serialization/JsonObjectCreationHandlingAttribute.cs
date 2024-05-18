namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false)]
public sealed class JsonObjectCreationHandlingAttribute : JsonAttribute
{
	public JsonObjectCreationHandling Handling
	{
		get
		{
			throw null;
		}
	}

	public JsonObjectCreationHandlingAttribute(JsonObjectCreationHandling handling)
	{
	}
}
