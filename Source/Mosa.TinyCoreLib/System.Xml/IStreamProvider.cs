using System.IO;

namespace System.Xml;

public interface IStreamProvider
{
	Stream GetStream();

	void ReleaseStream(Stream stream);
}
