namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true)]
public class SoapIncludeAttribute : Attribute
{
	public Type Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SoapIncludeAttribute(Type type)
	{
	}
}
