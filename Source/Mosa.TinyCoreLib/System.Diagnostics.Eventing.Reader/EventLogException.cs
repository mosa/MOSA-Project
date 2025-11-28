using System.Runtime.Serialization;

namespace System.Diagnostics.Eventing.Reader;

public class EventLogException : Exception
{
	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public EventLogException()
	{
	}

	protected EventLogException(int errorCode)
	{
	}

	protected EventLogException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public EventLogException(string message)
	{
	}

	public EventLogException(string message, Exception innerException)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
