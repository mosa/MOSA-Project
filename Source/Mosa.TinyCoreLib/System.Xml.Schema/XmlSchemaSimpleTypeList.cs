using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaSimpleTypeList : XmlSchemaSimpleTypeContent
{
	[XmlIgnore]
	public XmlSchemaSimpleType? BaseItemType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	public XmlSchemaSimpleType? ItemType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlAttribute("itemType")]
	public XmlQualifiedName ItemTypeName
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
