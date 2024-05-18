using System.Runtime.Serialization;

namespace System.Diagnostics.Eventing.Reader;

public class EventLogNotFoundException : EventLogException
{
	public EventLogNotFoundException()
	{
	}

	protected EventLogNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public EventLogNotFoundException(string message)
	{
	}

	public EventLogNotFoundException(string message, Exception innerException)
	{
	}
}
