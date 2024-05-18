using System.Runtime.Serialization;

namespace System.Diagnostics.Eventing.Reader;

public class EventLogInvalidDataException : EventLogException
{
	public EventLogInvalidDataException()
	{
	}

	protected EventLogInvalidDataException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public EventLogInvalidDataException(string message)
	{
	}

	public EventLogInvalidDataException(string message, Exception innerException)
	{
	}
}
