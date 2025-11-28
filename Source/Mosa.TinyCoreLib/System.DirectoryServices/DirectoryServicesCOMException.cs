using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.DirectoryServices;

public class DirectoryServicesCOMException : COMException, ISerializable
{
	public int ExtendedError
	{
		get
		{
			throw null;
		}
	}

	public string? ExtendedErrorMessage
	{
		get
		{
			throw null;
		}
	}

	public DirectoryServicesCOMException()
	{
	}

	protected DirectoryServicesCOMException(SerializationInfo info, StreamingContext context)
	{
	}

	public DirectoryServicesCOMException(string? message)
	{
	}

	public DirectoryServicesCOMException(string? message, Exception? inner)
	{
	}

	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
