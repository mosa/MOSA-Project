using System.Runtime.Serialization;

namespace System.ServiceProcess;

public class TimeoutException : SystemException
{
	public TimeoutException()
	{
	}

	protected TimeoutException(SerializationInfo info, StreamingContext context)
	{
	}

	public TimeoutException(string? message)
	{
	}

	public TimeoutException(string? message, Exception? innerException)
	{
	}
}
