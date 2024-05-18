using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false)]
public class JsonConverterAttribute : JsonAttribute
{
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
	public Type? ConverterType
	{
		get
		{
			throw null;
		}
	}

	protected JsonConverterAttribute()
	{
	}

	public JsonConverterAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type converterType)
	{
	}

	public virtual JsonConverter? CreateConverter(Type typeToConvert)
	{
		throw null;
	}
}
