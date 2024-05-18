using System.Runtime.Serialization;
using System.Xml;

namespace System.ServiceModel.Syndication;

[DataContract]
public abstract class ServiceDocumentFormatter
{
	public ServiceDocument Document
	{
		get
		{
			throw null;
		}
	}

	public abstract string Version { get; }

	protected ServiceDocumentFormatter()
	{
	}

	protected ServiceDocumentFormatter(ServiceDocument documentToWrite)
	{
	}

	public abstract bool CanRead(XmlReader reader);

	protected static SyndicationCategory CreateCategory(InlineCategoriesDocument inlineCategories)
	{
		throw null;
	}

	protected static ResourceCollectionInfo CreateCollection(Workspace workspace)
	{
		throw null;
	}

	protected virtual ServiceDocument CreateDocumentInstance()
	{
		throw null;
	}

	protected static InlineCategoriesDocument CreateInlineCategories(ResourceCollectionInfo collection)
	{
		throw null;
	}

	protected static ReferencedCategoriesDocument CreateReferencedCategories(ResourceCollectionInfo collection)
	{
		throw null;
	}

	protected static Workspace CreateWorkspace(ServiceDocument document)
	{
		throw null;
	}

	protected static void LoadElementExtensions(XmlReader reader, CategoriesDocument categories, int maxExtensionSize)
	{
	}

	protected static void LoadElementExtensions(XmlReader reader, ResourceCollectionInfo collection, int maxExtensionSize)
	{
	}

	protected static void LoadElementExtensions(XmlReader reader, ServiceDocument document, int maxExtensionSize)
	{
	}

	protected static void LoadElementExtensions(XmlReader reader, Workspace workspace, int maxExtensionSize)
	{
	}

	public abstract void ReadFrom(XmlReader reader);

	protected virtual void SetDocument(ServiceDocument document)
	{
	}

	protected static bool TryParseAttribute(string name, string ns, string value, CategoriesDocument categories, string version)
	{
		throw null;
	}

	protected static bool TryParseAttribute(string name, string ns, string value, ResourceCollectionInfo collection, string version)
	{
		throw null;
	}

	protected static bool TryParseAttribute(string name, string ns, string value, ServiceDocument document, string version)
	{
		throw null;
	}

	protected static bool TryParseAttribute(string name, string ns, string value, Workspace workspace, string version)
	{
		throw null;
	}

	protected static bool TryParseElement(XmlReader reader, CategoriesDocument categories, string version)
	{
		throw null;
	}

	protected static bool TryParseElement(XmlReader reader, ResourceCollectionInfo collection, string version)
	{
		throw null;
	}

	protected static bool TryParseElement(XmlReader reader, ServiceDocument document, string version)
	{
		throw null;
	}

	protected static bool TryParseElement(XmlReader reader, Workspace workspace, string version)
	{
		throw null;
	}

	protected static void WriteAttributeExtensions(XmlWriter writer, CategoriesDocument categories, string version)
	{
	}

	protected static void WriteAttributeExtensions(XmlWriter writer, ResourceCollectionInfo collection, string version)
	{
	}

	protected static void WriteAttributeExtensions(XmlWriter writer, ServiceDocument document, string version)
	{
	}

	protected static void WriteAttributeExtensions(XmlWriter writer, Workspace workspace, string version)
	{
	}

	protected static void WriteElementExtensions(XmlWriter writer, CategoriesDocument categories, string version)
	{
	}

	protected static void WriteElementExtensions(XmlWriter writer, ResourceCollectionInfo collection, string version)
	{
	}

	protected static void WriteElementExtensions(XmlWriter writer, ServiceDocument document, string version)
	{
	}

	protected static void WriteElementExtensions(XmlWriter writer, Workspace workspace, string version)
	{
	}

	public abstract void WriteTo(XmlWriter writer);
}
