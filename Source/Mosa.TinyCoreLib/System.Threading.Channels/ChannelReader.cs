using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace System.Threading.Channels;

public abstract class ChannelReader<T>
{
	public virtual bool CanCount
	{
		get
		{
			throw null;
		}
	}

	public virtual bool CanPeek
	{
		get
		{
			throw null;
		}
	}

	public virtual Task Completion
	{
		get
		{
			throw null;
		}
	}

	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	public virtual IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual bool TryPeek([MaybeNullWhen(false)] out T item)
	{
		throw null;
	}

	public abstract bool TryRead([MaybeNullWhen(false)] out T item);

	public abstract ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken));
}
