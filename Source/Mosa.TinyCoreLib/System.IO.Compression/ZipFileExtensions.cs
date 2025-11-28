using System.ComponentModel;

namespace System.IO.Compression;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class ZipFileExtensions
{
	public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName)
	{
		throw null;
	}

	public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName, CompressionLevel compressionLevel)
	{
		throw null;
	}

	public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName)
	{
	}

	public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName, bool overwriteFiles)
	{
	}

	public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName)
	{
	}

	public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName, bool overwrite)
	{
	}
}
