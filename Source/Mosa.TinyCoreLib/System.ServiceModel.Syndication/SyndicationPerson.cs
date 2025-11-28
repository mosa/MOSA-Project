using System.Collections.Generic;
using System.Xml;

namespace System.ServiceModel.Syndication;

public class SyndicationPerson
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

	public string Email
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

	public string Uri
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SyndicationPerson()
	{
	}

	protected SyndicationPerson(SyndicationPerson source)
	{
	}

	public SyndicationPerson(string email)
	{
	}

	public SyndicationPerson(string email, string name, string uri)
	{
	}

	public virtual SyndicationPerson Clone()
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
