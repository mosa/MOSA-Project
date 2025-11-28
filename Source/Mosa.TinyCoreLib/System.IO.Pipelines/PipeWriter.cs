using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines;

public abstract class PipeWriter : IBufferWriter<byte>
{
	public virtual bool CanGetUnflushedBytes
	{
		get
		{
			throw null;
		}
	}

	public virtual long UnflushedBytes
	{
		get
		{
			throw null;
		}
	}

	public abstract void Advance(int bytes);

	public virtual Stream AsStream(bool leaveOpen = false)
	{
		throw null;
	}

	public abstract void CancelPendingFlush();

	public abstract void Complete(Exception? exception = null);

	public virtual ValueTask CompleteAsync(Exception? exception = null)
	{
		throw null;
	}

	protected internal virtual Task CopyFromAsync(Stream source, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static PipeWriter Create(Stream stream, StreamPipeWriterOptions? writerOptions = null)
	{
		throw null;
	}

	public abstract ValueTask<FlushResult> FlushAsync(CancellationToken cancellationToken = default(CancellationToken));

	public abstract Memory<byte> GetMemory(int sizeHint = 0);

	public abstract Span<byte> GetSpan(int sizeHint = 0);

	[Obsolete("OnReaderCompleted has been deprecated and may not be invoked on all implementations of PipeWriter.")]
	public virtual void OnReaderCompleted(Action<Exception?, object?> callback, object? state)
	{
	}

	public virtual ValueTask<FlushResult> WriteAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
