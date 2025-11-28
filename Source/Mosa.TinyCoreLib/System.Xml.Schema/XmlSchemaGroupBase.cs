using System.Xml.Serialization;

namespace System.Xml.Schema;

public abstract class XmlSchemaGroupBase : XmlSchemaParticle
{
	[XmlIgnore]
	public abstract XmlSchemaObjectCollection Items { get; }

	internal XmlSchemaGroupBase()
	{
	}
}
