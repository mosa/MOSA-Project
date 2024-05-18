using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaXPath : XmlSchemaAnnotated
{
	[DefaultValue("")]
	[XmlAttribute("xpath")]
	public string? XPath
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
