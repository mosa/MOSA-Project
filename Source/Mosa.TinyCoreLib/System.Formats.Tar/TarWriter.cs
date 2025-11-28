using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Formats.Tar;

public sealed class TarWriter : IAsyncDisposable, IDisposable
{
	public TarEntryFormat Format
	{
		get
		{
			throw null;
		}
	}

	public TarWriter(Stream archiveStream)
	{
	}

	public TarWriter(Stream archiveStream, bool leaveOpen = false)
	{
	}

	public TarWriter(Stream archiveStream, TarEntryFormat format = TarEntryFormat.Pax, bool leaveOpen = false)
	{
	}

	public void Dispose()
	{
	}

	public ValueTask DisposeAsync()
	{
		throw null;
	}

	public void WriteEntry(TarEntry entry)
	{
	}

	public void WriteEntry(string fileName, string? entryName)
	{
	}

	public Task WriteEntryAsync(TarEntry entry, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public Task WriteEntryAsync(string fileName, string? entryName, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
