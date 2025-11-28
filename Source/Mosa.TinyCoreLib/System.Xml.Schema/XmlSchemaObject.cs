using System.Xml.Serialization;

namespace System.Xml.Schema;

public abstract class XmlSchemaObject
{
	[XmlIgnore]
	public int LineNumber
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
	public int LinePosition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlNamespaceDeclarations]
	public XmlSerializerNamespaces Namespaces
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
	public XmlSchemaObject? Parent
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
	public string? SourceUri
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
