using System.Runtime.Serialization;
using System.Xml;

namespace System.ServiceModel.Syndication;

[DataContract]
public abstract class SyndicationFeedFormatter
{
	public TryParseDateTimeCallback DateTimeParser
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TryParseUriCallback UriParser
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SyndicationFeed Feed
	{
		get
		{
			throw null;
		}
	}

	public abstract string Version { get; }

	protected SyndicationFeedFormatter()
	{
	}

	protected SyndicationFeedFormatter(SyndicationFeed feedToWrite)
	{
	}

	public abstract bool CanRead(XmlReader reader);

	protected internal static SyndicationCategory CreateCategory(SyndicationFeed feed)
	{
		throw null;
	}

	protected internal static SyndicationCategory CreateCategory(SyndicationItem item)
	{
		throw null;
	}

	protected abstract SyndicationFeed CreateFeedInstance();

	protected internal static SyndicationItem CreateItem(SyndicationFeed feed)
	{
		throw null;
	}

	protected internal static SyndicationLink CreateLink(SyndicationFeed feed)
	{
		throw null;
	}

	protected internal static SyndicationLink CreateLink(SyndicationItem item)
	{
		throw null;
	}

	protected internal static SyndicationPerson CreatePerson(SyndicationFeed feed)
	{
		throw null;
	}

	protected internal static SyndicationPerson CreatePerson(SyndicationItem item)
	{
		throw null;
	}

	protected internal static void LoadElementExtensions(XmlReader reader, SyndicationCategory category, int maxExtensionSize)
	{
	}

	protected internal static void LoadElementExtensions(XmlReader reader, SyndicationFeed feed, int maxExtensionSize)
	{
	}

	protected internal static void LoadElementExtensions(XmlReader reader, SyndicationItem item, int maxExtensionSize)
	{
	}

	protected internal static void LoadElementExtensions(XmlReader reader, SyndicationLink link, int maxExtensionSize)
	{
	}

	protected internal static void LoadElementExtensions(XmlReader reader, SyndicationPerson person, int maxExtensionSize)
	{
	}

	public abstract void ReadFrom(XmlReader reader);

	protected internal virtual void SetFeed(SyndicationFeed feed)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	protected internal static bool TryParseAttribute(string name, string ns, string value, SyndicationCategory category, string version)
	{
		throw null;
	}

	protected internal static bool TryParseAttribute(string name, string ns, string value, SyndicationFeed feed, string version)
	{
		throw null;
	}

	protected internal static bool TryParseAttribute(string name, string ns, string value, SyndicationItem item, string version)
	{
		throw null;
	}

	protected internal static bool TryParseAttribute(string name, string ns, string value, SyndicationLink link, string version)
	{
		throw null;
	}

	protected internal static bool TryParseAttribute(string name, string ns, string value, SyndicationPerson person, string version)
	{
		throw null;
	}

	protected internal static bool TryParseContent(XmlReader reader, SyndicationItem item, string contentType, string version, out SyndicationContent content)
	{
		throw null;
	}

	protected internal static bool TryParseElement(XmlReader reader, SyndicationCategory category, string version)
	{
		throw null;
	}

	protected internal static bool TryParseElement(XmlReader reader, SyndicationFeed feed, string version)
	{
		throw null;
	}

	protected internal static bool TryParseElement(XmlReader reader, SyndicationItem item, string version)
	{
		throw null;
	}

	protected internal static bool TryParseElement(XmlReader reader, SyndicationLink link, string version)
	{
		throw null;
	}

	protected internal static bool TryParseElement(XmlReader reader, SyndicationPerson person, string version)
	{
		throw null;
	}

	protected internal static void WriteAttributeExtensions(XmlWriter writer, SyndicationCategory category, string version)
	{
	}

	protected internal static void WriteAttributeExtensions(XmlWriter writer, SyndicationFeed feed, string version)
	{
	}

	protected internal static void WriteAttributeExtensions(XmlWriter writer, SyndicationItem item, string version)
	{
	}

	protected internal static void WriteAttributeExtensions(XmlWriter writer, SyndicationLink link, string version)
	{
	}

	protected internal static void WriteAttributeExtensions(XmlWriter writer, SyndicationPerson person, string version)
	{
	}

	protected internal static void WriteElementExtensions(XmlWriter writer, SyndicationCategory category, string version)
	{
	}

	protected internal static void WriteElementExtensions(XmlWriter writer, SyndicationFeed feed, string version)
	{
	}

	protected internal static void WriteElementExtensions(XmlWriter writer, SyndicationItem item, string version)
	{
	}

	protected internal static void WriteElementExtensions(XmlWriter writer, SyndicationLink link, string version)
	{
	}

	protected internal static void WriteElementExtensions(XmlWriter writer, SyndicationPerson person, string version)
	{
	}

	public abstract void WriteTo(XmlWriter writer);
}
