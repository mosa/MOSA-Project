using System.Runtime.Serialization;

namespace System.Security.Policy;

public class PolicyException : SystemException
{
	public PolicyException()
	{
	}

	protected PolicyException(SerializationInfo info, StreamingContext context)
	{
	}

	public PolicyException(string message)
	{
	}

	public PolicyException(string message, Exception exception)
	{
	}
}
