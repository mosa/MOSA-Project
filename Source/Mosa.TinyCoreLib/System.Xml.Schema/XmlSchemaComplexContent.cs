using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaComplexContent : XmlSchemaContentModel
{
	[XmlElement("extension", typeof(XmlSchemaComplexContentExtension))]
	[XmlElement("restriction", typeof(XmlSchemaComplexContentRestriction))]
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

	[XmlAttribute("mixed")]
	public bool IsMixed
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
