using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaElement : XmlSchemaParticle
{
	[DefaultValue(XmlSchemaDerivationMethod.None)]
	[XmlAttribute("block")]
	public XmlSchemaDerivationMethod Block
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
	public XmlSchemaDerivationMethod BlockResolved
	{
		get
		{
			throw null;
		}
	}

	[XmlElement("key", typeof(XmlSchemaKey))]
	[XmlElement("keyref", typeof(XmlSchemaKeyref))]
	[XmlElement("unique", typeof(XmlSchemaUnique))]
	public XmlSchemaObjectCollection Constraints
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

	[XmlIgnore]
	public XmlSchemaType? ElementSchemaType
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("XmlSchemaElement.ElementType has been deprecated. Use the ElementSchemaType property that returns a strongly typed element type instead.")]
	[XmlIgnore]
	public object? ElementType
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(XmlSchemaDerivationMethod.None)]
	[XmlAttribute("final")]
	public XmlSchemaDerivationMethod Final
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
	public XmlSchemaDerivationMethod FinalResolved
	{
		get
		{
			throw null;
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

	[DefaultValue(false)]
	[XmlAttribute("abstract")]
	public bool IsAbstract
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[XmlAttribute("nillable")]
	public bool IsNillable
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue("")]
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
		[param: AllowNull]
		set
		{
		}
	}

	[XmlElement("complexType", typeof(XmlSchemaComplexType))]
	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	public XmlSchemaType? SchemaType
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
		[param: AllowNull]
		set
		{
		}
	}

	[XmlAttribute("substitutionGroup")]
	public XmlQualifiedName SubstitutionGroup
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
