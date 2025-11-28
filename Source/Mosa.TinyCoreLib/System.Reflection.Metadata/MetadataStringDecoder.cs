using System.Text;

namespace System.Reflection.Metadata;

public class MetadataStringDecoder
{
	public static MetadataStringDecoder DefaultUTF8
	{
		get
		{
			throw null;
		}
	}

	public Encoding Encoding
	{
		get
		{
			throw null;
		}
	}

	public MetadataStringDecoder(Encoding encoding)
	{
	}

	public unsafe virtual string GetString(byte* bytes, int byteCount)
	{
		throw null;
	}
}
