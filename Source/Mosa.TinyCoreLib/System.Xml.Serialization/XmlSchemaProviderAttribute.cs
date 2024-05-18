namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
public sealed class XmlSchemaProviderAttribute : Attribute
{
	public bool IsAny
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? MethodName
	{
		get
		{
			throw null;
		}
	}

	public XmlSchemaProviderAttribute(string? methodName)
	{
	}
}
