using System.Collections.Immutable;
using System.IO;

namespace System.Reflection.Metadata;

public sealed class MetadataReaderProvider : IDisposable
{
	internal MetadataReaderProvider()
	{
	}

	public void Dispose()
	{
	}

	public unsafe static MetadataReaderProvider FromMetadataImage(byte* start, int size)
	{
		throw null;
	}

	public static MetadataReaderProvider FromMetadataImage(ImmutableArray<byte> image)
	{
		throw null;
	}

	public static MetadataReaderProvider FromMetadataStream(Stream stream, MetadataStreamOptions options = MetadataStreamOptions.Default, int size = 0)
	{
		throw null;
	}

	public unsafe static MetadataReaderProvider FromPortablePdbImage(byte* start, int size)
	{
		throw null;
	}

	public static MetadataReaderProvider FromPortablePdbImage(ImmutableArray<byte> image)
	{
		throw null;
	}

	public static MetadataReaderProvider FromPortablePdbStream(Stream stream, MetadataStreamOptions options = MetadataStreamOptions.Default, int size = 0)
	{
		throw null;
	}

	public MetadataReader GetMetadataReader(MetadataReaderOptions options = MetadataReaderOptions.ApplyWindowsRuntimeProjections, MetadataStringDecoder? utf8Decoder = null)
	{
		throw null;
	}
}
