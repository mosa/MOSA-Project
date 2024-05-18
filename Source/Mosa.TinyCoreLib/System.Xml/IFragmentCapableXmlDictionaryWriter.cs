using System.IO;

namespace System.Xml;

public interface IFragmentCapableXmlDictionaryWriter
{
	bool CanFragment { get; }

	void EndFragment();

	void StartFragment(Stream stream, bool generateSelfContainedTextFragment);

	void WriteFragment(byte[] buffer, int offset, int count);
}
