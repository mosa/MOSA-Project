using System.Runtime.Serialization;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectoryServerDownException : Exception, ISerializable
{
	public int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectoryServerDownException()
	{
	}

	protected ActiveDirectoryServerDownException(SerializationInfo info, StreamingContext context)
	{
	}

	public ActiveDirectoryServerDownException(string? message)
	{
	}

	public ActiveDirectoryServerDownException(string? message, Exception? inner)
	{
	}

	public ActiveDirectoryServerDownException(string? message, Exception? inner, int errorCode, string? name)
	{
	}

	public ActiveDirectoryServerDownException(string? message, int errorCode, string? name)
	{
	}

	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
