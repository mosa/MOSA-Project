using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace System.Xml;

public abstract class XmlDictionaryWriter : XmlWriter
{
	public virtual bool CanCanonicalize
	{
		get
		{
			throw null;
		}
	}

	public static XmlDictionaryWriter CreateBinaryWriter(Stream stream)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateBinaryWriter(Stream stream, IXmlDictionary? dictionary)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateBinaryWriter(Stream stream, IXmlDictionary? dictionary, XmlBinaryWriterSession? session)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateBinaryWriter(Stream stream, IXmlDictionary? dictionary, XmlBinaryWriterSession? session, bool ownsStream)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateDictionaryWriter(XmlWriter writer)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateMtomWriter(Stream stream, Encoding encoding, int maxSizeInBytes, string startInfo)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateMtomWriter(Stream stream, Encoding encoding, int maxSizeInBytes, string startInfo, string? boundary, string? startUri, bool writeMessageHeaders, bool ownsStream)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateTextWriter(Stream stream)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateTextWriter(Stream stream, Encoding encoding)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateTextWriter(Stream stream, Encoding encoding, bool ownsStream)
	{
		throw null;
	}

	public virtual void EndCanonicalization()
	{
	}

	public virtual void StartCanonicalization(Stream stream, bool includeComments, string[]? inclusivePrefixes)
	{
	}

	public virtual void WriteArray(string? prefix, string localName, string? namespaceUri, bool[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, string localName, string? namespaceUri, DateTime[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, string localName, string? namespaceUri, decimal[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, string localName, string? namespaceUri, double[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, string localName, string? namespaceUri, Guid[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, string localName, string? namespaceUri, short[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, string localName, string? namespaceUri, int[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, string localName, string? namespaceUri, long[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, string localName, string? namespaceUri, float[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, string localName, string? namespaceUri, TimeSpan[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, bool[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, DateTime[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, decimal[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, double[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, Guid[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, short[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, int[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, long[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, float[] array, int offset, int count)
	{
	}

	public virtual void WriteArray(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, TimeSpan[] array, int offset, int count)
	{
	}

	public void WriteAttributeString(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, string? value)
	{
	}

	public void WriteAttributeString(XmlDictionaryString localName, XmlDictionaryString? namespaceUri, string? value)
	{
	}

	public override Task WriteBase64Async(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public void WriteElementString(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri, string? value)
	{
	}

	public void WriteElementString(XmlDictionaryString localName, XmlDictionaryString? namespaceUri, string? value)
	{
	}

	public virtual void WriteNode(XmlDictionaryReader reader, bool defattr)
	{
	}

	public override void WriteNode(XmlReader reader, bool defattr)
	{
	}

	public virtual void WriteQualifiedName(XmlDictionaryString localName, XmlDictionaryString? namespaceUri)
	{
	}

	public virtual void WriteStartAttribute(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri)
	{
	}

	public void WriteStartAttribute(XmlDictionaryString localName, XmlDictionaryString? namespaceUri)
	{
	}

	public virtual void WriteStartElement(string? prefix, XmlDictionaryString localName, XmlDictionaryString? namespaceUri)
	{
	}

	public void WriteStartElement(XmlDictionaryString localName, XmlDictionaryString? namespaceUri)
	{
	}

	public virtual void WriteString(XmlDictionaryString? value)
	{
	}

	protected virtual void WriteTextNode(XmlDictionaryReader reader, bool isAttribute)
	{
	}

	public virtual void WriteValue(Guid value)
	{
	}

	public virtual void WriteValue(TimeSpan value)
	{
	}

	public virtual void WriteValue(IStreamProvider value)
	{
	}

	public virtual void WriteValue(UniqueId value)
	{
	}

	public virtual void WriteValue(XmlDictionaryString? value)
	{
	}

	public virtual Task WriteValueAsync(IStreamProvider value)
	{
		throw null;
	}

	public virtual void WriteXmlAttribute(string localName, string? value)
	{
	}

	public virtual void WriteXmlAttribute(XmlDictionaryString localName, XmlDictionaryString? value)
	{
	}

	public virtual void WriteXmlnsAttribute(string? prefix, string namespaceUri)
	{
	}

	public virtual void WriteXmlnsAttribute(string? prefix, XmlDictionaryString namespaceUri)
	{
	}
}
