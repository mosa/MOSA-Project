using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.ServiceModel.Syndication;

[XmlRoot(ElementName = "service", Namespace = "http://www.w3.org/2007/app")]
public class AtomPub10ServiceDocumentFormatter : ServiceDocumentFormatter, IXmlSerializable
{
	public override string Version
	{
		get
		{
			throw null;
		}
	}

	public AtomPub10ServiceDocumentFormatter()
	{
	}

	public AtomPub10ServiceDocumentFormatter(ServiceDocument documentToWrite)
	{
	}

	public AtomPub10ServiceDocumentFormatter(Type documentTypeToCreate)
	{
	}

	public override bool CanRead(XmlReader reader)
	{
		throw null;
	}

	protected override ServiceDocument CreateDocumentInstance()
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
[XmlRoot(ElementName = "service", Namespace = "http://www.w3.org/2007/app")]
public class AtomPub10ServiceDocumentFormatter<TServiceDocument> : AtomPub10ServiceDocumentFormatter where TServiceDocument : ServiceDocument, new()
{
	public AtomPub10ServiceDocumentFormatter()
	{
	}

	public AtomPub10ServiceDocumentFormatter(TServiceDocument documentToWrite)
	{
	}

	protected override ServiceDocument CreateDocumentInstance()
	{
		throw null;
	}
}
