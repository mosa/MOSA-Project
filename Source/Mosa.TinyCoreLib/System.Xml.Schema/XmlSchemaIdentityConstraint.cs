using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaIdentityConstraint : XmlSchemaAnnotated
{
	[XmlElement("field", typeof(XmlSchemaXPath))]
	public XmlSchemaObjectCollection Fields
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

	[XmlElement("selector", typeof(XmlSchemaXPath))]
	public XmlSchemaXPath? Selector
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
