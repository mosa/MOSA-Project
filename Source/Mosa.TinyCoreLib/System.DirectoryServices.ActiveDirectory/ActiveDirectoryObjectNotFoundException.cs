using System.Runtime.Serialization;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectoryObjectNotFoundException : Exception, ISerializable
{
	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public Type? Type
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectoryObjectNotFoundException()
	{
	}

	protected ActiveDirectoryObjectNotFoundException(SerializationInfo info, StreamingContext context)
	{
	}

	public ActiveDirectoryObjectNotFoundException(string? message)
	{
	}

	public ActiveDirectoryObjectNotFoundException(string? message, Exception? inner)
	{
	}

	public ActiveDirectoryObjectNotFoundException(string? message, Type? type, string? name)
	{
	}

	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
