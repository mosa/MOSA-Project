using System.Runtime.Serialization;

namespace System.DirectoryServices.Protocols;

public class DirectoryException : Exception
{
	public DirectoryException()
	{
	}

	protected DirectoryException(SerializationInfo info, StreamingContext context)
	{
	}

	public DirectoryException(string message)
	{
	}

	public DirectoryException(string message, Exception inner)
	{
	}
}
