namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class JsonIgnoreAttribute : JsonAttribute
{
	public JsonIgnoreCondition Condition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
