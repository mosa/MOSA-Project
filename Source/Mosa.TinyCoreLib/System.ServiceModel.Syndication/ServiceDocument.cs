using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.ServiceModel.Syndication;

public class ServiceDocument
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

	public Collection<Workspace> Workspaces
	{
		get
		{
			throw null;
		}
	}

	public ServiceDocument()
	{
	}

	public ServiceDocument(IEnumerable<Workspace> workspaces)
	{
	}

	protected internal virtual Workspace CreateWorkspace()
	{
		throw null;
	}

	public ServiceDocumentFormatter GetFormatter()
	{
		throw null;
	}

	public static ServiceDocument Load(XmlReader reader)
	{
		throw null;
	}

	public static TServiceDocument Load<TServiceDocument>(XmlReader reader) where TServiceDocument : ServiceDocument, new()
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
