using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace System.ServiceModel.Syndication;

public class XmlSyndicationContent : SyndicationContent
{
	public SyndicationElementExtension Extension
	{
		get
		{
			throw null;
		}
	}

	public override string Type
	{
		get
		{
			throw null;
		}
	}

	protected XmlSyndicationContent(XmlSyndicationContent source)
	{
	}

	public XmlSyndicationContent(string type, object dataContractExtension, XmlObjectSerializer dataContractSerializer)
	{
	}

	public XmlSyndicationContent(string type, object xmlSerializerExtension, XmlSerializer serializer)
	{
	}

	public XmlSyndicationContent(string type, SyndicationElementExtension extension)
	{
	}

	public XmlSyndicationContent(XmlReader reader)
	{
	}

	public override SyndicationContent Clone()
	{
		throw null;
	}

	public XmlDictionaryReader GetReaderAtContent()
	{
		throw null;
	}

	public TContent ReadContent<TContent>()
	{
		throw null;
	}

	public TContent ReadContent<TContent>(XmlObjectSerializer dataContractSerializer)
	{
		throw null;
	}

	public TContent ReadContent<TContent>(XmlSerializer serializer)
	{
		throw null;
	}

	protected override void WriteContentsTo(XmlWriter writer)
	{
	}
}
