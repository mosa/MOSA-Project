namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Field)]
public class XmlEnumAttribute : Attribute
{
	public string? Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlEnumAttribute()
	{
	}

	public XmlEnumAttribute(string? name)
	{
	}
}
