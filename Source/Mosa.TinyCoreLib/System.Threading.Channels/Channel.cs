namespace System.Threading.Channels;

public static class Channel
{
	public static Channel<T> CreateBounded<T>(int capacity)
	{
		throw null;
	}

	public static Channel<T> CreateBounded<T>(BoundedChannelOptions options)
	{
		throw null;
	}

	public static Channel<T> CreateBounded<T>(BoundedChannelOptions options, Action<T>? itemDropped)
	{
		throw null;
	}

	public static Channel<T> CreateUnbounded<T>()
	{
		throw null;
	}

	public static Channel<T> CreateUnbounded<T>(UnboundedChannelOptions options)
	{
		throw null;
	}
}
public abstract class Channel<T> : Channel<T, T>
{
}
public abstract class Channel<TWrite, TRead>
{
	public ChannelReader<TRead> Reader
	{
		get
		{
			throw null;
		}
		protected set
		{
		}
	}

	public ChannelWriter<TWrite> Writer
	{
		get
		{
			throw null;
		}
		protected set
		{
		}
	}

	public static implicit operator ChannelReader<TRead>(Channel<TWrite, TRead> channel)
	{
		throw null;
	}

	public static implicit operator ChannelWriter<TWrite>(Channel<TWrite, TRead> channel)
	{
		throw null;
	}
}
