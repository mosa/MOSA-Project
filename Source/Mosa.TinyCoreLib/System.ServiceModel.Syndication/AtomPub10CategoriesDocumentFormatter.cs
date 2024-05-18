using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.ServiceModel.Syndication;

[XmlRoot(ElementName = "categories", Namespace = "http://www.w3.org/2007/app")]
public class AtomPub10CategoriesDocumentFormatter : CategoriesDocumentFormatter, IXmlSerializable
{
	public override string Version
	{
		get
		{
			throw null;
		}
	}

	public AtomPub10CategoriesDocumentFormatter()
	{
	}

	public AtomPub10CategoriesDocumentFormatter(CategoriesDocument documentToWrite)
	{
	}

	public AtomPub10CategoriesDocumentFormatter(Type inlineDocumentType, Type referencedDocumentType)
	{
	}

	public override bool CanRead(XmlReader reader)
	{
		throw null;
	}

	protected override InlineCategoriesDocument CreateInlineCategoriesDocument()
	{
		throw null;
	}

	protected override ReferencedCategoriesDocument CreateReferencedCategoriesDocument()
	{
		throw null;
	}

	public override void ReadFrom(XmlReader reader)
	{
	}

	XmlSchema IXmlSerializable.GetSchema()
	{
		throw null;
	}

	void IXmlSerializable.ReadXml(XmlReader reader)
	{
	}

	void IXmlSerializable.WriteXml(XmlWriter writer)
	{
	}

	public override void WriteTo(XmlWriter writer)
	{
	}
}
