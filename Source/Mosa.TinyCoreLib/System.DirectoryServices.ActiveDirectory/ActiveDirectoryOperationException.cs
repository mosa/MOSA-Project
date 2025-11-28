using System.Runtime.Serialization;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectoryOperationException : Exception, ISerializable
{
	public int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectoryOperationException()
	{
	}

	protected ActiveDirectoryOperationException(SerializationInfo info, StreamingContext context)
	{
	}

	public ActiveDirectoryOperationException(string? message)
	{
	}

	public ActiveDirectoryOperationException(string? message, Exception? inner)
	{
	}

	public ActiveDirectoryOperationException(string? message, Exception? inner, int errorCode)
	{
	}

	public ActiveDirectoryOperationException(string? message, int errorCode)
	{
	}

	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
