using System.Xml.Serialization;

namespace System.Xml.Schema;

public abstract class XmlSchemaParticle : XmlSchemaAnnotated
{
	[XmlIgnore]
	public decimal MaxOccurs
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlAttribute("maxOccurs")]
	public string? MaxOccursString
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlIgnore]
	public decimal MinOccurs
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlAttribute("minOccurs")]
	public string? MinOccursString
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
