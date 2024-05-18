using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.ServiceModel.Syndication;

[XmlRoot(ElementName = "feed", Namespace = "http://www.w3.org/2005/Atom")]
public class Atom10FeedFormatter : SyndicationFeedFormatter, IXmlSerializable
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

	public override string Version
	{
		get
		{
			throw null;
		}
	}

	public Atom10FeedFormatter()
	{
	}

	public Atom10FeedFormatter(SyndicationFeed feedToWrite)
	{
	}

	public Atom10FeedFormatter(Type feedTypeToCreate)
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
[XmlRoot(ElementName = "feed", Namespace = "http://www.w3.org/2005/Atom")]
public class Atom10FeedFormatter<TSyndicationFeed> : Atom10FeedFormatter where TSyndicationFeed : SyndicationFeed, new()
{
	public Atom10FeedFormatter()
	{
	}

	public Atom10FeedFormatter(TSyndicationFeed feedToWrite)
	{
	}

	protected override SyndicationFeed CreateFeedInstance()
	{
		throw null;
	}
}
