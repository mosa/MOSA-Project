using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaSimpleTypeRestriction : XmlSchemaSimpleTypeContent
{
	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	public XmlSchemaSimpleType? BaseType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlAttribute("base")]
	public XmlQualifiedName BaseTypeName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlElement("enumeration", typeof(XmlSchemaEnumerationFacet))]
	[XmlElement("fractionDigits", typeof(XmlSchemaFractionDigitsFacet))]
	[XmlElement("length", typeof(XmlSchemaLengthFacet))]
	[XmlElement("maxExclusive", typeof(XmlSchemaMaxExclusiveFacet))]
	[XmlElement("maxInclusive", typeof(XmlSchemaMaxInclusiveFacet))]
	[XmlElement("maxLength", typeof(XmlSchemaMaxLengthFacet))]
	[XmlElement("minExclusive", typeof(XmlSchemaMinExclusiveFacet))]
	[XmlElement("minInclusive", typeof(XmlSchemaMinInclusiveFacet))]
	[XmlElement("minLength", typeof(XmlSchemaMinLengthFacet))]
	[XmlElement("pattern", typeof(XmlSchemaPatternFacet))]
	[XmlElement("totalDigits", typeof(XmlSchemaTotalDigitsFacet))]
	[XmlElement("whiteSpace", typeof(XmlSchemaWhiteSpaceFacet))]
	public XmlSchemaObjectCollection Facets
	{
		get
		{
			throw null;
		}
	}
}
