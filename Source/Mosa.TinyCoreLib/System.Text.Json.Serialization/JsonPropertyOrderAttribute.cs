namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class JsonPropertyOrderAttribute : JsonAttribute
{
	public int Order
	{
		get
		{
			throw null;
		}
	}

	public JsonPropertyOrderAttribute(int order)
	{
	}
}
