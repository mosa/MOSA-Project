namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class JsonUnmappedMemberHandlingAttribute : JsonAttribute
{
	public JsonUnmappedMemberHandling UnmappedMemberHandling
	{
		get
		{
			throw null;
		}
	}

	public JsonUnmappedMemberHandlingAttribute(JsonUnmappedMemberHandling unmappedMemberHandling)
	{
	}
}
