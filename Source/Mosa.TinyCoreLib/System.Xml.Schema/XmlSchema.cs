using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

namespace System.Xml.Schema;

[XmlRoot("schema", Namespace = "http://www.w3.org/2001/XMLSchema")]
public class XmlSchema : XmlSchemaObject
{
	public const string InstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";

	public const string Namespace = "http://www.w3.org/2001/XMLSchema";

	[DefaultValue(XmlSchemaForm.None)]
	[XmlAttribute("attributeFormDefault")]
	public XmlSchemaForm AttributeFormDefault
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
	public XmlSchemaObjectTable AttributeGroups
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaObjectTable Attributes
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(XmlSchemaDerivationMethod.None)]
	[XmlAttribute("blockDefault")]
	public XmlSchemaDerivationMethod BlockDefault
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
	[XmlAttribute("elementFormDefault")]
	public XmlSchemaForm ElementFormDefault
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
	public XmlSchemaObjectTable Elements
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(XmlSchemaDerivationMethod.None)]
	[XmlAttribute("finalDefault")]
	public XmlSchemaDerivationMethod FinalDefault
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
	public XmlSchemaObjectTable Groups
	{
		get
		{
			throw null;
		}
	}

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

	[XmlElement("import", typeof(XmlSchemaImport))]
	[XmlElement("include", typeof(XmlSchemaInclude))]
	[XmlElement("redefine", typeof(XmlSchemaRedefine))]
	public XmlSchemaObjectCollection Includes
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public bool IsCompiled
	{
		get
		{
			throw null;
		}
	}

	[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
	[XmlElement("attribute", typeof(XmlSchemaAttribute))]
	[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroup))]
	[XmlElement("complexType", typeof(XmlSchemaComplexType))]
	[XmlElement("element", typeof(XmlSchemaElement))]
	[XmlElement("group", typeof(XmlSchemaGroup))]
	[XmlElement("notation", typeof(XmlSchemaNotation))]
	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	public XmlSchemaObjectCollection Items
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaObjectTable Notations
	{
		get
		{
			throw null;
		}
	}

	[XmlIgnore]
	public XmlSchemaObjectTable SchemaTypes
	{
		get
		{
			throw null;
		}
	}

	[XmlAttribute("targetNamespace", DataType = "anyURI")]
	public string? TargetNamespace
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

	[XmlAttribute("version", DataType = "token")]
	public string? Version
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("XmlSchema.Compile has been deprecated. Use System.Xml.Schema.XmlSchemaSet for schema compilation and validation.")]
	public void Compile(ValidationEventHandler? validationEventHandler)
	{
	}

	[Obsolete("XmlSchema.Compile has been deprecated. Use System.Xml.Schema.XmlSchemaSet for schema compilation and validation.")]
	public void Compile(ValidationEventHandler? validationEventHandler, XmlResolver? resolver)
	{
	}

	public static XmlSchema? Read(Stream stream, ValidationEventHandler? validationEventHandler)
	{
		throw null;
	}

	public static XmlSchema? Read(TextReader reader, ValidationEventHandler? validationEventHandler)
	{
		throw null;
	}

	public static XmlSchema? Read(XmlReader reader, ValidationEventHandler? validationEventHandler)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Write(Stream stream)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Write(Stream stream, XmlNamespaceManager? namespaceManager)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Write(TextWriter writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Write(TextWriter writer, XmlNamespaceManager? namespaceManager)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Write(XmlWriter writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Write(XmlWriter writer, XmlNamespaceManager? namespaceManager)
	{
	}
}
