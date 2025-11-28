using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaChoice : XmlSchemaGroupBase
{
	[XmlElement("any", typeof(XmlSchemaAny))]
	[XmlElement("choice", typeof(XmlSchemaChoice))]
	[XmlElement("element", typeof(XmlSchemaElement))]
	[XmlElement("group", typeof(XmlSchemaGroupRef))]
	[XmlElement("sequence", typeof(XmlSchemaSequence))]
	public override XmlSchemaObjectCollection Items
	{
		get
		{
			throw null;
		}
	}
}
