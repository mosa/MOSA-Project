using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.ServiceModel.Syndication;

public class SyndicationItem
{
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

	public SyndicationContent Content
	{
		get
		{
			throw null;
		}
		set
		{
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

	public SyndicationElementExtensionCollection ElementExtensions
	{
		get
		{
			throw null;
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

	public DateTimeOffset PublishDate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SyndicationFeed SourceFeed
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TextSyndicationContent Summary
	{
		get
		{
			throw null;
		}
		set
		{
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

	public SyndicationItem()
	{
	}

	protected SyndicationItem(SyndicationItem source)
	{
	}

	public SyndicationItem(string title, SyndicationContent content, Uri itemAlternateLink, string id, DateTimeOffset lastUpdatedTime)
	{
	}

	public SyndicationItem(string title, string content, Uri itemAlternateLink)
	{
	}

	public SyndicationItem(string title, string content, Uri itemAlternateLink, string id, DateTimeOffset lastUpdatedTime)
	{
	}

	public void AddPermalink(Uri permalink)
	{
	}

	public virtual SyndicationItem Clone()
	{
		throw null;
	}

	protected internal virtual SyndicationCategory CreateCategory()
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

	public Atom10ItemFormatter GetAtom10Formatter()
	{
		throw null;
	}

	public Rss20ItemFormatter GetRss20Formatter()
	{
		throw null;
	}

	public Rss20ItemFormatter GetRss20Formatter(bool serializeExtensionsAsAtom)
	{
		throw null;
	}

	public static SyndicationItem Load(XmlReader reader)
	{
		throw null;
	}

	public static TSyndicationItem Load<TSyndicationItem>(XmlReader reader) where TSyndicationItem : SyndicationItem, new()
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

	protected internal virtual bool TryParseContent(XmlReader reader, string contentType, string version, out SyndicationContent content)
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
