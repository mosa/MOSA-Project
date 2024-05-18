using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaKeyref : XmlSchemaIdentityConstraint
{
	[XmlAttribute("refer")]
	public XmlQualifiedName Refer
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
