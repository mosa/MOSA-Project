using System.Runtime.Serialization;
using System.Xml;

namespace System.ServiceModel.Syndication;

[DataContract]
public abstract class SyndicationItemFormatter
{
	public SyndicationItem Item
	{
		get
		{
			throw null;
		}
	}

	public abstract string Version { get; }

	protected SyndicationItemFormatter()
	{
	}

	protected SyndicationItemFormatter(SyndicationItem itemToWrite)
	{
	}

	public abstract bool CanRead(XmlReader reader);

	protected static SyndicationCategory CreateCategory(SyndicationItem item)
	{
		throw null;
	}

	protected abstract SyndicationItem CreateItemInstance();

	protected static SyndicationLink CreateLink(SyndicationItem item)
	{
		throw null;
	}

	protected static SyndicationPerson CreatePerson(SyndicationItem item)
	{
		throw null;
	}

	protected static void LoadElementExtensions(XmlReader reader, SyndicationCategory category, int maxExtensionSize)
	{
	}

	protected static void LoadElementExtensions(XmlReader reader, SyndicationItem item, int maxExtensionSize)
	{
	}

	protected static void LoadElementExtensions(XmlReader reader, SyndicationLink link, int maxExtensionSize)
	{
	}

	protected static void LoadElementExtensions(XmlReader reader, SyndicationPerson person, int maxExtensionSize)
	{
	}

	public abstract void ReadFrom(XmlReader reader);

	protected internal virtual void SetItem(SyndicationItem item)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	protected static bool TryParseAttribute(string name, string ns, string value, SyndicationCategory category, string version)
	{
		throw null;
	}

	protected static bool TryParseAttribute(string name, string ns, string value, SyndicationItem item, string version)
	{
		throw null;
	}

	protected static bool TryParseAttribute(string name, string ns, string value, SyndicationLink link, string version)
	{
		throw null;
	}

	protected static bool TryParseAttribute(string name, string ns, string value, SyndicationPerson person, string version)
	{
		throw null;
	}

	protected static bool TryParseContent(XmlReader reader, SyndicationItem item, string contentType, string version, out SyndicationContent content)
	{
		throw null;
	}

	protected static bool TryParseElement(XmlReader reader, SyndicationCategory category, string version)
	{
		throw null;
	}

	protected static bool TryParseElement(XmlReader reader, SyndicationItem item, string version)
	{
		throw null;
	}

	protected static bool TryParseElement(XmlReader reader, SyndicationLink link, string version)
	{
		throw null;
	}

	protected static bool TryParseElement(XmlReader reader, SyndicationPerson person, string version)
	{
		throw null;
	}

	protected static void WriteAttributeExtensions(XmlWriter writer, SyndicationCategory category, string version)
	{
	}

	protected static void WriteAttributeExtensions(XmlWriter writer, SyndicationItem item, string version)
	{
	}

	protected static void WriteAttributeExtensions(XmlWriter writer, SyndicationLink link, string version)
	{
	}

	protected static void WriteAttributeExtensions(XmlWriter writer, SyndicationPerson person, string version)
	{
	}

	protected void WriteElementExtensions(XmlWriter writer, SyndicationCategory category, string version)
	{
	}

	protected static void WriteElementExtensions(XmlWriter writer, SyndicationItem item, string version)
	{
	}

	protected void WriteElementExtensions(XmlWriter writer, SyndicationLink link, string version)
	{
	}

	protected void WriteElementExtensions(XmlWriter writer, SyndicationPerson person, string version)
	{
	}

	public abstract void WriteTo(XmlWriter writer);
}
