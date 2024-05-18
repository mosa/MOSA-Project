using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public sealed class SqlXml : INullable, IXmlSerializable
{
	public bool IsNull
	{
		get
		{
			throw null;
		}
	}

	public static SqlXml Null
	{
		get
		{
			throw null;
		}
	}

	public string Value
	{
		get
		{
			throw null;
		}
	}

	public SqlXml()
	{
	}

	public SqlXml(Stream? value)
	{
	}

	public SqlXml(XmlReader? value)
	{
	}

	public XmlReader CreateReader()
	{
		throw null;
	}

	public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
	{
		throw null;
	}

	XmlSchema? IXmlSerializable.GetSchema()
	{
		throw null;
	}

	void IXmlSerializable.ReadXml(XmlReader r)
	{
	}

	void IXmlSerializable.WriteXml(XmlWriter writer)
	{
	}
}
