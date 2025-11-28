using System.IO;

namespace System.Xml;

public interface IXmlBinaryReaderInitializer
{
	void SetInput(byte[] buffer, int offset, int count, IXmlDictionary? dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession? session, OnXmlDictionaryReaderClose? onClose);

	void SetInput(Stream stream, IXmlDictionary? dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession? session, OnXmlDictionaryReaderClose? onClose);
}
