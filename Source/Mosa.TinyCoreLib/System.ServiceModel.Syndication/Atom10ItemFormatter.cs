using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.ServiceModel.Syndication;

[XmlRoot(ElementName = "entry", Namespace = "http://www.w3.org/2005/Atom")]
public class Atom10ItemFormatter : SyndicationItemFormatter, IXmlSerializable
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

	public override string Version
	{
		get
		{
			throw null;
		}
	}

	public Atom10ItemFormatter()
	{
	}

	public Atom10ItemFormatter(SyndicationItem itemToWrite)
	{
	}

	public Atom10ItemFormatter(Type itemTypeToCreate)
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
[XmlRoot(ElementName = "entry", Namespace = "http://www.w3.org/2005/Atom")]
public class Atom10ItemFormatter<TSyndicationItem> : Atom10ItemFormatter where TSyndicationItem : SyndicationItem, new()
{
	public Atom10ItemFormatter()
	{
	}

	public Atom10ItemFormatter(TSyndicationItem itemToWrite)
	{
	}

	protected override SyndicationItem CreateItemInstance()
	{
		throw null;
	}
}
