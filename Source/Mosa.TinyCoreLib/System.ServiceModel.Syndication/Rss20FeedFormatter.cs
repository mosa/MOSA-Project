using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.ServiceModel.Syndication;

[XmlRoot(ElementName = "rss", Namespace = "")]
public class Rss20FeedFormatter : SyndicationFeedFormatter, IXmlSerializable
{
	protected Type FeedType
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

	public Rss20FeedFormatter()
	{
	}

	public Rss20FeedFormatter(SyndicationFeed feedToWrite)
	{
	}

	public Rss20FeedFormatter(SyndicationFeed feedToWrite, bool serializeExtensionsAsAtom)
	{
	}

	public Rss20FeedFormatter(Type feedTypeToCreate)
	{
	}

	public override bool CanRead(XmlReader reader)
	{
		throw null;
	}

	protected override SyndicationFeed CreateFeedInstance()
	{
		throw null;
	}

	public override void ReadFrom(XmlReader reader)
	{
	}

	protected virtual SyndicationItem ReadItem(XmlReader reader, SyndicationFeed feed)
	{
		throw null;
	}

	protected virtual IEnumerable<SyndicationItem> ReadItems(XmlReader reader, SyndicationFeed feed, out bool areAllItemsRead)
	{
		throw null;
	}

	protected internal override void SetFeed(SyndicationFeed feed)
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

	protected virtual void WriteItem(XmlWriter writer, SyndicationItem item, Uri feedBaseUri)
	{
	}

	protected virtual void WriteItems(XmlWriter writer, IEnumerable<SyndicationItem> items, Uri feedBaseUri)
	{
	}

	public override void WriteTo(XmlWriter writer)
	{
	}
}
[XmlRoot(ElementName = "rss", Namespace = "")]
public class Rss20FeedFormatter<TSyndicationFeed> : Rss20FeedFormatter where TSyndicationFeed : SyndicationFeed, new()
{
	public Rss20FeedFormatter()
	{
	}

	public Rss20FeedFormatter(TSyndicationFeed feedToWrite)
	{
	}

	public Rss20FeedFormatter(TSyndicationFeed feedToWrite, bool serializeExtensionsAsAtom)
	{
	}

	protected override SyndicationFeed CreateFeedInstance()
	{
		throw null;
	}
}
