using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAttributeGroup : XmlSchemaAnnotated
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

	[XmlAttribute("name")]
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

	[XmlIgnore]
	public XmlQualifiedName QualifiedName
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaAttributeGroup? RedefinedAttributeGroup
	{
		get
		{
			throw null;
		}
	}
}
