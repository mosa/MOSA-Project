using System.Collections.Generic;
using System.Xml;

namespace System.ServiceModel.Syndication;

public class SyndicationCategory
{
	public Dictionary<XmlQualifiedName, string> AttributeExtensions
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

	public string Label
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Scheme
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SyndicationCategory()
	{
	}

	protected SyndicationCategory(SyndicationCategory source)
	{
	}

	public SyndicationCategory(string name)
	{
	}

	public SyndicationCategory(string name, string scheme, string label)
	{
	}

	public virtual SyndicationCategory Clone()
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
