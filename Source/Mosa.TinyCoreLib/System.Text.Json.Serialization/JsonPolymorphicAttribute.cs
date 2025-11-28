namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public sealed class JsonPolymorphicAttribute : JsonAttribute
{
	public bool IgnoreUnrecognizedTypeDiscriminators
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? TypeDiscriminatorPropertyName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonUnknownDerivedTypeHandling UnknownDerivedTypeHandling
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
