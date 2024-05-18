using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaType : XmlSchemaAnnotated
{
	[Obsolete("XmlSchemaType.BaseSchemaType has been deprecated. Use the BaseXmlSchemaType property that returns a strongly typed base schema type instead.")]
	[XmlIgnore]
	public object? BaseSchemaType
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaType? BaseXmlSchemaType
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaDatatype? Datatype
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaDerivationMethod DerivedBy
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

	[XmlIgnore]
	public virtual bool IsMixed
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

	[XmlIgnore]
	public XmlTypeCode TypeCode
	{
		get
		{
			throw null;
		}
	}

	public static XmlSchemaComplexType? GetBuiltInComplexType(XmlTypeCode typeCode)
	{
		throw null;
	}

	public static XmlSchemaComplexType? GetBuiltInComplexType(XmlQualifiedName qualifiedName)
	{
		throw null;
	}

	public static XmlSchemaSimpleType GetBuiltInSimpleType(XmlTypeCode typeCode)
	{
		throw null;
	}

	public static XmlSchemaSimpleType? GetBuiltInSimpleType(XmlQualifiedName qualifiedName)
	{
		throw null;
	}

	public static bool IsDerivedFrom([NotNullWhen(true)] XmlSchemaType? derivedType, [NotNullWhen(true)] XmlSchemaType? baseType, XmlSchemaDerivationMethod except)
	{
		throw null;
	}
}
