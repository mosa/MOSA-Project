using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace System.Xml;

public abstract class XmlReader : IDisposable
{
	public abstract int AttributeCount { get; }

	public abstract string BaseURI { get; }

	public virtual bool CanReadBinaryContent
	{
		get
		{
			throw null;
		}
	}

	public virtual bool CanReadValueChunk
	{
		get
		{
			throw null;
		}
	}

	public virtual bool CanResolveEntity
	{
		get
		{
			throw null;
		}
	}

	public abstract int Depth { get; }

	public abstract bool EOF { get; }

	public virtual bool HasAttributes
	{
		get
		{
			throw null;
		}
	}

	public virtual bool HasValue
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsDefault
	{
		get
		{
			throw null;
		}
	}

	public abstract bool IsEmptyElement { get; }

	public virtual string this[int i]
	{
		get
		{
			throw null;
		}
	}

	public virtual string? this[string name]
	{
		get
		{
			throw null;
		}
	}

	public virtual string? this[string name, string? namespaceURI]
	{
		get
		{
			throw null;
		}
	}

	public abstract string LocalName { get; }

	public virtual string Name
	{
		get
		{
			throw null;
		}
	}

	public abstract string NamespaceURI { get; }

	public abstract XmlNameTable NameTable { get; }

	public abstract XmlNodeType NodeType { get; }

	public abstract string Prefix { get; }

	public virtual char QuoteChar
	{
		get
		{
			throw null;
		}
	}

	public abstract ReadState ReadState { get; }

	public virtual IXmlSchemaInfo? SchemaInfo
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlReaderSettings? Settings
	{
		get
		{
			throw null;
		}
	}

	public abstract string Value { get; }

	public virtual Type ValueType
	{
		get
		{
			throw null;
		}
	}

	public virtual string XmlLang
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlSpace XmlSpace
	{
		get
		{
			throw null;
		}
	}

	public virtual void Close()
	{
	}

	public static XmlReader Create(Stream input)
	{
		throw null;
	}

	public static XmlReader Create(Stream input, XmlReaderSettings? settings)
	{
		throw null;
	}

	public static XmlReader Create(Stream input, XmlReaderSettings? settings, string? baseUri)
	{
		throw null;
	}

	public static XmlReader Create(Stream input, XmlReaderSettings? settings, XmlParserContext? inputContext)
	{
		throw null;
	}

	public static XmlReader Create(TextReader input)
	{
		throw null;
	}

	public static XmlReader Create(TextReader input, XmlReaderSettings? settings)
	{
		throw null;
	}

	public static XmlReader Create(TextReader input, XmlReaderSettings? settings, string? baseUri)
	{
		throw null;
	}

	public static XmlReader Create(TextReader input, XmlReaderSettings? settings, XmlParserContext? inputContext)
	{
		throw null;
	}

	public static XmlReader Create(string inputUri)
	{
		throw null;
	}

	public static XmlReader Create(string inputUri, XmlReaderSettings? settings)
	{
		throw null;
	}

	public static XmlReader Create(string inputUri, XmlReaderSettings? settings, XmlParserContext? inputContext)
	{
		throw null;
	}

	public static XmlReader Create(XmlReader reader, XmlReaderSettings? settings)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public abstract string GetAttribute(int i);

	public abstract string? GetAttribute(string name);

	public abstract string? GetAttribute(string name, string? namespaceURI);

	public virtual Task<string> GetValueAsync()
	{
		throw null;
	}

	public static bool IsName(string str)
	{
		throw null;
	}

	public static bool IsNameToken(string str)
	{
		throw null;
	}

	public virtual bool IsStartElement()
	{
		throw null;
	}

	public virtual bool IsStartElement(string name)
	{
		throw null;
	}

	public virtual bool IsStartElement(string localname, string ns)
	{
		throw null;
	}

	public abstract string? LookupNamespace(string prefix);

	public virtual void MoveToAttribute(int i)
	{
	}

	public abstract bool MoveToAttribute(string name);

	public abstract bool MoveToAttribute(string name, string? ns);

	public virtual XmlNodeType MoveToContent()
	{
		throw null;
	}

	[DebuggerStepThrough]
	public virtual Task<XmlNodeType> MoveToContentAsync()
	{
		throw null;
	}

	public abstract bool MoveToElement();

	public abstract bool MoveToFirstAttribute();

	public abstract bool MoveToNextAttribute();

	public abstract bool Read();

	public virtual Task<bool> ReadAsync()
	{
		throw null;
	}

	public abstract bool ReadAttributeValue();

	public virtual object ReadContentAs(Type returnType, IXmlNamespaceResolver? namespaceResolver)
	{
		throw null;
	}

	[DebuggerStepThrough]
	public virtual Task<object> ReadContentAsAsync(Type returnType, IXmlNamespaceResolver? namespaceResolver)
	{
		throw null;
	}

	public virtual int ReadContentAsBase64(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual int ReadContentAsBinHex(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual bool ReadContentAsBoolean()
	{
		throw null;
	}

	public virtual DateTime ReadContentAsDateTime()
	{
		throw null;
	}

	public virtual DateTimeOffset ReadContentAsDateTimeOffset()
	{
		throw null;
	}

	public virtual decimal ReadContentAsDecimal()
	{
		throw null;
	}

	public virtual double ReadContentAsDouble()
	{
		throw null;
	}

	public virtual float ReadContentAsFloat()
	{
		throw null;
	}

	public virtual int ReadContentAsInt()
	{
		throw null;
	}

	public virtual long ReadContentAsLong()
	{
		throw null;
	}

	public virtual object ReadContentAsObject()
	{
		throw null;
	}

	[DebuggerStepThrough]
	public virtual Task<object> ReadContentAsObjectAsync()
	{
		throw null;
	}

	public virtual string ReadContentAsString()
	{
		throw null;
	}

	public virtual Task<string> ReadContentAsStringAsync()
	{
		throw null;
	}

	public virtual object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		throw null;
	}

	public virtual object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver, string localName, string namespaceURI)
	{
		throw null;
	}

	[DebuggerStepThrough]
	public virtual Task<object> ReadElementContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		throw null;
	}

	public virtual int ReadElementContentAsBase64(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual bool ReadElementContentAsBoolean()
	{
		throw null;
	}

	public virtual bool ReadElementContentAsBoolean(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual DateTime ReadElementContentAsDateTime()
	{
		throw null;
	}

	public virtual DateTime ReadElementContentAsDateTime(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual decimal ReadElementContentAsDecimal()
	{
		throw null;
	}

	public virtual decimal ReadElementContentAsDecimal(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual double ReadElementContentAsDouble()
	{
		throw null;
	}

	public virtual double ReadElementContentAsDouble(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual float ReadElementContentAsFloat()
	{
		throw null;
	}

	public virtual float ReadElementContentAsFloat(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual int ReadElementContentAsInt()
	{
		throw null;
	}

	public virtual int ReadElementContentAsInt(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual long ReadElementContentAsLong()
	{
		throw null;
	}

	public virtual long ReadElementContentAsLong(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual object ReadElementContentAsObject()
	{
		throw null;
	}

	public virtual object ReadElementContentAsObject(string localName, string namespaceURI)
	{
		throw null;
	}

	[DebuggerStepThrough]
	public virtual Task<object> ReadElementContentAsObjectAsync()
	{
		throw null;
	}

	public virtual string ReadElementContentAsString()
	{
		throw null;
	}

	public virtual string ReadElementContentAsString(string localName, string namespaceURI)
	{
		throw null;
	}

	[DebuggerStepThrough]
	public virtual Task<string> ReadElementContentAsStringAsync()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual string ReadElementString()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual string ReadElementString(string name)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual string ReadElementString(string localname, string ns)
	{
		throw null;
	}

	public virtual void ReadEndElement()
	{
	}

	public virtual string ReadInnerXml()
	{
		throw null;
	}

	[DebuggerStepThrough]
	public virtual Task<string> ReadInnerXmlAsync()
	{
		throw null;
	}

	public virtual string ReadOuterXml()
	{
		throw null;
	}

	[DebuggerStepThrough]
	public virtual Task<string> ReadOuterXmlAsync()
	{
		throw null;
	}

	public virtual void ReadStartElement()
	{
	}

	public virtual void ReadStartElement(string name)
	{
	}

	public virtual void ReadStartElement(string localname, string ns)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual string ReadString()
	{
		throw null;
	}

	public virtual XmlReader ReadSubtree()
	{
		throw null;
	}

	public virtual bool ReadToDescendant(string name)
	{
		throw null;
	}

	public virtual bool ReadToDescendant(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual bool ReadToFollowing(string name)
	{
		throw null;
	}

	public virtual bool ReadToFollowing(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual bool ReadToNextSibling(string name)
	{
		throw null;
	}

	public virtual bool ReadToNextSibling(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual int ReadValueChunk(char[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual Task<int> ReadValueChunkAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public abstract void ResolveEntity();

	public virtual void Skip()
	{
	}

	public virtual Task SkipAsync()
	{
		throw null;
	}
}
