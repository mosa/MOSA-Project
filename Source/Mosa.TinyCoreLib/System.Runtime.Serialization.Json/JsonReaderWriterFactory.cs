using System.IO;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json;

public static class JsonReaderWriterFactory
{
	public static XmlDictionaryReader CreateJsonReader(byte[] buffer, int offset, int count, Encoding? encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose? onClose)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateJsonReader(byte[] buffer, int offset, int count, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateJsonReader(byte[] buffer, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateJsonReader(Stream stream, Encoding? encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose? onClose)
	{
		throw null;
	}

	public static XmlDictionaryReader CreateJsonReader(Stream stream, XmlDictionaryReaderQuotas quotas)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateJsonWriter(Stream stream)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateJsonWriter(Stream stream, Encoding encoding)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateJsonWriter(Stream stream, Encoding encoding, bool ownsStream)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateJsonWriter(Stream stream, Encoding encoding, bool ownsStream, bool indent)
	{
		throw null;
	}

	public static XmlDictionaryWriter CreateJsonWriter(Stream stream, Encoding encoding, bool ownsStream, bool indent, string? indentChars)
	{
		throw null;
	}
}
