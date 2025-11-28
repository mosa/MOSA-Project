using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaRedefine : XmlSchemaExternal
{
	[XmlIgnore]
	public XmlSchemaObjectTable AttributeGroups
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaObjectTable Groups
	{
		get
		{
			throw null;
		}
	}

	[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
	[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroup))]
	[XmlElement("complexType", typeof(XmlSchemaComplexType))]
	[XmlElement("group", typeof(XmlSchemaGroup))]
	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	public XmlSchemaObjectCollection Items
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaObjectTable SchemaTypes
	{
		get
		{
			throw null;
		}
	}
}
