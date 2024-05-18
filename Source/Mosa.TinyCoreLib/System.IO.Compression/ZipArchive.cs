using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System.IO.Compression;

public class ZipArchive : IDisposable
{
	public string Comment
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public ReadOnlyCollection<ZipArchiveEntry> Entries
	{
		get
		{
			throw null;
		}
	}

	public ZipArchiveMode Mode
	{
		get
		{
			throw null;
		}
	}

	public ZipArchive(Stream stream)
	{
	}

	public ZipArchive(Stream stream, ZipArchiveMode mode)
	{
	}

	public ZipArchive(Stream stream, ZipArchiveMode mode, bool leaveOpen)
	{
	}

	public ZipArchive(Stream stream, ZipArchiveMode mode, bool leaveOpen, Encoding? entryNameEncoding)
	{
	}

	public ZipArchiveEntry CreateEntry(string entryName)
	{
		throw null;
	}

	public ZipArchiveEntry CreateEntry(string entryName, CompressionLevel compressionLevel)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public ZipArchiveEntry? GetEntry(string entryName)
	{
		throw null;
	}
}
