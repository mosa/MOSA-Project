using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public abstract class XmlSchemaFacet : XmlSchemaAnnotated
{
	[DefaultValue(false)]
	[XmlAttribute("fixed")]
	public virtual bool IsFixed
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlAttribute("value")]
	public string? Value
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
