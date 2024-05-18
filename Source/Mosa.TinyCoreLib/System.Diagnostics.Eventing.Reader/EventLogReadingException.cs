using System.Runtime.Serialization;

namespace System.Diagnostics.Eventing.Reader;

public class EventLogReadingException : EventLogException
{
	public EventLogReadingException()
	{
	}

	protected EventLogReadingException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public EventLogReadingException(string message)
	{
	}

	public EventLogReadingException(string message, Exception innerException)
	{
	}
}
