using System.ComponentModel;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO;

public class FileStream : Stream
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

	[Obsolete("FileStream.Handle has been deprecated. Use FileStream's SafeFileHandle property instead.")]
	public virtual IntPtr Handle
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsAsync
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

	public virtual string Name
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

	public virtual SafeFileHandle SafeFileHandle
	{
		get
		{
			throw null;
		}
	}

	public FileStream(SafeFileHandle handle, FileAccess access)
	{
	}

	public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize)
	{
	}

	public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This constructor has been deprecated. Use FileStream(SafeFileHandle handle, FileAccess access) instead.")]
	public FileStream(IntPtr handle, FileAccess access)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This constructor has been deprecated. Use FileStream(SafeFileHandle handle, FileAccess access) and optionally make a new SafeFileHandle with ownsHandle=false if needed instead.")]
	public FileStream(IntPtr handle, FileAccess access, bool ownsHandle)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This constructor has been deprecated. Use FileStream(SafeFileHandle handle, FileAccess access, int bufferSize) and optionally make a new SafeFileHandle with ownsHandle=false if needed instead.")]
	public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This constructor has been deprecated. Use FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) and optionally make a new SafeFileHandle with ownsHandle=false if needed instead.")]
	public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
	{
	}

	public FileStream(string path, FileMode mode)
	{
	}

	public FileStream(string path, FileMode mode, FileAccess access)
	{
	}

	public FileStream(string path, FileMode mode, FileAccess access, FileShare share)
	{
	}

	public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
	{
	}

	public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
	{
	}

	public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
	{
	}

	public FileStream(string path, FileStreamOptions options)
	{
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
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

	~FileStream()
	{
	}

	public override void Flush()
	{
	}

	public virtual void Flush(bool flushToDisk)
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("macos")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("freebsd")]
	public virtual void Lock(long position, long length)
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

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("macos")]
	[UnsupportedOSPlatform("tvos")]
	[UnsupportedOSPlatform("freebsd")]
	public virtual void Unlock(long position, long length)
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
