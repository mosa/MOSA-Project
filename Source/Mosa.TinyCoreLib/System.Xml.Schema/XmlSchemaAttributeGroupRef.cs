using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAttributeGroupRef : XmlSchemaAnnotated
{
	[XmlAttribute("ref")]
	public XmlQualifiedName RefName
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}
}
