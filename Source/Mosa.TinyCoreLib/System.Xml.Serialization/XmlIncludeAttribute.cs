namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true)]
public class XmlIncludeAttribute : Attribute
{
	public Type? Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlIncludeAttribute(Type? type)
	{
	}
}
