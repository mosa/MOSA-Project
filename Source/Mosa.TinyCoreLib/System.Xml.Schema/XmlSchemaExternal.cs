using System.Xml.Serialization;

namespace System.Xml.Schema;

public abstract class XmlSchemaExternal : XmlSchemaObject
{
	[XmlAttribute("id", DataType = "ID")]
	public string? Id
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
	public XmlSchema? Schema
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlAttribute("schemaLocation", DataType = "anyURI")]
	public string? SchemaLocation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlAnyAttribute]
	public XmlAttribute[]? UnhandledAttributes
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
