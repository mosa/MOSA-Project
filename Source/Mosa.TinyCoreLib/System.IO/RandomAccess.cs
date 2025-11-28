using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO;

public static class RandomAccess
{
	public static void FlushToDisk(SafeFileHandle handle)
	{
		throw null;
	}

	public static long GetLength(SafeFileHandle handle)
	{
		throw null;
	}

	public static void SetLength(SafeFileHandle handle, long length)
	{
		throw null;
	}

	public static long Read(SafeFileHandle handle, IReadOnlyList<Memory<byte>> buffers, long fileOffset)
	{
		throw null;
	}

	public static int Read(SafeFileHandle handle, Span<byte> buffer, long fileOffset)
	{
		throw null;
	}

	public static ValueTask<long> ReadAsync(SafeFileHandle handle, IReadOnlyList<Memory<byte>> buffers, long fileOffset, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static ValueTask<int> ReadAsync(SafeFileHandle handle, Memory<byte> buffer, long fileOffset, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static void Write(SafeFileHandle handle, IReadOnlyList<ReadOnlyMemory<byte>> buffers, long fileOffset)
	{
	}

	public static void Write(SafeFileHandle handle, ReadOnlySpan<byte> buffer, long fileOffset)
	{
	}

	public static ValueTask WriteAsync(SafeFileHandle handle, IReadOnlyList<ReadOnlyMemory<byte>> buffers, long fileOffset, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static ValueTask WriteAsync(SafeFileHandle handle, ReadOnlyMemory<byte> buffer, long fileOffset, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
