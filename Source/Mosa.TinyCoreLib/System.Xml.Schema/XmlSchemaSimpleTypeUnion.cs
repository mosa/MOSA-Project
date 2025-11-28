using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaSimpleTypeUnion : XmlSchemaSimpleTypeContent
{
	[XmlIgnore]
	public XmlSchemaSimpleType[]? BaseMemberTypes
	{
		get
		{
			throw null;
		}
	}

	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	public XmlSchemaObjectCollection BaseTypes
	{
		get
		{
			throw null;
		}
	}

	[XmlAttribute("memberTypes")]
	public XmlQualifiedName[]? MemberTypes
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
