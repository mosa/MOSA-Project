using System.IO;
using System.Text;

namespace System.Xml;

public interface IXmlTextReaderInitializer
{
	void SetInput(byte[] buffer, int offset, int count, Encoding? encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose? onClose);

	void SetInput(Stream stream, Encoding? encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose? onClose);
}
