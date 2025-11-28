using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO.IsolatedStorage;

public class IsolatedStorageFileStream : FileStream
{
	public override bool CanRead
	{
		get
		{
			throw null;
		}
	}

	public override bool CanSeek
	{
		get
		{
			throw null;
		}
	}

	public override bool CanWrite
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("IsolatedStorageFileStream.Handle has been deprecated. Use IsolatedStorageFileStream's SafeFileHandle property instead.")]
	public override IntPtr Handle
	{
		get
		{
			throw null;
		}
	}

	public override bool IsAsync
	{
		get
		{
			throw null;
		}
	}

	public override long Length
	{
		get
		{
			throw null;
		}
	}

	public override long Position
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override SafeFileHandle SafeFileHandle
	{
		get
		{
			throw null;
		}
	}

	public IsolatedStorageFileStream(string path, FileMode mode)
		: base(null, (FileAccess)0)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access)
		: base(null, (FileAccess)0)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share)
		: base(null, (FileAccess)0)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
		: base(null, (FileAccess)0)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, IsolatedStorageFile? isf)
		: base(null, (FileAccess)0)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, IsolatedStorageFile? isf)
		: base(null, (FileAccess)0)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, IsolatedStorageFile? isf)
		: base(null, (FileAccess)0)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, IsolatedStorageFile? isf)
		: base(null, (FileAccess)0)
	{
	}

	public override IAsyncResult BeginRead(byte[] array, int offset, int numBytes, AsyncCallback? userCallback, object? stateObject)
	{
		throw null;
	}

	public override IAsyncResult BeginWrite(byte[] array, int offset, int numBytes, AsyncCallback? userCallback, object? stateObject)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override ValueTask DisposeAsync()
	{
		throw null;
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		throw null;
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
	}

	public override void Flush()
	{
	}

	public override void Flush(bool flushToDisk)
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	[UnsupportedOSPlatform("macos")]
	public override void Lock(long position, long length)
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public override int Read(Span<byte> buffer)
	{
		throw null;
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override int ReadByte()
	{
		throw null;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw null;
	}

	public override void SetLength(long value)
	{
	}

	[UnsupportedOSPlatform("macos")]
	public override void Unlock(long position, long length)
	{
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
	}

	public override void Write(ReadOnlySpan<byte> buffer)
	{
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override void WriteByte(byte value)
	{
	}
}
