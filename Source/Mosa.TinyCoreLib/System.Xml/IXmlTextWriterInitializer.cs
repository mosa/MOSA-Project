using System.IO;
using System.Text;

namespace System.Xml;

public interface IXmlTextWriterInitializer
{
	void SetOutput(Stream stream, Encoding encoding, bool ownsStream);
}
