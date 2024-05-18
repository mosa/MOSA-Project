using System.Text;

namespace System.IO.Compression;

public static class ZipFile
{
	public static void CreateFromDirectory(string sourceDirectoryName, Stream destination)
	{
	}

	public static void CreateFromDirectory(string sourceDirectoryName, Stream destination, CompressionLevel compressionLevel, bool includeBaseDirectory)
	{
	}

	public static void CreateFromDirectory(string sourceDirectoryName, Stream destination, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding? entryNameEncoding)
	{
	}

	public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName)
	{
	}

	public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory)
	{
	}

	public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding? entryNameEncoding)
	{
	}

	public static void ExtractToDirectory(Stream source, string destinationDirectoryName)
	{
	}

	public static void ExtractToDirectory(Stream source, string destinationDirectoryName, bool overwriteFiles)
	{
	}

	public static void ExtractToDirectory(Stream source, string destinationDirectoryName, Encoding? entryNameEncoding)
	{
	}

	public static void ExtractToDirectory(Stream source, string destinationDirectoryName, Encoding? entryNameEncoding, bool overwriteFiles)
	{
	}

	public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
	{
	}

	public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, bool overwriteFiles)
	{
	}

	public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, Encoding? entryNameEncoding)
	{
	}

	public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, Encoding? entryNameEncoding, bool overwriteFiles)
	{
	}

	public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode)
	{
		throw null;
	}

	public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode, Encoding? entryNameEncoding)
	{
		throw null;
	}

	public static ZipArchive OpenRead(string archiveFileName)
	{
		throw null;
	}
}
