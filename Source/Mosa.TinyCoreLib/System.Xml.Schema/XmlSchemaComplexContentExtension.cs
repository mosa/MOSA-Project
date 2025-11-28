using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaComplexContentExtension : XmlSchemaContent
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

	[XmlElement("all", typeof(XmlSchemaAll))]
	[XmlElement("choice", typeof(XmlSchemaChoice))]
	[XmlElement("group", typeof(XmlSchemaGroupRef))]
	[XmlElement("sequence", typeof(XmlSchemaSequence))]
	public XmlSchemaParticle? Particle
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
