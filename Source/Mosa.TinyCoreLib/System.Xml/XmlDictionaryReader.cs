using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace System.Xml;

public abstract class XmlDictionaryReader : XmlReader
{
	public virtual bool CanCanonicalize
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlDictionaryReaderQuotas Quotas
	{
		get
		{
			throw null;
		}
	}

	public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary? dictionary, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary? dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession? session)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary? dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession? session, OnXmlDictionaryReaderClose? onClose)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary? dictionary, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary? dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession? session)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary? dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession? session, OnXmlDictionaryReaderClose? onClose)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateBinaryReader(Stream stream, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateDictionaryReader(XmlReader reader)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, string? contentType, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, string? contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose? onClose)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, string? contentType, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, string? contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose? onClose)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateTextReader(byte[] buffer, int offset, int count, Encoding? encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose? onClose)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateTextReader(byte[] buffer, int offset, int count, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateTextReader(byte[] buffer, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateTextReader(Stream stream, Encoding? encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose? onClose)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateTextReader(Stream stream, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public virtual void EndCanonicalization()
	{
	}

	public virtual string? GetAttribute(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual void GetNonAtomizedNames(out string localName, out string namespaceUri)
	{
		throw null;
	}

	public virtual int IndexOfLocalName(string[] localNames, string namespaceUri)
	{
		throw null;
	}

	public virtual int IndexOfLocalName(XmlDictionaryString[] localNames, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual bool IsLocalName(string localName)
	{
		throw null;
	}

	public virtual bool IsLocalName(XmlDictionaryString localName)
	{
		throw null;
	}

	public virtual bool IsNamespaceUri(string namespaceUri)
	{
		throw null;
	}

	public virtual bool IsNamespaceUri(XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual bool IsStartArray([NotNullWhen(true)] out Type? type)
	{
		throw null;
	}

	public virtual bool IsStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	protected bool IsTextNode(XmlNodeType nodeType)
	{
		throw null;
	}

	public virtual void MoveToStartElement()
	{
	}

	public virtual void MoveToStartElement(string name)
	{
	}

	public virtual void MoveToStartElement(string localName, string namespaceUri)
	{
	}

	public virtual void MoveToStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
	}

	public virtual int ReadArray(string localName, string namespaceUri, bool[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(string localName, string namespaceUri, DateTime[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(string localName, string namespaceUri, decimal[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(string localName, string namespaceUri, double[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(string localName, string namespaceUri, Guid[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(string localName, string namespaceUri, short[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(string localName, string namespaceUri, int[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(string localName, string namespaceUri, long[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(string localName, string namespaceUri, float[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, DateTime[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, decimal[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
	{
		throw null;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
	{
		throw null;
	}

	public virtual bool[] ReadBooleanArray(string localName, string namespaceUri)
	{
		throw null;
	}

	public virtual bool[] ReadBooleanArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public override object ReadContentAs(Type type, IXmlNamespaceResolver? namespaceResolver)
	{
		throw null;
	}

	public virtual byte[] ReadContentAsBase64()
	{
		throw null;
	}

	public virtual byte[] ReadContentAsBinHex()
	{
		throw null;
	}

	protected byte[] ReadContentAsBinHex(int maxByteArrayContentLength)
	{
		throw null;
	}

	public virtual int ReadContentAsChars(char[] chars, int offset, int count)
	{
		throw null;
	}

	public override decimal ReadContentAsDecimal()
	{
		throw null;
	}

	public override float ReadContentAsFloat()
	{
		throw null;
	}

	public virtual Guid ReadContentAsGuid()
	{
		throw null;
	}

	public virtual void ReadContentAsQualifiedName(out string localName, out string namespaceUri)
	{
		throw null;
	}

	public override string ReadContentAsString()
	{
		throw null;
	}

	protected string ReadContentAsString(int maxStringContentLength)
	{
		throw null;
	}

	public virtual string ReadContentAsString(string[] strings, out int index)
	{
		throw null;
	}

	public virtual string ReadContentAsString(XmlDictionaryString[] strings, out int index)
	{
		throw null;
	}

	public virtual TimeSpan ReadContentAsTimeSpan()
	{
		throw null;
	}

	public virtual UniqueId ReadContentAsUniqueId()
	{
		throw null;
	}

	public virtual DateTime[] ReadDateTimeArray(string localName, string namespaceUri)
	{
		throw null;
	}

	public virtual DateTime[] ReadDateTimeArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual decimal[] ReadDecimalArray(string localName, string namespaceUri)
	{
		throw null;
	}

	public virtual decimal[] ReadDecimalArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual double[] ReadDoubleArray(string localName, string namespaceUri)
	{
		throw null;
	}

	public virtual double[] ReadDoubleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual byte[] ReadElementContentAsBase64()
	{
		throw null;
	}

	public virtual byte[] ReadElementContentAsBinHex()
	{
		throw null;
	}

	public override bool ReadElementContentAsBoolean()
	{
		throw null;
	}

	public override DateTime ReadElementContentAsDateTime()
	{
		throw null;
	}

	public override decimal ReadElementContentAsDecimal()
	{
		throw null;
	}

	public override double ReadElementContentAsDouble()
	{
		throw null;
	}

	public override float ReadElementContentAsFloat()
	{
		throw null;
	}

	public virtual Guid ReadElementContentAsGuid()
	{
		throw null;
	}

	public override int ReadElementContentAsInt()
	{
		throw null;
	}

	public override long ReadElementContentAsLong()
	{
		throw null;
	}

	public override string ReadElementContentAsString()
	{
		throw null;
	}

	public virtual TimeSpan ReadElementContentAsTimeSpan()
	{
		throw null;
	}

	public virtual UniqueId ReadElementContentAsUniqueId()
	{
		throw null;
	}

	public virtual void ReadFullStartElement()
	{
	}

	public virtual void ReadFullStartElement(string name)
	{
	}

	public virtual void ReadFullStartElement(string localName, string namespaceUri)
	{
	}

	public virtual void ReadFullStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
	}

	public virtual Guid[] ReadGuidArray(string localName, string namespaceUri)
	{
		throw null;
	}

	public virtual Guid[] ReadGuidArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual short[] ReadInt16Array(string localName, string namespaceUri)
	{
		throw null;
	}

	public virtual short[] ReadInt16Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual int[] ReadInt32Array(string localName, string namespaceUri)
	{
		throw null;
	}

	public virtual int[] ReadInt32Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual long[] ReadInt64Array(string localName, string namespaceUri)
	{
		throw null;
	}

	public virtual long[] ReadInt64Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual float[] ReadSingleArray(string localName, string namespaceUri)
	{
		throw null;
	}

	public virtual float[] ReadSingleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual void ReadStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
	}

	public override string ReadString()
	{
		throw null;
	}

	protected string ReadString(int maxStringContentLength)
	{
		throw null;
	}

	public virtual TimeSpan[] ReadTimeSpanArray(string localName, string namespaceUri)
	{
		throw null;
	}

	public virtual TimeSpan[] ReadTimeSpanArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		throw null;
	}

	public virtual int ReadValueAsBase64(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public virtual void StartCanonicalization(Stream stream, bool includeComments, string[]? inclusivePrefixes)
	{
	}

	public virtual bool TryGetArrayLength(out int count)
	{
		throw null;
	}

	public virtual bool TryGetBase64ContentLength(out int length)
	{
		throw null;
	}

	public virtual bool TryGetLocalNameAsDictionaryString([NotNullWhen(true)] out XmlDictionaryString? localName)
	{
		throw null;
	}

	public virtual bool TryGetNamespaceUriAsDictionaryString([NotNullWhen(true)] out XmlDictionaryString? namespaceUri)
	{
		throw null;
	}

	public virtual bool TryGetValueAsDictionaryString([NotNullWhen(true)] out XmlDictionaryString? value)
	{
		throw null;
	}
}
