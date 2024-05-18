using System.Runtime.Serialization;

namespace System.Threading.Channels;

public class ChannelClosedException : InvalidOperationException
{
	protected ChannelClosedException(SerializationInfo info, StreamingContext context)
	{
	}

	public ChannelClosedException()
	{
	}

	public ChannelClosedException(Exception? innerException)
	{
	}

	public ChannelClosedException(string? message)
	{
	}

	public ChannelClosedException(string? message, Exception? innerException)
	{
	}
}
