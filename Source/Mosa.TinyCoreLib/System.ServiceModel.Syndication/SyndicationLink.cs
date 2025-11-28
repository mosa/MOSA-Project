using System.Collections.Generic;
using System.Xml;

namespace System.ServiceModel.Syndication;

public class SyndicationLink
{
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

	public SyndicationElementExtensionCollection ElementExtensions
	{
		get
		{
			throw null;
		}
	}

	public long Length
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string MediaType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string RelationshipType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Title
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Uri Uri
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SyndicationLink()
	{
	}

	protected SyndicationLink(SyndicationLink source)
	{
	}

	public SyndicationLink(Uri uri)
	{
	}

	public SyndicationLink(Uri uri, string relationshipType, string title, string mediaType, long length)
	{
	}

	public virtual SyndicationLink Clone()
	{
		throw null;
	}

	public static SyndicationLink CreateAlternateLink(Uri uri)
	{
		throw null;
	}

	public static SyndicationLink CreateAlternateLink(Uri uri, string mediaType)
	{
		throw null;
	}

	public static SyndicationLink CreateMediaEnclosureLink(Uri uri, string mediaType, long length)
	{
		throw null;
	}

	public static SyndicationLink CreateSelfLink(Uri uri)
	{
		throw null;
	}

	public static SyndicationLink CreateSelfLink(Uri uri, string mediaType)
	{
		throw null;
	}

	public Uri GetAbsoluteUri()
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
