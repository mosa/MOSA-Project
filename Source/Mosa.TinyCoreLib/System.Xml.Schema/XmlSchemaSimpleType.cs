using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaSimpleType : XmlSchemaType
{
	[XmlElement("list", typeof(XmlSchemaSimpleTypeList))]
	[XmlElement("restriction", typeof(XmlSchemaSimpleTypeRestriction))]
	[XmlElement("union", typeof(XmlSchemaSimpleTypeUnion))]
	public XmlSchemaSimpleTypeContent? Content
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
