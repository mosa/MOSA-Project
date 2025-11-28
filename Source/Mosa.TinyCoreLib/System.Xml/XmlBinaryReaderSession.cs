using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public class XmlBinaryReaderSession : IXmlDictionary
{
	public XmlDictionaryString Add(int id, string value)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool TryLookup(int key, [NotNullWhen(true)] out XmlDictionaryString? result)
	{
		throw null;
	}

	public bool TryLookup(string value, [NotNullWhen(true)] out XmlDictionaryString? result)
	{
		throw null;
	}

	public bool TryLookup(XmlDictionaryString value, [NotNullWhen(true)] out XmlDictionaryString? result)
	{
		throw null;
	}
}
