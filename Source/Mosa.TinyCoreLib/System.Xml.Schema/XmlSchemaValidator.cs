using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Schema;

public sealed class XmlSchemaValidator
{
	public IXmlLineInfo LineInfoProvider
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

	public Uri? SourceUri
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public object ValidationEventSender
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlResolver? XmlResolver
	{
		set
		{
		}
	}

	public event ValidationEventHandler? ValidationEventHandler
	{
		add
		{
		}
		remove
		{
		}
	}

	public XmlSchemaValidator(XmlNameTable nameTable, XmlSchemaSet schemas, IXmlNamespaceResolver namespaceResolver, XmlSchemaValidationFlags validationFlags)
	{
	}

	public void AddSchema(XmlSchema schema)
	{
	}

	public void EndValidation()
	{
	}

	public XmlSchemaAttribute[] GetExpectedAttributes()
	{
		throw null;
	}

	public XmlSchemaParticle[] GetExpectedParticles()
	{
		throw null;
	}

	public void GetUnspecifiedDefaultAttributes(ArrayList defaultAttributes)
	{
	}

	public void Initialize()
	{
	}

	public void Initialize(XmlSchemaObject partialValidationType)
	{
	}

	public void SkipToEndElement(XmlSchemaInfo? schemaInfo)
	{
	}

	public object? ValidateAttribute(string localName, string namespaceUri, string attributeValue, XmlSchemaInfo? schemaInfo)
	{
		throw null;
	}

	public object? ValidateAttribute(string localName, string namespaceUri, XmlValueGetter attributeValue, XmlSchemaInfo? schemaInfo)
	{
		throw null;
	}

	public void ValidateElement(string localName, string namespaceUri, XmlSchemaInfo? schemaInfo)
	{
	}

	public void ValidateElement(string localName, string namespaceUri, XmlSchemaInfo? schemaInfo, string? xsiType, string? xsiNil, string? xsiSchemaLocation, string? xsiNoNamespaceSchemaLocation)
	{
	}

	public object? ValidateEndElement(XmlSchemaInfo? schemaInfo)
	{
		throw null;
	}

	public object? ValidateEndElement(XmlSchemaInfo? schemaInfo, object typedValue)
	{
		throw null;
	}

	public void ValidateEndOfAttributes(XmlSchemaInfo? schemaInfo)
	{
	}

	public void ValidateText(string elementValue)
	{
	}

	public void ValidateText(XmlValueGetter elementValue)
	{
	}

	public void ValidateWhitespace(string elementValue)
	{
	}

	public void ValidateWhitespace(XmlValueGetter elementValue)
	{
	}
}
