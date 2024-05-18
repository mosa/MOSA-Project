using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public interface IXmlDictionary
{
	bool TryLookup(int key, [NotNullWhen(true)] out XmlDictionaryString? result);

	bool TryLookup(string value, [NotNullWhen(true)] out XmlDictionaryString? result);

	bool TryLookup(XmlDictionaryString value, [NotNullWhen(true)] out XmlDictionaryString? result);
}
