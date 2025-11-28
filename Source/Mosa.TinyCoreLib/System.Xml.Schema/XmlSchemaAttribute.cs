using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAttribute : XmlSchemaAnnotated
{
	[XmlIgnore]
	public XmlSchemaSimpleType? AttributeSchemaType
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("XmlSchemaAttribute.AttributeType has been deprecated. Use the AttributeSchemaType property that returns a strongly typed attribute type instead.")]
	[XmlIgnore]
	public object? AttributeType
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(null)]
	[XmlAttribute("default")]
	public string? DefaultValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	[XmlAttribute("fixed")]
	public string? FixedValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(XmlSchemaForm.None)]
	[XmlAttribute("form")]
	public XmlSchemaForm Form
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlAttribute("name")]
	public string? Name
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
	public XmlQualifiedName QualifiedName
	{
		get
		{
			throw null;
		}
	}

	[XmlAttribute("ref")]
	public XmlQualifiedName RefName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlElement("simpleType")]
	public XmlSchemaSimpleType? SchemaType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlAttribute("type")]
	public XmlQualifiedName SchemaTypeName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(XmlSchemaUse.None)]
	[XmlAttribute("use")]
	public XmlSchemaUse Use
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
