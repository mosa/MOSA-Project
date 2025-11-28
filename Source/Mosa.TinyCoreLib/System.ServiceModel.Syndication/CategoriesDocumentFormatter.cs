using System.Runtime.Serialization;
using System.Xml;

namespace System.ServiceModel.Syndication;

[DataContract]
public abstract class CategoriesDocumentFormatter
{
	public CategoriesDocument Document
	{
		get
		{
			throw null;
		}
	}

	public abstract string Version { get; }

	protected CategoriesDocumentFormatter()
	{
	}

	protected CategoriesDocumentFormatter(CategoriesDocument documentToWrite)
	{
	}

	public abstract bool CanRead(XmlReader reader);

	protected virtual InlineCategoriesDocument CreateInlineCategoriesDocument()
	{
		throw null;
	}

	protected virtual ReferencedCategoriesDocument CreateReferencedCategoriesDocument()
	{
		throw null;
	}

	public abstract void ReadFrom(XmlReader reader);

	protected virtual void SetDocument(CategoriesDocument document)
	{
	}

	public abstract void WriteTo(XmlWriter writer);
}
