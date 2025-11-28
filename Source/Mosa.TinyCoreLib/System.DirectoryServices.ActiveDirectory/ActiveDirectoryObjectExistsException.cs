using System.Runtime.Serialization;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectoryObjectExistsException : Exception
{
	public ActiveDirectoryObjectExistsException()
	{
	}

	protected ActiveDirectoryObjectExistsException(SerializationInfo info, StreamingContext context)
	{
	}

	public ActiveDirectoryObjectExistsException(string? message)
	{
	}

	public ActiveDirectoryObjectExistsException(string? message, Exception? inner)
	{
	}
}
