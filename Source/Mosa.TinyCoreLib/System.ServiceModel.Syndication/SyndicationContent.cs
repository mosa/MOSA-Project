using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace System.ServiceModel.Syndication;

public abstract class SyndicationContent
{
	public Dictionary<XmlQualifiedName, string> AttributeExtensions
	{
		get
		{
			throw null;
		}
	}

	public abstract string Type { get; }

	protected SyndicationContent()
	{
	}

	protected SyndicationContent(SyndicationContent source)
	{
	}

	public abstract SyndicationContent Clone();

	public static TextSyndicationContent CreateHtmlContent(string content)
	{
		throw null;
	}

	public static TextSyndicationContent CreatePlaintextContent(string content)
	{
		throw null;
	}

	public static UrlSyndicationContent CreateUrlContent(Uri url, string mediaType)
	{
		throw null;
	}

	public static TextSyndicationContent CreateXhtmlContent(string content)
	{
		throw null;
	}

	public static XmlSyndicationContent CreateXmlContent(object dataContractObject)
	{
		throw null;
	}

	public static XmlSyndicationContent CreateXmlContent(object dataContractObject, XmlObjectSerializer dataContractSerializer)
	{
		throw null;
	}

	public static XmlSyndicationContent CreateXmlContent(object xmlSerializerObject, XmlSerializer serializer)
	{
		throw null;
	}

	public static XmlSyndicationContent CreateXmlContent(XmlReader xmlReader)
	{
		throw null;
	}

	protected abstract void WriteContentsTo(XmlWriter writer);

	public void WriteTo(XmlWriter writer, string outerElementName, string outerElementNamespace)
	{
	}
}
