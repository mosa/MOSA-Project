using System.IO;
using System.Text;

namespace System.Runtime.Serialization.Json;

public interface IXmlJsonWriterInitializer
{
	void SetOutput(Stream stream, Encoding encoding, bool ownsStream);
}
