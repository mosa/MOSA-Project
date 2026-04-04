using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public abstract class Stream : MarshalByRefObject, IAsyncDisposable, IDisposable
{
	public static readonly Stream Null;

	public abstract bool CanRead { get; }

	public abstract bool CanSeek { get; }

	public virtual bool CanTimeout => false;

	public abstract bool CanWrite { get; }

	public abstract long Length { get; }

	public abstract long Position { get; set; }

	public virtual int ReadTimeout
	{
		get
		{
			Internal.Exceptions.Generic.InvalidOperation();
			return 0;
		}
		set
		{
			_ = value;
			Internal.Exceptions.Generic.InvalidOperation();
		}
	}

	public virtual int WriteTimeout
	{
		get
		{
			Internal.Exceptions.Generic.InvalidOperation();
			return 0;
		}
		set
		{
			_ = value;
			Internal.Exceptions.Generic.InvalidOperation();
		}
	}

	internal bool closed;

	public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) => throw new NotImplementedException();

	public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) => throw new NotImplementedException();

	public virtual void Close()
	{
		Dispose(true);
		closed = true;
	}

	public void CopyTo(Stream destination)
	{
		if (!CanRead || !destination.CanWrite)
			Internal.Exceptions.Generic.NotSupported();

		if (closed)
			Internal.Exceptions.Generic.ObjectDisposed("source");

		var buffer = new byte[Internal.Impl.Stream.CopyBufferSize];

		int read;
		while ((read = Read(buffer, 0, buffer.Length)) > 0)
			destination.Write(buffer, 0, read);
	}

	public virtual void CopyTo(Stream destination, int bufferSize) => throw new NotImplementedException();

	public Task CopyToAsync(Stream destination) => throw new NotImplementedException();

	public Task CopyToAsync(Stream destination, int bufferSize) => throw new NotImplementedException();

	public virtual Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) => throw new NotImplementedException();

	public Task CopyToAsync(Stream destination, CancellationToken cancellationToken) => throw new NotImplementedException();

	[Obsolete("CreateWaitHandle has been deprecated. Use the ManualResetEvent(false) constructor instead.")]
	protected virtual WaitHandle CreateWaitHandle() => throw new NotImplementedException();

	public void Dispose() => Close();

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
			Flush();
	}

	public virtual ValueTask DisposeAsync() => throw new NotImplementedException();

	public virtual int EndRead(IAsyncResult asyncResult) => throw new NotImplementedException();

	public virtual void EndWrite(IAsyncResult asyncResult) => throw new NotImplementedException();

	public abstract void Flush();

	public Task FlushAsync() => throw new NotImplementedException();

	public virtual Task FlushAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

	[Obsolete("Do not call or override this method.")]
	protected virtual void ObjectInvariant() => throw new NotImplementedException();

	public abstract int Read(byte[] buffer, int offset, int count);

	public virtual int Read(Span<byte> buffer) => throw new NotImplementedException();

	public Task<int> ReadAsync(byte[] buffer, int offset, int count) => throw new NotImplementedException();

	public virtual Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => throw new NotImplementedException();

	public virtual ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken)) => throw new NotImplementedException();

	public int ReadAtLeast(Span<byte> buffer, int minimumBytes, bool throwOnEndOfStream = true) => throw new NotImplementedException();

	public ValueTask<int> ReadAtLeastAsync(Memory<byte> buffer, int minimumBytes, bool throwOnEndOfStream = true, CancellationToken cancellationToken = default(CancellationToken)) => throw new NotImplementedException();

	public virtual int ReadByte() => throw new NotImplementedException();

	public void ReadExactly(byte[] buffer, int offset, int count) => throw new NotImplementedException();

	public void ReadExactly(Span<byte> buffer) => throw new NotImplementedException();

	public ValueTask ReadExactlyAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default(CancellationToken)) => throw new NotImplementedException();

	public ValueTask ReadExactlyAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken)) => throw new NotImplementedException();

	public abstract long Seek(long offset, SeekOrigin origin);

	public abstract void SetLength(long value);

	public static Stream Synchronized(Stream stream) => throw new NotImplementedException();

	protected static void ValidateBufferArguments(byte[] buffer, int offset, int count) => throw new NotImplementedException();

	protected static void ValidateCopyToArguments(Stream destination, int bufferSize) => throw new NotImplementedException();

	public abstract void Write(byte[] buffer, int offset, int count);

	public virtual void Write(ReadOnlySpan<byte> buffer) => throw new NotImplementedException();

	public Task WriteAsync(byte[] buffer, int offset, int count) => throw new NotImplementedException();

	public virtual Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => throw new NotImplementedException();

	public virtual ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken)) => throw new NotImplementedException();

	public virtual void WriteByte(byte value) => throw new NotImplementedException();
}
