using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Transactions;

public class TransactionManagerCommunicationException : TransactionException
{
	public TransactionManagerCommunicationException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TransactionManagerCommunicationException(SerializationInfo info, StreamingContext context)
	{
	}

	public TransactionManagerCommunicationException(string? message)
	{
	}

	public TransactionManagerCommunicationException(string? message, Exception? innerException)
	{
	}
}
