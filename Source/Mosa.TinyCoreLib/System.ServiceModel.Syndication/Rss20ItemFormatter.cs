using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.ServiceModel.Syndication;

[XmlRoot(ElementName = "item", Namespace = "")]
public class Rss20ItemFormatter : SyndicationItemFormatter, IXmlSerializable
{
	protected Type ItemType
	{
		get
		{
			throw null;
		}
	}

	public bool PreserveAttributeExtensions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool PreserveElementExtensions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SerializeExtensionsAsAtom
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string Version
	{
		get
		{
			throw null;
		}
	}

	public Rss20ItemFormatter()
	{
	}

	public Rss20ItemFormatter(SyndicationItem itemToWrite)
	{
	}

	public Rss20ItemFormatter(SyndicationItem itemToWrite, bool serializeExtensionsAsAtom)
	{
	}

	public Rss20ItemFormatter(Type itemTypeToCreate)
	{
	}

	public override bool CanRead(XmlReader reader)
	{
		throw null;
	}

	protected override SyndicationItem CreateItemInstance()
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
[XmlRoot(ElementName = "item", Namespace = "")]
public class Rss20ItemFormatter<TSyndicationItem> : Rss20ItemFormatter, IXmlSerializable where TSyndicationItem : SyndicationItem, new()
{
	public Rss20ItemFormatter()
	{
	}

	public Rss20ItemFormatter(TSyndicationItem itemToWrite)
	{
	}

	public Rss20ItemFormatter(TSyndicationItem itemToWrite, bool serializeExtensionsAsAtom)
	{
	}

	protected override SyndicationItem CreateItemInstance()
	{
		throw null;
	}
}
