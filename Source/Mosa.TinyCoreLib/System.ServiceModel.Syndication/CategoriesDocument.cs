using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.ServiceModel.Syndication;

public abstract class CategoriesDocument
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

	internal CategoriesDocument()
	{
	}

	public static InlineCategoriesDocument Create(Collection<SyndicationCategory> categories)
	{
		throw null;
	}

	public static InlineCategoriesDocument Create(Collection<SyndicationCategory> categories, bool isFixed, string scheme)
	{
		throw null;
	}

	public static ReferencedCategoriesDocument Create(Uri linkToCategoriesDocument)
	{
		throw null;
	}

	public CategoriesDocumentFormatter GetFormatter()
	{
		throw null;
	}

	public static CategoriesDocument Load(XmlReader reader)
	{
		throw null;
	}

	public void Save(XmlWriter writer)
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
