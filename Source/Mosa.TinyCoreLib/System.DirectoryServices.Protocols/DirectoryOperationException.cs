using System.Runtime.Serialization;

namespace System.DirectoryServices.Protocols;

public class DirectoryOperationException : DirectoryException, ISerializable
{
	public DirectoryResponse Response
	{
		get
		{
			throw null;
		}
	}

	public DirectoryOperationException()
	{
	}

	public DirectoryOperationException(DirectoryResponse response)
	{
	}

	public DirectoryOperationException(DirectoryResponse response, string message)
	{
	}

	public DirectoryOperationException(DirectoryResponse response, string message, Exception inner)
	{
	}

	protected DirectoryOperationException(SerializationInfo info, StreamingContext context)
	{
	}

	public DirectoryOperationException(string message)
	{
	}

	public DirectoryOperationException(string message, Exception inner)
	{
	}

	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
