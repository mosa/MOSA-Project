using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public class XmlDictionary : IXmlDictionary
{
	public static IXmlDictionary Empty
	{
		get
		{
			throw null;
		}
	}

	public XmlDictionary()
	{
	}

	public XmlDictionary(int capacity)
	{
	}

	public virtual XmlDictionaryString Add(string value)
	{
		throw null;
	}

	public virtual bool TryLookup(int key, [NotNullWhen(true)] out XmlDictionaryString? result)
	{
		throw null;
	}

	public virtual bool TryLookup(string value, [NotNullWhen(true)] out XmlDictionaryString? result)
	{
		throw null;
	}

	public virtual bool TryLookup(XmlDictionaryString value, [NotNullWhen(true)] out XmlDictionaryString? result)
	{
		throw null;
	}
}
