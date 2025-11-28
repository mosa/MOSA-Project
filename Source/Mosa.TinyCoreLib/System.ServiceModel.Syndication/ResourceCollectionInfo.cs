using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.ServiceModel.Syndication;

public class ResourceCollectionInfo
{
	public Collection<string> Accepts
	{
		get
		{
			throw null;
		}
	}

	public Dictionary<XmlQualifiedName, string> AttributeExtensions
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

	public Collection<CategoriesDocument> Categories
	{
		get
		{
			throw null;
		}
	}

	public SyndicationElementExtensionCollection ElementExtensions
	{
		get
		{
			throw null;
		}
	}

	public Uri Link
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

	public ResourceCollectionInfo()
	{
	}

	public ResourceCollectionInfo(TextSyndicationContent title, Uri link)
	{
	}

	public ResourceCollectionInfo(TextSyndicationContent title, Uri link, IEnumerable<CategoriesDocument> categories, bool allowsNewEntries)
	{
	}

	public ResourceCollectionInfo(TextSyndicationContent title, Uri link, IEnumerable<CategoriesDocument> categories, IEnumerable<string> accepts)
	{
	}

	public ResourceCollectionInfo(string title, Uri link)
	{
	}

	protected internal virtual InlineCategoriesDocument CreateInlineCategoriesDocument()
	{
		throw null;
	}

	protected internal virtual ReferencedCategoriesDocument CreateReferencedCategoriesDocument()
	{
		throw null;
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
