using System.Runtime.Serialization;

namespace System.DirectoryServices.Protocols;

public class TlsOperationException : DirectoryOperationException
{
	public TlsOperationException()
	{
	}

	public TlsOperationException(DirectoryResponse response)
	{
	}

	public TlsOperationException(DirectoryResponse response, string message)
	{
	}

	public TlsOperationException(DirectoryResponse response, string message, Exception inner)
	{
	}

	protected TlsOperationException(SerializationInfo info, StreamingContext context)
	{
	}

	public TlsOperationException(string message)
	{
	}

	public TlsOperationException(string message, Exception inner)
	{
	}
}
