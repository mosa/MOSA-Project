using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Formats.Tar;

public static class TarFile
{
	public static void CreateFromDirectory(string sourceDirectoryName, Stream destination, bool includeBaseDirectory)
	{
	}

	public static void CreateFromDirectory(string sourceDirectoryName, string destinationFileName, bool includeBaseDirectory)
	{
	}

	public static Task CreateFromDirectoryAsync(string sourceDirectoryName, Stream destination, bool includeBaseDirectory, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task CreateFromDirectoryAsync(string sourceDirectoryName, string destinationFileName, bool includeBaseDirectory, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static void ExtractToDirectory(Stream source, string destinationDirectoryName, bool overwriteFiles)
	{
	}

	public static void ExtractToDirectory(string sourceFileName, string destinationDirectoryName, bool overwriteFiles)
	{
	}

	public static Task ExtractToDirectoryAsync(Stream source, string destinationDirectoryName, bool overwriteFiles, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task ExtractToDirectoryAsync(string sourceFileName, string destinationDirectoryName, bool overwriteFiles, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
