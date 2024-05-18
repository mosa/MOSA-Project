using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace System.ServiceModel.Syndication;

public class SyndicationElementExtension
{
	public string OuterName
	{
		get
		{
			throw null;
		}
	}

	public string OuterNamespace
	{
		get
		{
			throw null;
		}
	}

	public SyndicationElementExtension(object dataContractExtension)
	{
	}

	public SyndicationElementExtension(object dataContractExtension, XmlObjectSerializer dataContractSerializer)
	{
	}

	public SyndicationElementExtension(object xmlSerializerExtension, XmlSerializer serializer)
	{
	}

	public SyndicationElementExtension(string outerName, string outerNamespace, object dataContractExtension)
	{
	}

	public SyndicationElementExtension(string outerName, string outerNamespace, object dataContractExtension, XmlObjectSerializer dataContractSerializer)
	{
	}

	public SyndicationElementExtension(XmlReader xmlReader)
	{
	}

	public TExtension GetObject<TExtension>()
	{
		throw null;
	}

	public TExtension GetObject<TExtension>(XmlObjectSerializer serializer)
	{
		throw null;
	}

	public TExtension GetObject<TExtension>(XmlSerializer serializer)
	{
		throw null;
	}

	public XmlReader GetReader()
	{
		throw null;
	}

	public void WriteTo(XmlWriter writer)
	{
	}
}
