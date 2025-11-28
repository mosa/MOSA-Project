using System.IO;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json;

public interface IXmlJsonReaderInitializer
{
	void SetInput(byte[] buffer, int offset, int count, Encoding? encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose? onClose);

	void SetInput(Stream stream, Encoding? encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose? onClose);
}
