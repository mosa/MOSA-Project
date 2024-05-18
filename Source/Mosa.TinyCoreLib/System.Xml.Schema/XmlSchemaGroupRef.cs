using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaGroupRef : XmlSchemaParticle
{
	[XmlIgnore]
	public XmlSchemaGroupBase? Particle
	{
		get
		{
			throw null;
		}
	}

	[XmlAttribute("ref")]
	public XmlQualifiedName RefName
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
