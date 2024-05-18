using System.ComponentModel;
using System.Reflection.PortableExecutable;

namespace System.Reflection.Metadata;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class PEReaderExtensions
{
	public static MetadataReader GetMetadataReader(this PEReader peReader)
	{
		throw null;
	}

	public static MetadataReader GetMetadataReader(this PEReader peReader, MetadataReaderOptions options)
	{
		throw null;
	}

	public static MetadataReader GetMetadataReader(this PEReader peReader, MetadataReaderOptions options, MetadataStringDecoder? utf8Decoder)
	{
		throw null;
	}

	public static MethodBodyBlock GetMethodBody(this PEReader peReader, int relativeVirtualAddress)
	{
		throw null;
	}
}
