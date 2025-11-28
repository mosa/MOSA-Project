using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.ServiceModel.Syndication;

public class Workspace
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

	public Collection<ResourceCollectionInfo> Collections
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

	public Workspace()
	{
	}

	public Workspace(TextSyndicationContent title, IEnumerable<ResourceCollectionInfo> collections)
	{
	}

	public Workspace(string title, IEnumerable<ResourceCollectionInfo> collections)
	{
	}

	protected internal virtual ResourceCollectionInfo CreateResourceCollection()
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
