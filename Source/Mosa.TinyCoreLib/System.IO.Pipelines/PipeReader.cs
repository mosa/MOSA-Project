using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines;

public abstract class PipeReader
{
	public abstract void AdvanceTo(SequencePosition consumed);

	public abstract void AdvanceTo(SequencePosition consumed, SequencePosition examined);

	public virtual Stream AsStream(bool leaveOpen = false)
	{
		throw null;
	}

	public abstract void CancelPendingRead();

	public abstract void Complete(Exception? exception = null);

	public virtual ValueTask CompleteAsync(Exception? exception = null)
	{
		throw null;
	}

	public virtual Task CopyToAsync(PipeWriter destination, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task CopyToAsync(Stream destination, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static PipeReader Create(ReadOnlySequence<byte> sequence)
	{
		throw null;
	}

	public static PipeReader Create(Stream stream, StreamPipeReaderOptions? readerOptions = null)
	{
		throw null;
	}

	[Obsolete("OnWriterCompleted has been deprecated and may not be invoked on all implementations of PipeReader.")]
	public virtual void OnWriterCompleted(Action<Exception?, object?> callback, object? state)
	{
	}

	public abstract ValueTask<ReadResult> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));

	public ValueTask<ReadResult> ReadAtLeastAsync(int minimumSize, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	protected virtual ValueTask<ReadResult> ReadAtLeastAsyncCore(int minimumSize, CancellationToken cancellationToken)
	{
		throw null;
	}

	public abstract bool TryRead(out ReadResult result);
}
