using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public abstract class Stream : MarshalByRefObject, IAsyncDisposable, IDisposable
{
	public static readonly Stream Null;

	public abstract bool CanRead { get; }

	public abstract bool CanSeek { get; }

	public virtual bool CanTimeout
	{
		get
		{
			throw null;
		}
	}

	public abstract bool CanWrite { get; }

	public abstract long Length { get; }

	public abstract long Position { get; set; }

	public virtual int ReadTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual int WriteTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public virtual void Close()
	{
	}

	public void CopyTo(Stream destination)
	{
	}

	public virtual void CopyTo(Stream destination, int bufferSize)
	{
	}

	public Task CopyToAsync(Stream destination)
	{
		throw null;
	}

	public Task CopyToAsync(Stream destination, int bufferSize)
	{
		throw null;
	}

	public virtual Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task CopyToAsync(Stream destination, CancellationToken cancellationToken)
	{
		throw null;
	}

	[Obsolete("CreateWaitHandle has been deprecated. Use the ManualResetEvent(false) constructor instead.")]
	protected virtual WaitHandle CreateWaitHandle()
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual ValueTask DisposeAsync()
	{
		throw null;
	}

	public virtual int EndRead(IAsyncResult asyncResult)
	{
		throw null;
	}

	public virtual void EndWrite(IAsyncResult asyncResult)
	{
	}

	public abstract void Flush();

	public Task FlushAsync()
	{
		throw null;
	}

	public virtual Task FlushAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	[Obsolete("Do not call or override this method.")]
	protected virtual void ObjectInvariant()
	{
	}

	public abstract int Read(byte[] buffer, int offset, int count);

	public virtual int Read(Span<byte> buffer)
	{
		throw null;
	}

	public Task<int> ReadAsync(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public virtual Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		throw null;
	}

	public virtual ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public int ReadAtLeast(Span<byte> buffer, int minimumBytes, bool throwOnEndOfStream = true)
	{
		throw null;
	}

	public ValueTask<int> ReadAtLeastAsync(Memory<byte> buffer, int minimumBytes, bool throwOnEndOfStream = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual int ReadByte()
	{
		throw null;
	}

	public void ReadExactly(byte[] buffer, int offset, int count)
	{
	}

	public void ReadExactly(Span<byte> buffer)
	{
	}

	public ValueTask ReadExactlyAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask ReadExactlyAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public abstract long Seek(long offset, SeekOrigin origin);

	public abstract void SetLength(long value);

	public static Stream Synchronized(Stream stream)
	{
		throw null;
	}

	protected static void ValidateBufferArguments(byte[] buffer, int offset, int count)
	{
	}

	protected static void ValidateCopyToArguments(Stream destination, int bufferSize)
	{
	}

	public abstract void Write(byte[] buffer, int offset, int count);

	public virtual void Write(ReadOnlySpan<byte> buffer)
	{
	}

	public Task WriteAsync(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public virtual Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		throw null;
	}

	public virtual ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual void WriteByte(byte value)
	{
	}
}
