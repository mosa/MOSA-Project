using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAll : XmlSchemaGroupBase
{
	[XmlElement("element", typeof(XmlSchemaElement))]
	public override XmlSchemaObjectCollection Items
	{
		get
		{
			throw null;
		}
	}
}
