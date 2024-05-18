using System.Runtime.Serialization;

namespace System.Diagnostics.Eventing.Reader;

public class EventLogProviderDisabledException : EventLogException
{
	public EventLogProviderDisabledException()
	{
	}

	protected EventLogProviderDisabledException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public EventLogProviderDisabledException(string message)
	{
	}

	public EventLogProviderDisabledException(string message, Exception innerException)
	{
	}
}
