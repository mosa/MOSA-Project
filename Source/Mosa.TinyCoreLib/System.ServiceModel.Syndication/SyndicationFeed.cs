using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.ServiceModel.Syndication;

public class SyndicationFeed
{
	public SyndicationLink Documentation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Collection<string> SkipDays
	{
		get
		{
			throw null;
		}
	}

	public Collection<int> SkipHours
	{
		get
		{
			throw null;
		}
	}

	public SyndicationTextInput TextInput
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan? TimeToLive
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Dictionary<XmlQualifiedName, string> AttributeExtensions
	{
		get
		{
			throw null;
		}
	}

	public Collection<SyndicationPerson> Authors
	{
		get
		{
			throw null;
		}
	}

	public Uri BaseUri
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Collection<SyndicationCategory> Categories
	{
		get
		{
			throw null;
		}
	}

	public Collection<SyndicationPerson> Contributors
	{
		get
		{
			throw null;
		}
	}

	public TextSyndicationContent Copyright
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TextSyndicationContent Description
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SyndicationElementExtensionCollection ElementExtensions
	{
		get
		{
			throw null;
		}
	}

	public string Generator
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Id
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Uri ImageUrl
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IEnumerable<SyndicationItem> Items
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Language
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTimeOffset LastUpdatedTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Collection<SyndicationLink> Links
	{
		get
		{
			throw null;
		}
	}

	public TextSyndicationContent Title
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SyndicationFeed()
	{
	}

	public SyndicationFeed(IEnumerable<SyndicationItem> items)
	{
	}

	protected SyndicationFeed(SyndicationFeed source, bool cloneItems)
	{
	}

	public SyndicationFeed(string title, string description, Uri feedAlternateLink)
	{
	}

	public SyndicationFeed(string title, string description, Uri feedAlternateLink, IEnumerable<SyndicationItem> items)
	{
	}

	public SyndicationFeed(string title, string description, Uri feedAlternateLink, string id, DateTimeOffset lastUpdatedTime)
	{
	}

	public SyndicationFeed(string title, string description, Uri feedAlternateLink, string id, DateTimeOffset lastUpdatedTime, IEnumerable<SyndicationItem> items)
	{
	}

	public virtual SyndicationFeed Clone(bool cloneItems)
	{
		throw null;
	}

	protected internal virtual SyndicationCategory CreateCategory()
	{
		throw null;
	}

	protected internal virtual SyndicationItem CreateItem()
	{
		throw null;
	}

	protected internal virtual SyndicationLink CreateLink()
	{
		throw null;
	}

	protected internal virtual SyndicationPerson CreatePerson()
	{
		throw null;
	}

	public Atom10FeedFormatter GetAtom10Formatter()
	{
		throw null;
	}

	public Rss20FeedFormatter GetRss20Formatter()
	{
		throw null;
	}

	public Rss20FeedFormatter GetRss20Formatter(bool serializeExtensionsAsAtom)
	{
		throw null;
	}

	public static SyndicationFeed Load(XmlReader reader)
	{
		throw null;
	}

	public static TSyndicationFeed Load<TSyndicationFeed>(XmlReader reader) where TSyndicationFeed : SyndicationFeed, new()
	{
		throw null;
	}

	public void SaveAsAtom10(XmlWriter writer)
	{
	}

	public void SaveAsRss20(XmlWriter writer)
	{
	}

	protected internal virtual bool TryParseAttribute(string name, string ns, string value, string version)
	{
		throw null;
	}

	protected internal virtual bool TryParseElement(XmlReader reader, string version)
	{
		throw null;
	}

	protected internal virtual void WriteAttributeExtensions(XmlWriter writer, string version)
	{
	}

	protected internal virtual void WriteElementExtensions(XmlWriter writer, string version)
	{
	}
}
