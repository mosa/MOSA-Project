using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAny : XmlSchemaParticle
{
	[XmlAttribute("namespace")]
	public string? Namespace
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(XmlSchemaContentProcessing.None)]
	[XmlAttribute("processContents")]
	public XmlSchemaContentProcessing ProcessContents
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
