using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Formats.Tar;

public sealed class TarReader : IAsyncDisposable, IDisposable
{
	public TarReader(Stream archiveStream, bool leaveOpen = false)
	{
	}

	public void Dispose()
	{
	}

	public ValueTask DisposeAsync()
	{
		throw null;
	}

	public TarEntry? GetNextEntry(bool copyData = false)
	{
		throw null;
	}

	public ValueTask<TarEntry?> GetNextEntryAsync(bool copyData = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
