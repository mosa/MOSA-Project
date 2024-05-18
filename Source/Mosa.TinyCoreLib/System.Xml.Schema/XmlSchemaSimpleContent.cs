using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaSimpleContent : XmlSchemaContentModel
{
	[XmlElement("extension", typeof(XmlSchemaSimpleContentExtension))]
	[XmlElement("restriction", typeof(XmlSchemaSimpleContentRestriction))]
	public override XmlSchemaContent? Content
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
