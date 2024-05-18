using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaSimpleContentExtension : XmlSchemaContent
{
	[XmlElement("anyAttribute")]
	public XmlSchemaAnyAttribute? AnyAttribute
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlElement("attribute", typeof(XmlSchemaAttribute))]
	[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
	public XmlSchemaObjectCollection Attributes
	{
		get
		{
			throw null;
		}
	}

	[XmlAttribute("base")]
	public XmlQualifiedName BaseTypeName
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
