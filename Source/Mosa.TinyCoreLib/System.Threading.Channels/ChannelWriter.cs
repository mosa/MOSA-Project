using System.Threading.Tasks;

namespace System.Threading.Channels;

public abstract class ChannelWriter<T>
{
	public void Complete(Exception? error = null)
	{
	}

	public virtual bool TryComplete(Exception? error = null)
	{
		throw null;
	}

	public abstract bool TryWrite(T item);

	public abstract ValueTask<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken));

	public virtual ValueTask WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
