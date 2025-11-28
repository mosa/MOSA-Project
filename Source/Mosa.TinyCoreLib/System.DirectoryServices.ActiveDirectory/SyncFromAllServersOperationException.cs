using System.Runtime.Serialization;

namespace System.DirectoryServices.ActiveDirectory;

public class SyncFromAllServersOperationException : ActiveDirectoryOperationException, ISerializable
{
	public SyncFromAllServersErrorInformation[] ErrorInformation
	{
		get
		{
			throw null;
		}
	}

	public SyncFromAllServersOperationException()
	{
	}

	protected SyncFromAllServersOperationException(SerializationInfo info, StreamingContext context)
	{
	}

	public SyncFromAllServersOperationException(string? message)
	{
	}

	public SyncFromAllServersOperationException(string? message, Exception? inner)
	{
	}

	public SyncFromAllServersOperationException(string? message, Exception? inner, SyncFromAllServersErrorInformation[]? errors)
	{
	}

	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
