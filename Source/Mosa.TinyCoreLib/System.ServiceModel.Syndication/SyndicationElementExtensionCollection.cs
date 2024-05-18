using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace System.ServiceModel.Syndication;

public sealed class SyndicationElementExtensionCollection : Collection<SyndicationElementExtension>
{
	internal SyndicationElementExtensionCollection()
	{
	}

	public void Add(object extension)
	{
	}

	public void Add(object dataContractExtension, DataContractSerializer serializer)
	{
	}

	public void Add(object xmlSerializerExtension, XmlSerializer serializer)
	{
	}

	public void Add(string outerName, string outerNamespace, object dataContractExtension)
	{
	}

	public void Add(string outerName, string outerNamespace, object dataContractExtension, XmlObjectSerializer dataContractSerializer)
	{
	}

	public void Add(XmlReader xmlReader)
	{
	}

	protected override void ClearItems()
	{
	}

	public XmlReader GetReaderAtElementExtensions()
	{
		throw null;
	}

	protected override void InsertItem(int index, SyndicationElementExtension item)
	{
	}

	public Collection<TExtension> ReadElementExtensions<TExtension>(string extensionName, string extensionNamespace)
	{
		throw null;
	}

	public Collection<TExtension> ReadElementExtensions<TExtension>(string extensionName, string extensionNamespace, XmlObjectSerializer serializer)
	{
		throw null;
	}

	public Collection<TExtension> ReadElementExtensions<TExtension>(string extensionName, string extensionNamespace, XmlSerializer serializer)
	{
		throw null;
	}

	protected override void RemoveItem(int index)
	{
	}

	protected override void SetItem(int index, SyndicationElementExtension item)
	{
	}
}
