namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
public class JsonDerivedTypeAttribute : JsonAttribute
{
	public Type DerivedType
	{
		get
		{
			throw null;
		}
	}

	public object? TypeDiscriminator
	{
		get
		{
			throw null;
		}
	}

	public JsonDerivedTypeAttribute(Type derivedType)
	{
	}

	public JsonDerivedTypeAttribute(Type derivedType, int typeDiscriminator)
	{
	}

	public JsonDerivedTypeAttribute(Type derivedType, string typeDiscriminator)
	{
	}
}
