using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaComplexType : XmlSchemaType
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

	[XmlIgnore]
	public XmlSchemaObjectTable AttributeUses
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaAnyAttribute? AttributeWildcard
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(XmlSchemaDerivationMethod.None)]
	[XmlAttribute("block")]
	public XmlSchemaDerivationMethod Block
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
	public XmlSchemaDerivationMethod BlockResolved
	{
		get
		{
			throw null;
		}
	}

	[XmlElement("complexContent", typeof(XmlSchemaComplexContent))]
	[XmlElement("simpleContent", typeof(XmlSchemaSimpleContent))]
	public XmlSchemaContentModel? ContentModel
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
	public XmlSchemaContentType ContentType
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaParticle ContentTypeParticle
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(false)]
	[XmlAttribute("abstract")]
	public bool IsAbstract
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[XmlAttribute("mixed")]
	public override bool IsMixed
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
